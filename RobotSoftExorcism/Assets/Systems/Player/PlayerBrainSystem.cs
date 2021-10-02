using SystemBase;
using Systems.Movement;
using UniRx;
using UnityEngine;

namespace Systems.Player
{
    [GameSystem]
    public class PlayerBrainSystem : GameSystem<PlayerBrainComponent>
    {
        public override void Register(PlayerBrainComponent component)
        {
            SystemFixedUpdate()
                .Select(_ => component)
                .Subscribe(ControlPlayer)
                .AddTo(component);
        }

        private static void ControlPlayer(PlayerBrainComponent player)
        {
            var movement = player.GetComponent<MovementComponent>();
            SetPlayerMovement(movement);
            StopPlayerIfItIsNotMoving(player, movement);
        }

        private static void StopPlayerIfItIsNotMoving(PlayerBrainComponent playerBrainComponent, MovementComponent movement)
        {
            movement.Friction = movement.Direction.Value == Vector2.zero 
                ? playerBrainComponent.stopFriction : playerBrainComponent.goFriction;
        }

        private static void SetPlayerMovement(MovementComponent movement)
        {
            movement.Direction.Value = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
            {
                movement.Direction.Value += Vector2.up;
            }

            if (Input.GetKey(KeyCode.S))
            {
                movement.Direction.Value += Vector2.down;
            }

            if (Input.GetKey(KeyCode.A))
            {
                movement.Direction.Value += Vector2.left;
            }

            if (Input.GetKey(KeyCode.D))
            {
                movement.Direction.Value += Vector2.right;
            }
        }
    }
}