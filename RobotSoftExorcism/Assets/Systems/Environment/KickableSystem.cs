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
                    component.OldPosition = component.transform.position;
                    var strength = component.kickStrength;
                    var body = component.GetComponent<Rigidbody>();
                    if (!body)
                    {
                        body = component.AddComponent<Rigidbody>();
                    }

                    if (!body) return;
                    body.useGravity = false;
                    body.drag = 1;
                    body.AddForce(new Vector3(strength, strength, 0));
                    SystemUpdate(component)
                        .TakeUntil(Observable.Timer(TimeSpan.FromMilliseconds(5000)))
                        .DoOnCompleted(() => Object.Destroy(body))
                        .Subscribe(_ => ReturnToGround(body, component))
                        .AddTo(component);
                    break;
                case KickEffect.YeetPerson:
                    var player = Object.FindObjectOfType<PlayerBrainComponent>();
                    var directionAway = player.transform.position.DirectionTo(component.transform.position);
                    var movement = component.GetComponent<MovementComponent>();
                    movement.AddForce(directionAway * component.kickStrength);
                    break;
            }
        }

        private void ReturnToGround(Rigidbody body, KickableComponent kickable)
        {
            var position = body.transform.position;
            var dir = position.DirectionTo(kickable.OldPosition);
            var dist = position.DistanceTo(kickable.OldPosition);
            if (dist < 0.1) return;
            body.AddForce(new Vector3(0, dir.y, 0) * 10);
        }
    }
}