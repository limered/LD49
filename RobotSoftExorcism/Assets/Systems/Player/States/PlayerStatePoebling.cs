using SystemBase.StateMachineBase;

namespace Systems.Player.States
{
    [NextValidStates(typeof(PlayerStateNormal))]
    public class PlayerStatePoebling: BaseState<PlayerBrainComponent>
    {
        public override void Enter(StateContext<PlayerBrainComponent> context)
        {
            throw new System.NotImplementedException();
        }
    }
}