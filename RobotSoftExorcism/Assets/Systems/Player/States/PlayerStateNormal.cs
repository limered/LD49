using SystemBase.StateMachineBase;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Player.States
{
    [NextValidStates(typeof(PlayerStateDrinking),
        typeof(PlayerStateFallen),
        typeof(PlayerStatePuking),
        typeof(PlayerStateKicking),
        typeof(PlayerStatePoebling))]
    public class PlayerStateNormal: BaseState<PlayerBrainComponent>
    {
        public override void Enter(StateContext<PlayerBrainComponent> context)
        {
            context.Owner.UpdateAsObservable()
                .Subscribe(_ => { CheckSpecialActions(context); })
                .AddTo(this);
        }

        private static void CheckSpecialActions(StateContext<PlayerBrainComponent> context)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                context.GoToState(new PlayerStateKicking());
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                context.GoToState(new PlayerStatePoebling());
            }
        }
    }
}