using System;
using SystemBase.StateMachineBase;
using Systems.Movement;
using Systems.Player.Events;
using UniRx;
using UnityEngine;

namespace Systems.Player.States
{
    [NextValidStates(typeof(PlayerStateNormal))]
    public class PlayerStatePuking: BaseState<PlayerBrainComponent>
    {
        public override void Enter(StateContext<PlayerBrainComponent> context)
        {
            var movement = context.Owner.GetComponent<MovementComponent>();
            movement.Velocity = Vector2.zero;
            movement.Direction.Value = Vector2.zero;
            movement.Acceleration = Vector2.zero;

            context.Owner.PukeFactor = 0;
            context.Owner.PukePercentage = 0;
            
            context.Owner.pukeParticles.Play();
            
            MessageBroker.Default.Publish(new PlayerPukeEvent());
            
            Observable.Timer(TimeSpan.FromMilliseconds(3000))
                .Subscribe(_ => {
                    context.Owner.pukeParticles.Stop();
                    context.GoToState(new PlayerStateNormal());
                })
                .AddTo(this);
        }
    }
}