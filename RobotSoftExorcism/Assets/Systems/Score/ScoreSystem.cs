using SystemBase;
using Systems.Player.Events;
using UniRx;
using UnityEngine;

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

            component.crime.Subscribe(value => CheckIfPoliceIsComing(component, value)).AddTo(component);
        }

        private void CheckIfPoliceIsComing(ScoreComponent component, int value)
        {
            if (value >= 100)
            {
                var police = GameObject.Instantiate(component.police);
                police.transform.position = new Vector3(4, -2.5f, 0); //TODO set correct position
                
                //TODO play animation with police
                MessageBroker.Default.Publish(new PlayerStressesPoliceEvent());
            }
        }
    }
}