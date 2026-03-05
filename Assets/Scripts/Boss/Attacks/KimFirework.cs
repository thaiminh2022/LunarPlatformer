using System;
using UnityEngine;
using Random = System.Random;
using UnityRandom = UnityEngine.Random;

public class KimFirework : KimJongGoonState
{
    [Header("AimShoot")]
    [SerializeField] private float _timeBtwAimShoot = .3f;
    [SerializeField] private int _totalAimShoot = 5;
 
    [Header("Side")]
    [SerializeField] private float _timeBtwSideShoot = 1f;
    [SerializeField] private int _totalSideShoot = 3;
    [SerializeField] private int _sweepAngle = 30;
    [SerializeField] private int _sideAmount = 5;


    [Header("Misc")]
    [SerializeField] private FireworkProjectile _firework;
    [SerializeField] private Transform _hand;

    private float _timeBtwShoot;
    private float _totalShootTime;

    private enum AttackTypes
    {
        AimShoot,
        SideLeft,
        SideRight,
    }
    private AttackTypes _attackType;

    

    public override void EnterState()
    {
        var max = Enum.GetValues(typeof(AttackTypes)).Length;
        var randAttack = UnityRandom.Range(0, max);
        _attackType = (AttackTypes)randAttack;

        _totalShootTime = _attackType switch
        {
            AttackTypes.AimShoot => _totalAimShoot,
            AttackTypes.SideLeft => _totalSideShoot,
            AttackTypes.SideRight => _totalSideShoot,
        };
    }
    private void ResetTime()
    {
        _timeBtwShoot = _attackType switch
        {
            AttackTypes.AimShoot => _timeBtwAimShoot,
            AttackTypes.SideLeft => _timeBtwSideShoot,
            AttackTypes.SideRight => _timeBtwSideShoot,
        };
    }

    private void Update()
    {
        if (_timeBtwShoot <= 0 && _totalShootTime > 0)
        {
            switch (_attackType)
            {
                case AttackTypes.AimShoot:
                    AimShoot();
                    break;
                case AttackTypes.SideLeft:
                    SideLeft();
                    break;
                case AttackTypes.SideRight:
                    SideRight();
                    break;
            
            }


            _totalShootTime--;
            ResetTime();
        }
        else
        {
            _timeBtwShoot -= Time.deltaTime;
        }

        if (_totalShootTime <= 0)
        {
            sm.SwitchState(sm.thinkingState);
        }
    }
    private void AimShoot()
    {
        int xOffset = UnityRandom.Range(0, 10);
        LeanTween.moveX(sm.gameObject, sm.transform.position.x + xOffset, .5f)
            .setEaseInOutBounce()
            .setOnComplete(() =>
            {
                var diff = (sm.player.transform.position - transform.position).normalized;
                var go = Instantiate(_firework, _hand.position, Quaternion.identity);
                go.Setup(diff);
            });
    }
    private void SideLeft()
    {
        var offset = Vector3.zero;
        var leftCenter = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, Camera.main.nearClipPlane));
        LeanTween.move(sm.gameObject, leftCenter + offset, 1f)
            .setEaseOutQuint()
            .setOnComplete(() =>
            {
                Vector2 baseDir = (sm.player.transform.position - sm.transform.position).normalized;

                var startAngle = -_sweepAngle / 2f;
                var angleStep = _sideAmount > 1 ? _sweepAngle / (_sideAmount - 1) : 0f;

                for (int i = 0; i < _sideAmount; i++)
                {
                    var angle = startAngle + angleStep * i;
                    var dir = Rotate(baseDir, angle);
                    var go = Instantiate(_firework, _hand.position, Quaternion.identity);
                    go.Setup(dir.normalized);
                }
            });
        
        

    }
    private void SideRight()
    {
        var offset = Vector3.zero;
        var rightCenter = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f, Camera.main.nearClipPlane));

        LeanTween.move(sm.gameObject, rightCenter + offset, 1f)
            .setEaseOutQuint()
            .setOnComplete(() =>
            {
                Vector2 baseDir = (sm.player.transform.position - sm.transform.position).normalized;

                float startAngle = -_sweepAngle / 2f;
                float angleStep = _sideAmount > 1 ? _sweepAngle / (_sideAmount - 1) : 0f;

                for (int i = 0; i < _sideAmount; i++)
                {
                    float angle = startAngle + angleStep * i;
                    Vector2 dir = Rotate(baseDir, angle);
                    var go = Instantiate(_firework, _hand.position, Quaternion.identity);
                    go.Setup(dir.normalized);
                }   
            });
    }
    private static Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }

}