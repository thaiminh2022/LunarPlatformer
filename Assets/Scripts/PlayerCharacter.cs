using System;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private int _maxHealth;

    [Header("Movement")]
    [SerializeField] float _moveSpeed = 8f;
    [SerializeField] float _acceleration = 12f;
    [SerializeField] float _deceleration = 15f;

    [Header("Jump")]
    [SerializeField] float _jumpForce = 10f;
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundCheckRadius = 1;
    [SerializeField] LayerMask _groundLayer;
    

    [SerializeField] Rigidbody2D _rb;
    private HealthSystem _healthSystem;

    private float _inputValue;

    private void Awake()
    {
        _healthSystem = new HealthSystem(_maxHealth);
    }
    private void Update()
    {
        _inputValue = InputManager.GetMoveValue();
        HandleJump();
    }
    private void FixedUpdate()
    {
        HandleMove();
    }

    private void HandleJump()
    {
        if (!InputManager.GetJump())
            return;

        if (!IsGrounded())
            return;

        Debug.Log("Just jumped");
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private bool IsGrounded()
    {
        var col = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
        return col != null;
    }

    private void HandleMove()
    {
        var currentX = _rb.linearVelocityX;
        var targetX = _moveSpeed * _inputValue;
        float rate = _inputValue != 0 ? _acceleration : _deceleration;
        currentX = Mathf.Lerp(currentX, targetX, rate * Time.fixedDeltaTime);
        _rb.linearVelocityX = currentX;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    }
}
