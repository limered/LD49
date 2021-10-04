using SystemBase;
using Systems.Player.Events;
using UniRx;

namespace Systems.Ui
{
    [GameSystem]
    public class CurtainSystem : GameSystem<CurtainComponent>
    {
        public override void Register(CurtainComponent component)
        {
            MessageBroker.Default.Receive<PlayerEnterExitEvent>()
                .Subscribe(e => component.GoToNextScene())
                .AddTo(component);
        }
    }
}