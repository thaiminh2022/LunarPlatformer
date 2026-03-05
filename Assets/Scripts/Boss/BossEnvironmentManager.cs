using UnityEngine;

public class BossEnvironmentManager : MonoBehaviour
{
    [SerializeField] private Transform[] _morePlatforms;
    [SerializeField] private float _transitionTime = 0.5f;
    private Vector2[] _platformDefaultPositions;
    
    
    private void Awake()
    {
        _platformDefaultPositions = new Vector2[_morePlatforms.Length];

        for (var i = 0; i < _morePlatforms.Length; i++)
        {
            _platformDefaultPositions[i] = _morePlatforms[i].position;
            _morePlatforms[i].position = new Vector2(0, -50);
        }
    }

    public void SendRocketLauncher()
    {
    }

    public void SendMorePlatform()
    {
        for (var i = 0; i < _platformDefaultPositions.Length; i++)
        {
            var defaultPosition = _platformDefaultPositions[i];
            var targetTransform = _morePlatforms[i];

            LeanTween
                .move(targetTransform.gameObject, defaultPosition, _transitionTime)
                .setEase(LeanTweenType.easeOutCubic);
        }
    }
}
