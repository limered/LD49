using System;
using System.Linq;
using Assets.Utils.Math;
using SystemBase.StateMachineBase;
using Systems.Environment;
using Systems.Player.Events;
using UniRx;
using Object = UnityEngine.Object;

namespace Systems.Player.States
{
    [NextValidStates(typeof(PlayerStateNormal))]
    public class PlayerStateDrinking: BaseState<PlayerBrainComponent>
    {
        public override void Enter(StateContext<PlayerBrainComponent> context)
        {
            var player = context.Owner;
            var (nearestDrink, distanceToPlayer) = Object.FindObjectsOfType<DrinkComponent>()
                .Select(o => (o, player.transform.position.DistanceTo(o.transform.position)))
                .OrderBy(t => t.Item2)
                .FirstOrDefault();

            if (nearestDrink != null && distanceToPlayer < 2)
            {
                MessageBroker.Default.Publish(new PlayerDrinkEvent());

                context.Owner.criticalSwayFactor += 500;
                
                Observable.Timer(TimeSpan.FromMilliseconds(2000))
                    .Subscribe(_ => context.GoToState(new PlayerStateNormal()))
                    .AddTo(this);
            }
            else
            {
                context.GoToState(new PlayerStateNormal());
            }
        }
    }
}