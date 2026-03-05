using UnityEngine;

public class FireworkProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed;

    private Vector2 _dir = Vector2.zero;

    public void Setup(Vector2 dir)
    {
        _dir = dir.normalized;  
        var offset = -90;
        var rotationAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationAngle + offset);
    }

    private void Awake()
    {
        Destroy(gameObject, 5f);
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _speed * Time.deltaTime * _dir);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamagable>(out var damagable))
        {
            damagable.Damage(1);
        }
        Explode();
    }

    private void Explode()
    {
        Destroy(gameObject);
    }

}