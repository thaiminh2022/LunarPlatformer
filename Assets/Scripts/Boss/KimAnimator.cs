using UnityEngine;
public class KimAnimator : MonoBehaviour  
{
    [SerializeField] private Animator _anim;

    public const string IDLE = "idle";
    public const string THINKING = "thinking";
    public const string ATTACK = "attack";

    public void PlayIdle() => _anim.Play(IDLE);
    public void PlayThinking() => _anim.Play(THINKING);
    public void PlayAttack() => _anim.Play(ATTACK);

}
