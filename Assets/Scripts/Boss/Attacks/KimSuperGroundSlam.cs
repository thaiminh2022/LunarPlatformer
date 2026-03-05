using UnityEditor;
using UnityEngine;

public class KimSuperGroundSlam : KimJongGoonState
{
    [SerializeField] private float _groundCheckDistance = 20f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _damageLayer;
    [SerializeField] private float _slamDamageRadius = 3f;
    [SerializeField] private float _stunTime = 2f;
    [SerializeField] private BossEnvironmentManager _envManager;

    [SerializeField] private float _startTimeBeforeSlam = 3f;
    private float _timeBeforeSlam;
    Vector2 _slamPosition;

    public override void EnterState()
    {
        _timeBeforeSlam = _startTimeBeforeSlam;

        // move to center
        var hit =  Physics2D.Raycast(transform.position, Vector2.down,  _groundCheckDistance, _groundLayer);
        if (hit)
        {
            _slamPosition = hit.point;
        }else
        {
            _slamPosition = transform.position + Vector3.down  * _groundCheckDistance;  
        }
    }
    private void Update()
    {
        if (_timeBeforeSlam <= 0)
        {
            sm.transform.position = _slamPosition;
            Debug.Log(_slamPosition);
            var hit = Physics2D.OverlapCircle(transform.position, _slamDamageRadius, _damageLayer);
            if (hit != null && hit.TryGetComponent<IDamagable>(out var damagable))
            {
                damagable.Damage(1);
            }
            _envManager.SendMorePlatform();
            sm.SwitchState(sm.idleState, new KimIdleArg
            {
                IdleTime = _stunTime
            });
        }
        else
        {
            _timeBeforeSlam -= Time.deltaTime;
        }
    }
}