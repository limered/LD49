using System;
using System.Linq;
using Assets.Utils.Math;
using StrongSystems.Audio;
using SystemBase.StateMachineBase;
using Systems.Environment;
using Systems.Player.Events;
using Systems.SoundManagement;
using UniRx;
using Object = UnityEngine.Object;

namespace Systems.Player.States
{
    [NextValidStates(typeof(PlayerStateNormal))]
    public class PlayerStatePoebling: BaseState<PlayerBrainComponent>
    {
        public override void Enter(StateContext<PlayerBrainComponent> context)
        {
            SoundSystem.RodPoebel.PlayRandom();
            
            var poebelElementsInVicinity = Object.FindObjectsOfType<PoebelComponent>()
                .Where(po =>
                    context.Owner.transform.position.DistanceTo(po.transform.position) < context.Owner.poebelRange)
                .ToList();

            foreach (var poebelComponent in poebelElementsInVicinity)
            {
                poebelComponent.WasPoebeledOnTrigger.Execute();
            }

            if (poebelElementsInVicinity.Any())
            {
                MessageBroker.Default.Publish(new PlayerPoebelEvent());
            }
            
            Observable.Timer(TimeSpan.FromMilliseconds(500))
                .Subscribe(_ => context.GoToState(new PlayerStateNormal()))
                .AddTo(this);
        }
    }
}