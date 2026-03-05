
using System;
using System.Collections.Generic;
using UnityEngine;

public class KimThinkingState : KimJongGoonState
{
    [SerializeField] private float _thinkingTime = 2f;
    [SerializeField] private Transform _thinkingPosition;
    private float _thinkingTimer;

    private int? _comboIdx = null;
    private readonly List<KimJongGoonState> _currentCombo = new();

    private List<List<KimJongGoonState>> _availableCombos;

    public override void Setup(KimJongGoonStateMachine stateMachine, KimStateArg args = null)
    {
        base.Setup(stateMachine, args);
        _availableCombos = new List<List<KimJongGoonState>>()
        {
            new()
            {
                sm.kimFirework,
                sm.kimSuperGroundSlam,
                sm.kimShadowClone,
                sm.kimVoiceAttack
            },
        };
    }

    public override void EnterState()
    {
        sm.kimAnim.PlayThinking();

        LeanTween.move(sm.gameObject, _thinkingPosition.position, 1f)
            .setEaseOutQuint()
            .setOnComplete(() =>
            {   
                if (_comboIdx != null)
                {
                    // move on to next attack
                    _comboIdx++;
                    if (_comboIdx < _currentCombo.Count)
                    {
                        sm.SwitchState(_currentCombo[_comboIdx.Value]);
                        return;
                    }
     
                    _currentCombo.Clear();
                    _comboIdx = null;
                }
                _thinkingTimer = _thinkingTime;
            });
    }
    private void Update()
    {
        if (_thinkingTimer > 0)
        {
            _thinkingTimer -= Time.deltaTime;
        }else if (_comboIdx == null)
        {
            ChooseAttack();
        }
    }
    private void ChooseAttack()
    {
        var attacks = _availableCombos[0];
        StartCombo(attacks);
    }

    private void StartCombo(List<KimJongGoonState> attacks)
    {
        if (_currentCombo.Count > 0)
            return;
        
        _currentCombo.AddRange(attacks);
        _comboIdx = 0;
        sm.SwitchState(attacks[_comboIdx.Value]);
    }
}
