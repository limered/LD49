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
                .Subscribe(e => component.fallMeterText.text = $"Fall Meter: {e.SwayPercent:P}")
                .AddTo(component);

            MessageBroker.Default.Receive<PlayerPukeUpdateEvent>()
                .Subscribe(e => component.pukeMeterText.text = $"Puke Meter: {e.PukePercent:P}")
                .AddTo(component);
        }
    }
}