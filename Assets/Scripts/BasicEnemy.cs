using UnityEngine;

public class BasicEnemy : MonoBehaviour, IDamagable
{
    HealthSystem _healthSystem;

    private void Awake()
    {
        _healthSystem = new HealthSystem(1);
        _healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead()
    {
        Destroy(gameObject);
    }

    public void Damage(int amount)
    {
        _healthSystem.Damage(amount);
    }
}
