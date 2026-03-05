using UnityEngine;

public class VoiceProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private GameObject _vfx;

    private Vector2 _dir = Vector2.zero;

    public void Setup(Vector2 dir)
    {
        _dir = dir.normalized;  
        var rotationAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
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
        if (collision.TryGetComponent<IDamagable>(out var damageable))
        {
            damageable.Damage(1);
        }
        Explode(collision.transform.position);
    }

    private void Explode(Vector3 position)
    {
        var go = Instantiate(_vfx,position, Quaternion.identity);
        Destroy(go, 3f);
        Destroy(gameObject);
    }
}