using UnityEngine;

public class KimIdleState : KimJongGoonState
{
    [SerializeField]
    private float _idleTime = 1f;

    public override void EnterState()
    {
        sm.kimAnim.PlayIdle();
        var args = GetArgs<KimIdleArg>();
        if ( args != null)
        {
            _idleTime = args.IdleTime;
        }
    }

    private void Update()
    {
        if (_idleTime > 0) {
            _idleTime -= Time.deltaTime;
        }else
        {
            sm.SwitchState(sm.thinkingState);
        }
    }

}

public class KimIdleArg : KimStateArg
{
    public float IdleTime { get; set; }
}


