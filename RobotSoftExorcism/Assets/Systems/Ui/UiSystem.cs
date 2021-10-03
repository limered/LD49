using SystemBase;
using Systems.Player.Events;
using Systems.Score;
using UniRx;
using Utils.Plugins;

namespace Systems.Ui
{
    [GameSystem]
    public class UiSystem : GameSystem<UiComponent, ScoreComponent>
    {
        private readonly ReactiveProperty<UiComponent> ui = new ReactiveProperty<UiComponent>();

        public override void Register(UiComponent component)
        {
            ui.Value = component;
            
            MessageBroker.Default.Receive<PlayerSwayUpdateEvent>()
                .Subscribe(e => component.fallSlider.value = e.SwayPercent)
                .AddTo(component);

            MessageBroker.Default.Receive<PlayerPukeUpdateEvent>()
                .Subscribe(e => component.pukeSlider.value = e.PukePercent)
                .AddTo(component);
        }

        public override void Register(ScoreComponent component)
        {
            ui.WhereNotNull()
                .Subscribe(_ =>
                {
                    component.crime
                        .Subscribe(v => ui.Value.crimeSlider.value = v / 100.0f)
                        .AddTo(component);
                })
                .AddTo(component);
        }
    }
}