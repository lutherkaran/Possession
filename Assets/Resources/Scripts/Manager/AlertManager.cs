using System;
using UnityEngine;

// Owns the shared "danger meter" and body-alert escalation for the abandoned-body threat.
// Guards report into this rather than tracking global alert state themselves, so any number
// of guards can contribute to (and be dispatched by) one UI-readable danger value.
//
// Flow: ReportBodySpotted (guard sees the body, telegraph "!") -> either ReportBodyLost
// (player hid/escaped in time -- decays back down) or EscalateAlert (reaction window expired,
// other guards get called in) -> RaiseDanger ticks up while the escalation continues ->
// hitting 1.0 ends the game via GameManager.TriggerGameOver().
public class AlertManager : IManagable
{
    private static AlertManager Instance;
    public static AlertManager instance { get { return Instance == null ? Instance = new AlertManager() : Instance; } }

    public event EventHandler<float> OnDangerChanged;   // 0..1, for a UI danger meter
    public event EventHandler OnBodySpotted;             // guard first sees the body ('!' telegraph)
    public event EventHandler OnAlertEscalated;          // reaction window expired, other guards called in
    public event EventHandler OnBodyLost;                // player successfully hid/escaped
    public event EventHandler OnGameOver;

    public float DangerMeter { get; private set; }
    public bool IsGameOver { get; private set; }

    private readonly float dangerDecayPerSecond = 0.08f;

    public void Initialize()
    {
        DangerMeter = 0f;
        IsGameOver = false;
    }

    public void PostInitialize() { }

    public void ReportBodySpotted(Vector3 position)
    {
        if (IsGameOver) return;
        OnBodySpotted?.Invoke(this, EventArgs.Empty);
    }

    public void EscalateAlert()
    {
        if (IsGameOver) return;
        OnAlertEscalated?.Invoke(this, EventArgs.Empty);
    }

    public void ReportBodyLost()
    {
        if (IsGameOver) return;
        OnBodyLost?.Invoke(this, EventArgs.Empty);
    }

    public void ReportAnimalCaptured()
    {
        // Hook point for UI/audio feedback and animal-bond tracking on the low-stakes capture path.
    }

    public void RaiseDanger(float amount)
    {
        if (IsGameOver) return;
        DangerMeter = Mathf.Clamp01(DangerMeter + amount);
        OnDangerChanged?.Invoke(this, DangerMeter);

        if (DangerMeter >= 1f)
        {
            TriggerGameOver();
        }
    }

    public void Refresh(float deltaTime)
    {
        if (IsGameOver) return;

        if (DangerMeter > 0f)
        {
            DangerMeter = Mathf.Clamp01(DangerMeter - dangerDecayPerSecond * deltaTime);
            OnDangerChanged?.Invoke(this, DangerMeter);
        }
    }

    private void TriggerGameOver()
    {
        IsGameOver = true;
        OnGameOver?.Invoke(this, EventArgs.Empty);
        GameManager.instance?.TriggerGameOver();
    }

    public void PhysicsRefresh(float fixedDeltaTime) { }
    public void LateRefresh(float deltaTime) { }

    public void OnDemolish()
    {
        Instance = null;
    }
}