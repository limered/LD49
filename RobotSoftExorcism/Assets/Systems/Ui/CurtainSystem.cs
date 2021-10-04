using SystemBase;
using Systems.Player.Events;
using UniRx;
using UnityEngine;

namespace Systems.Ui
{
    [GameSystem]
    public class CurtainSystem : GameSystem<CurtainComponent>
    {
        public override void Register(CurtainComponent component)
        {
            MessageBroker.Default.Receive<PlayerEnterExitEvent>()
                .Subscribe(e =>GoToNextScene(component))
                .AddTo(component);
        }

        private void GoToNextScene(CurtainComponent component)
        {
            component.GetComponent<Animator>().Play("close_curtain");
        }
    }
}