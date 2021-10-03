using SystemBase;
using Systems.Player.Events;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems.Ui
{
    [GameSystem]
    public class UiSystem : GameSystem<UiComponent>
    {
        public override void Register(UiComponent component)
        {
            MessageBroker.Default.Receive<PlayerSwayUpdateEvent>()
                .Subscribe(e => component.fallSlider.value = e.SwayPercent)
                .AddTo(component);

            MessageBroker.Default.Receive<PlayerPukeUpdateEvent>()
                .Subscribe(e => component.pukeSlider.value = e.PukePercent)
                .AddTo(component);
        }
    }
}