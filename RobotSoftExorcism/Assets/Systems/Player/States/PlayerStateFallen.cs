﻿using System;
using SystemBase.StateMachineBase;
using Systems.Movement;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Player.States
{
    [NextValidStates(typeof(PlayerStateNormal), typeof(PlayerStateKicking))]
    public class PlayerStateFallen: BaseState<PlayerBrainComponent>
    {
        public override void Enter(StateContext<PlayerBrainComponent> context)
        {
            var movement = context.Owner.GetComponent<MovementComponent>();
            movement.Velocity = Vector2.zero;
            movement.Direction.Value = Vector2.zero;
            movement.Acceleration = Vector2.zero;
            
            var stopTimer = Observable.Timer(TimeSpan.FromMilliseconds(context.Owner.fallenDuration));
            
            context.Owner.UpdateAsObservable()
                .TakeUntil(stopTimer)
                .Where(_ => Input.GetKeyDown(KeyCode.K))
                .Subscribe(_ => context.GoToState(new PlayerStateKicking()))
                .AddTo(this);

            stopTimer
                .Subscribe(_ => context.GoToState(new PlayerStateNormal()))
                .AddTo(this);
        }
    }
}