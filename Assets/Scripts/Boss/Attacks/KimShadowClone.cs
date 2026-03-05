using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class KimShadowClone : KimJongGoonState
{
    [SerializeField] private ShadowCloneProjectile _kimJongClone;
    [SerializeField] private int _cloneAmount = 4;
    [SerializeField] private float _distancePerClone = 5f;
    [SerializeField] private int _delayPerCloneSlam = 2;

    private float _totalWaitTimeBeforeChangeState;
    public override void EnterState()
    {
        List<Vector2> positions = new();

        for (var i = 0; i <= _cloneAmount / 2; i++)
        {
            if (i == 0)
                continue;

            var position = transform.position + new Vector3(i * _distancePerClone, 0);
            var nposition = transform.position + new Vector3(-i * _distancePerClone, 0);
            
            positions.Add(position);
            positions.Add(nposition);
        }
        _totalWaitTimeBeforeChangeState = _cloneAmount * _delayPerCloneSlam;

        var di = 1;
        while (positions.Count > 0) { 
            var randIdx = Random.Range(0, positions.Count);
            Vector3 position = positions[randIdx];
            var go = Instantiate(_kimJongClone, position, Quaternion.identity);
            
            go.SetDir(() =>
            {
                var dir = sm.player.transform.position - position;
                return dir.normalized;
            });
            go.SetDelay(di * _delayPerCloneSlam);
            go.StartMove(); 
            positions.RemoveAt(randIdx);
            di++;
        }
    }

    private void Update()
    {
        if (_totalWaitTimeBeforeChangeState <= 0)
        {
            sm.SwitchState(sm.thinkingState);
        }else
        {
            _totalWaitTimeBeforeChangeState -= Time.deltaTime;
        }
    }
}
