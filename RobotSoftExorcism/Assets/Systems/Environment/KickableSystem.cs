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
        Animate,
        Explode
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
                    component.AddComponent<Rigidbody>().AddForce(new Vector3(1000, 1000, 0));
                    break;
                case KickEffect.Animate:
                    break;
                case KickEffect.Explode:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}