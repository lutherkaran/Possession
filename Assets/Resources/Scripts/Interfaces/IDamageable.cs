using System;

public interface IDamageable
{
    void HealthChanged(float healthChangedValue);
    float GetMaxHealth();

    public event EventHandler<OnDamagedEventArgs> OnDamaged;
    public class OnDamagedEventArgs : EventArgs
    {
        public float health;
    }
}
