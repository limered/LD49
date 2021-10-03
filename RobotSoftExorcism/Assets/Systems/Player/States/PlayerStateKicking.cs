using System;
using System.Linq;
using Assets.Utils.Math;
using SystemBase.StateMachineBase;
using Systems.Environment;
using UniRx;
using Object = UnityEngine.Object;

namespace Systems.Player.States
{
    [NextValidStates(typeof(PlayerStateNormal))]
    public class PlayerStateKicking: BaseState<PlayerBrainComponent>
    {
        public override void Enter(StateContext<PlayerBrainComponent> context)
        {
            var kickables = Object.FindObjectsOfType<KickableComponent>()
                .Where(kickable => kickable.transform.position.DistanceTo(context.Owner.transform.position) < 2);
            
            foreach (var kickable in kickables)
            {
                kickable.HasBeenKickedTrigger.Execute();
            }

            Observable.Timer(TimeSpan.FromMilliseconds(500))
                .Subscribe(_ => context.GoToState(new PlayerStateNormal()))
                .AddTo(this);
        }
    }
}