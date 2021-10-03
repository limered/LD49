using System;
using SystemBase;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

namespace Systems.Environment
{
    public enum KickEffect
    {
        Yeet,
        Animate
    }
    [GameSystem]
    public class KickableSystem : GameSystem<KickableComponent>
    {
        public override void Register(KickableComponent component)
        {
             component.HasBeenKickedTrigger
                 .Subscribe(_ => ApplyKickEffect(component))
                 .AddTo(component);
        }

        private void ApplyKickEffect(KickableComponent component)
        {
            switch (component.kickEffect)
            {
                case KickEffect.Yeet:
                    var body = component.AddComponent<Rigidbody>();
                    body.AddForce(new Vector3(1000, 1000, 0));
                    break;
                case KickEffect.Animate:
                    break;
            }
        }
    }
}