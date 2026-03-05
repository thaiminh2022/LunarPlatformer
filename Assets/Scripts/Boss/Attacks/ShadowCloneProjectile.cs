using System;
using UnityEngine;

public class ShadowCloneProjectile: MonoBehaviour, IDamagable
{
	[SerializeField] Rigidbody2D _rb;
    [SerializeField] float _speed = 10f;

    private Func<Vector2> _getDir;
    private Vector2 _moveDir;
    private bool _startMove = false;
    private bool _gotDir = false;
    private float _timeBeforeMove;
    public void StartMove()
    {
        _startMove = true;
    }

    public void SetDelay(float delay)
    {
        _timeBeforeMove= delay;
    }
    public void Damage(int amount)
    {
        Destroy(gameObject);
    }

    public void SetDir(Func<Vector2> getDir)
    {
        _getDir = getDir;
    }

    private void FixedUpdate()
    {
        if (!_startMove)
            return;

        if (_timeBeforeMove <= 0)
        {
            if (!_gotDir)
            {
                _moveDir = _getDir();
                _gotDir = true;
            }
            
            _rb.MovePosition(_rb.position + _speed * Time.deltaTime * _moveDir);
        }else
        {
            _timeBeforeMove -= Time.deltaTime;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<IDamagable>(out var damagable))
        {
            damagable.Damage(1);
        }
        Destroy(gameObject);
    }
}
