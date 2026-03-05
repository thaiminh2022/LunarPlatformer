using System;
using UnityEngine;

public class KimVoiceAttack: KimJongGoonState
{
    [SerializeField] private VoiceProjectile _voiceGameObject;
    [SerializeField] private float _startShootDelay = 5;

    private float _shootDelay;
    
    public override void EnterState()
    {
        _shootDelay = _startShootDelay;
    }

    private void Update()
    {
        if (_shootDelay <= 0)
        {
            var dir = (sm.player.transform.position - sm.transform.position).normalized;
            var spawnPosition = sm.transform.position + (-dir * 10);
            var go = Instantiate(_voiceGameObject, spawnPosition, Quaternion.identity);
            go.Setup(dir);
            sm.SwitchState(sm.idleState);
        }
        else
        {
            _shootDelay -= Time.deltaTime;
        }
        
    }
}
