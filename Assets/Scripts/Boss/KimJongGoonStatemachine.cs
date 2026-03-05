using UnityEngine;

public class KimJongGoonStateMachine : MonoBehaviour, IDamagable
{
    [Header("states")]
    public KimJongGoonState idleState;
    public KimJongGoonState thinkingState;
    public KimJongGoonState kimFirework;
    public KimJongGoonState kimGrabAttack;
    public KimJongGoonState kimShadowClone;
    public KimJongGoonState kimVoiceAttack;
    public KimJongGoonState kimSuperGroundSlam;


    

    [Header("Mechanics")]
    public KimAnimator kimAnim;
    public PlayerCharacter player;
    [SerializeField] private int _maxHealth = 10;
    

    private KimJongGoonState _currentState;
    private HealthSystem _healthSystem;

    private KimJongGoonState[] AllStates => new KimJongGoonState[] { 
        idleState, thinkingState, kimFirework, kimGrabAttack, kimShadowClone, kimVoiceAttack, kimSuperGroundSlam
    };

    public void Awake()
    {
        _healthSystem = new HealthSystem(_maxHealth);
        foreach (var state in AllStates) { 
            state.gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        SwitchState(idleState);
    }

    public void Damage(int amount)
    {
        _healthSystem.Damage(amount);
        if (_healthSystem.IsDead())
        {
            SwitchState(null);
            Destroy(gameObject);
        } 
    }

    public void SwitchState(KimJongGoonState state, KimStateArg args = null)
    {
        if (_currentState != null)
        {
            _currentState.ResetArg();
            _currentState.ExitState();
            _currentState.gameObject.SetActive(false);
        }
        _currentState = state;
        
        if (_currentState != null)
        {
            // must follow this flow or game breaks
            _currentState.Setup(this, args);
            _currentState.EnterState();
            _currentState.gameObject.SetActive(true);
        }
    }
}

public abstract class KimJongGoonState : MonoBehaviour
{
    protected KimJongGoonStateMachine sm;
    private KimStateArg args;

    protected TArgType GetArgs<TArgType>() where TArgType : KimStateArg 
    {
        return (TArgType)args;
    }

    public virtual void Setup(KimJongGoonStateMachine sm, KimStateArg args = null)
    {
        this.sm = sm;   
        this.args = args;
    }
    public void ResetArg()
    {
        args= null;
    }

    public virtual void ExitState() { }
    public virtual void EnterState() { }
}

public abstract class KimStateArg { };