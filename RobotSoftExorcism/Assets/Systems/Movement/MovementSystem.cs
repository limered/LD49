using GameState.States;
using SystemBase;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils;

namespace Systems.Movement
{
    [GameSystem]
    public class MovementSystem : GameSystem<MovementComponent>
    {
        public override void Register(MovementComponent component)
        {
            component.FixedUpdateAsObservable()
                .Select(_ => component)
                .Subscribe(CalculateMovement)
                .AddTo(component);

            ResetRigidbody(component);
        }

        private static void ResetRigidbody(Component comp)
        {
            var body = comp.GetComponent<Rigidbody>();
            body.drag = 0;
            body.angularDrag = 0;
            body.isKinematic = false;
            body.useGravity = false;
            body.freezeRotation = true;
        }

        private static void FixCollider(MovementComponent component)
        {
            component.Collider.transform.localPosition = Vector3.zero;
        }

        private static void ApplyAnimationToObject(MovementComponent component)
        {
            var positionChange = component.Velocity * Time.fixedDeltaTime;
            var transform = component.transform;
            var lastPosition = transform.position;
            transform.position = new Vector3(
                lastPosition.x + positionChange.x,
                lastPosition.y + positionChange.y,
                0.1f);
        }

        private static void Animate(MovementComponent component)
        {
            var futureVel = component.Velocity + component.Acceleration * Time.fixedDeltaTime;
            var speed = component.Velocity.magnitude;
            if (speed < component.MaxSpeed)
            {
                component.Velocity = futureVel;
            }
            else
            {
                component.Velocity = component.Velocity.normalized * component.MaxSpeed;
            }
        }

        private static void ApplyFriction(MovementComponent component)
        {
            var backFriction = component.Velocity * -component.Friction;
            component.Velocity += backFriction * Time.fixedDeltaTime;
        }

        private static void ApplyDirection(MovementComponent component)
        {
            component.Acceleration = component.Direction.Value * component.Speed;
        }

        private void CalculateMovement(MovementComponent component)
        {
            if (!(IoC.Game.GameStateContext.CurrentState.Value is Running)) return;
            
            StopRigidbodyMovement(component);
            ApplyDirection(component);
            ApplyFriction(component);
            Animate(component);
            ApplyAnimationToObject(component);
            if (component.Collider) FixCollider(component);
        }

        private static void StopRigidbodyMovement(MovementComponent component)
        {
            var body = component.GetComponent<Rigidbody>();
            body.angularVelocity = Vector3.zero;
            body.velocity = Vector3.zero;
        }
    }
}