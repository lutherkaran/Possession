using System;

public interface IDamageable
{
    void HealthChanged(float healthChangedValue);

    public event EventHandler<OnDamagedEventArgs> OnDamaged;
    public class OnDamagedEventArgs : EventArgs
    {
        public float health;
        public float maxHealth;
    }
}
