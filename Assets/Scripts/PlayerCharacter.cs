using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IDamagable
{


    [Header("Movement")]
    [SerializeField] float _moveSpeed = 8f;
    [SerializeField] float _acceleration = 12f;
    [SerializeField] float _deceleration = 15f;

    [Header("Movement/Dash")]
    [SerializeField] float _dashForce = 10f;
    [SerializeField] float _dashTime = 1f;

    [Header("Jump")]
    [SerializeField] float _jumpForce = 10f;
    [SerializeField] float _coyoteTime = 0.1f; // How much time before player can no longer jump when walk off ground
    [SerializeField] float _jumpBufferTime = 0.1f; // If player press jump while on air within this time, player auto jump when hit ground
    [SerializeField] float _jumpCutMultiplier = 0.5f; // If player release jump button early, make player jump shorter
    [SerializeField] float _commitTime = 0.2f; // How much time before movement input is lock


    [Header("Jump/GroundCheck")]
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundCheckRadius = 1;
    [SerializeField] LayerMask _groundLayer;

    [Header("Jump/Falling")]
    [SerializeField] float _fallGravityScale = 2.2f; // How heavy player is when fall
    

    [Header("Misc")]
    [SerializeField] PlayerAttackSystem _attackSystem;
    [SerializeField] private int _maxHealth;
    [SerializeField] Rigidbody2D _rb;

    private HealthSystem _healthSystem;

    // Data
    private float _inputX;
    private bool _queueJump;
    private bool _queueDash;
    private bool _jumpHeld;
    private bool _isGrounded;
    private bool _justBonk = false;
    private float _defaultGravityScale;
    
    // Timer
    private float _coyoteTimer;
    private float _bufferTimer;
    private float _commitTimer;
    private float _dashTimer;

    private void Awake()
    {
        _healthSystem = new HealthSystem(_maxHealth);
        _attackSystem.OnHitEnemyHead += AttackSystem_OnHitEnemyHead;
        _defaultGravityScale = _rb.gravityScale;
    }

    private void AttackSystem_OnHitEnemyHead()
    {
        _rb.linearVelocityY = 0;
        Jump();
        _justBonk = true;
    }

    private void Update()
    {

        _inputX = InputManager.GetMoveValue();
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer) != null;
        if (InputManager.GetJumpDown() && _queueJump == false)
        {
            _queueJump = true;
        }
          if (InputManager.GetDash() && _queueDash == false)
        {
            _queueDash =    true; 
        }
        _jumpHeld  = InputManager.GetJumpHeld();

        HandleFlip();

        if (_isGrounded) _coyoteTimer = _coyoteTime;
        else _coyoteTimer -= Time.deltaTime;

        if (_queueJump) _bufferTimer = _jumpBufferTime;
        else _bufferTimer -= Time.deltaTime;

        if (_isGrounded) _justBonk = false;

        if (_isGrounded || _justBonk) _commitTimer = _commitTime;
        else if (_commitTimer > 0) _commitTimer -= Time.deltaTime; 

        if(_dashTimer > 0) _dashTimer -= Time.deltaTime; 

    }
    private void FixedUpdate()      
    {
        HandleDash();
        Move();
        HandleJump();
        FallRise();
    }
    private void HandleDash()
    {
        if (!_queueDash || _dashTimer > 0)
        {
            _queueDash = false;
            return;
        }

        _dashTimer = _dashTime;
        _queueDash = false;



        var dir = transform.localScale.x;
        _rb.linearVelocityX = 0;
        _rb.AddForce(_dashForce * dir * Vector2.right, ForceMode2D.Impulse);
    }
    private void HandleJump()
    {
        if (_coyoteTimer > 0 && _bufferTimer > 0) {
            _bufferTimer = 0f;
            _coyoteTimer = 0f;
            _queueJump = false;

            _commitTimer = _commitTime;
            Jump();
        }


        _queueJump = false;
        if (!_jumpHeld && _rb.linearVelocityY > 0.01f && !_justBonk)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y * _jumpCutMultiplier);
        }
    }   
    private void Jump() 
    {
        _rb.linearVelocityY = 0;
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }
    private void FallRise()
    {
        if (_rb.linearVelocity.y < -0.01f)
            _rb.gravityScale = _fallGravityScale;
        else
            _rb.gravityScale = _defaultGravityScale;
    }

    private void Move()
    {
        if (_commitTimer <= 0)
            return;

        var currentX = _rb.linearVelocityX;
        var targetX = _moveSpeed * _inputX;     
        float rate = _inputX != 0 ? _acceleration : _deceleration;
        
        currentX = Mathf.MoveTowards(currentX, targetX, rate * Time.fixedDeltaTime);
        _rb.linearVelocityX = currentX;
    }

    private void HandleFlip()
    {
        var scale = transform.localScale;
        if (_inputX > 0)
        {
            scale.x = 1;
        }
        else if (_inputX < 0)
        {
            scale.x = -1;
        }
        transform.localScale = scale;
    }

    public void Damage(int amount)
    {
        _healthSystem.Damage(amount);
    }

    public float GetMoveX() => _inputX;
    public Vector2 GetVelocity() => _rb.linearVelocity;
    public bool Dashed() => _queueDash && InputManager.GetDash(); 
}
