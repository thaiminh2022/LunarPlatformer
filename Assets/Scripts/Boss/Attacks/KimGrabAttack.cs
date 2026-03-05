using UnityEngine;

public class KimGrabAttack : KimJongGoonState
{
    [SerializeField] private float _startWindupTime = 2f;
    private float _windupTime;
    private bool _playGrabed = false;
    public override void EnterState()
    {
        _playGrabed = false;
        _windupTime = _startWindupTime;
    }

    private void Grab()
    {
        // Move to player 
        // If player get hit
        // Do grab
    }

    private void Update()
    {
        if (_windupTime <= 0 && !_playGrabed)
        {
            // Do grab
            Grab();
            _playGrabed = true;
        }
        else
        {
            _windupTime -= Time.deltaTime;
        }
    }
}
