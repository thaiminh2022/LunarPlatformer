
// hit point system that works with both player and enemies
using System;

[Serializable]
public class HealthSystem
{
    public int Health { get; set; }
    public int MaxHealth { get; set;  }

    public event Action OnDead;

    public HealthSystem(int maxHealth)
    {
        MaxHealth = maxHealth;
        Health = MaxHealth;
    }
    public void Damage(int amount)
    {
        if (Health <= 0)
            return;
        
        Health -= amount;

        if (Health <= 0)
        {
            OnDead?.Invoke();
        }
    }
    public void Heal(int amount)
    {
        Health += Math.Clamp(Health + amount, 0, MaxHealth);
    }
    public bool IsDead() => Health <= 0;

}