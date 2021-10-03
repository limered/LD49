using SystemBase.StateMachineBase;
using UnityEngine;

namespace Systems.Player.States
{
    [NextValidStates(typeof(PlayerStateNormal))]
    public class PlayerStateKicking: BaseState<PlayerBrainComponent>
    {
        public override void Enter(StateContext<PlayerBrainComponent> context)
        {
            Debug.Log("Kick");
            context.GoToState(new PlayerStateNormal());
        }
    }
}