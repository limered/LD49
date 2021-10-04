using System;
using System.Linq;
using Assets.Utils.Math;
using SystemBase.StateMachineBase;
using Systems.Environment;
using Systems.Movement;
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
            player.GetComponent<MovementComponent>().Stop();
            
            var (nearestDrink, distanceToPlayer) = Object.FindObjectsOfType<DrinkComponent>()
                .Select(o => (o, player.transform.position.DistanceTo(o.transform.position)))
                .OrderBy(t => t.Item2)
                .FirstOrDefault();

            if (nearestDrink != null && distanceToPlayer < 1.5f)
            {
                MessageBroker.Default.Publish(new PlayerDrinkEvent());
                context.Owner.criticalSwayFactor += 500;
                Object.Destroy(nearestDrink.gameObject);
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