
// hit point system that works with both player and enemies
using System;

[Serializable]
public class HealthSystem
{
    public int Health { get; set; }
    public int MaxHealth { get; set;  }

    public HealthSystem(int maxHealth)
    {
        MaxHealth = maxHealth;
        Health = MaxHealth;
    }

}