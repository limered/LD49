using System.Collections;
using System.Collections.Generic;
using SystemBase;
using Systems;
using Systems.Player;
using Systems.Player.Events;
using Systems.Score;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils;

[GameSystem]
public class ExitSystem : GameSystem<ExitComponent>
{
    public override void Register(ExitComponent component)
    {
        component.OnTriggerEnterAsObservable().Subscribe(collider => PlayerEntersExit(collider)).AddTo(component);
    }

    private void PlayerEntersExit(Collider collider)
    {
        if (collider.GetComponent<PlayerBrainComponent>())
        {
            var coffeeCount = IoC.Game.GetComponent<ScoreComponent>().coffeeCount.Value;
            MessageBroker.Default.Publish(new PlayerEnterExitEvent());
        }
    }
}
