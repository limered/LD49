using SystemBase.StateMachineBase;

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
            
        }
    }
}