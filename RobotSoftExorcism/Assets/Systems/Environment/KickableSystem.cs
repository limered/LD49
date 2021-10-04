using System;
using Assets.Utils.Math;
using SystemBase;
using Systems.Movement;
using Systems.Player;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Systems.Environment
{
    public enum KickEffect
    {
        Yeet,
        YeetPerson,
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
                    var body = component.GetComponent<Rigidbody>();
                    if (!body)
                    {
                        body = component.AddComponent<Rigidbody>();
                    }
                    body.AddForce(new Vector3(component.kickStrength, component.kickStrength, 0));
                    break;
                case KickEffect.YeetPerson:
                    var player = Object.FindObjectOfType<PlayerBrainComponent>();
                    var directionAway = player.transform.position.DirectionTo(component.transform.position);
                    var movement = component.GetComponent<MovementComponent>();
                    movement.AddForce(directionAway * component.kickStrength);
                    break;
            }
        }
    }
}