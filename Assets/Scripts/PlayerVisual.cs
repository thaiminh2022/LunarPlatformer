using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    const string IDLE = "idle";
    const string RUN = "run";
    const string JUMP = "jump";
    const string DASH = "dash";


    [SerializeField] Animator _animator;
    [SerializeField] PlayerCharacter _character;

    private void Update()
    {
        if(_character.Dashed())
        {
            _animator.Play(DASH);
        }
        else if (Mathf.Abs(_character.GetVelocity().y) > 0.1f)
        {
            _animator.Play(JUMP);
        }
        else if (_character.GetMoveX() != 0)
        {
            _animator.Play(RUN);
        }else
        {
            _animator.Play(IDLE);
        }
    }

}
