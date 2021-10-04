using SystemBase;
using Systems.Player.Events;
using UniRx;

namespace Systems.Score
{
    [GameSystem]
    public class ScoreSystem : GameSystem<ScoreComponent>
    {
        private enum Score
        {
            Puke = 20,
            Kick = 10,
            Poebel = 5,
        }
        
        public override void Register(ScoreComponent component)
        {
            MessageBroker.Default.Receive<PlayerPukeEvent>()
                .Subscribe(_ => component.crime.Value += (int) Score.Puke)
                .AddTo(component);
            
            MessageBroker.Default.Receive<PlayerKickEvent>()
                .Subscribe(_ => component.crime.Value += (int) Score.Kick)
                .AddTo(component);
            
            MessageBroker.Default.Receive<PlayerPoebelEvent>()
                .Subscribe(_ => component.crime.Value += (int) Score.Poebel)
                .AddTo(component);

            MessageBroker.Default.Receive<PlayerDrinkEvent>()
                .Subscribe(_ => component.coffeeCount.Value++)
                .AddTo(component);

            component.crime.Subscribe(value => CheckIfPoliceIsComing(value)).AddTo(component);
        }

        private void CheckIfPoliceIsComing(int value)
        {
            if (value >= 100)
            {
                MessageBroker.Default.Publish(new PlayerStressesPoliceEvent());
            }
        }
    }
}