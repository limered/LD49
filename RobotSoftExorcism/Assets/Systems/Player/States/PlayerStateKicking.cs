using System;
using System.Linq;
using Assets.Utils.Math;
using StrongSystems.Audio;
using SystemBase.StateMachineBase;
using Systems.Environment;
using Systems.Player.Events;
using Systems.SoundManagement;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Systems.Player.States
{
    [NextValidStates(typeof(PlayerStateNormal))]
    public class PlayerStateKicking: BaseState<PlayerBrainComponent>
    {
        public override void Enter(StateContext<PlayerBrainComponent> context)
        {
            var kickablesInRange = Object.FindObjectsOfType<KickableComponent>()
                .Where(kickable => IsInRange(context, kickable))
                .ToList();
            
            foreach (var kickable in kickablesInRange)
            {
                kickable.HasBeenKickedTrigger.Execute();
            }
            
            if (kickablesInRange.Any())
            {
                MessageBroker.Default.Publish(new PlayerKickEvent());
            }

            Observable.Timer(TimeSpan.FromMilliseconds(500))
                .Subscribe(_ => context.GoToState(new PlayerStateNormal()))
                .AddTo(this);
        }

        private static bool IsInRange(StateContext<PlayerBrainComponent> context, KickableComponent kickable)
        {
            return kickable.transform.position.DistanceTo(context.Owner.transform.position) < context.Owner.kickRange;
        }
    }
}