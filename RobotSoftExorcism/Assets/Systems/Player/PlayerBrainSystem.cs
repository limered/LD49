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
            StopPlayerOnBoundary(player, movement);
        }

        private static void StopPlayerOnBoundary(PlayerBrainComponent player, MovementComponent movement)
        {
            if (!IsOutsideOfBounds(player, movement.transform.position)) return;
            
            movement.Velocity = new Vector2(movement.Velocity.x, 0);
            var oldPosition = movement.transform.position;
            movement.Direction.Value = new Vector2(movement.Direction.Value.x, 0);
                
            if (movement.transform.position.y > player.maxMovementPosition.y) 
            {
                movement.transform.position = new Vector3(oldPosition.x, player.maxMovementPosition.y, oldPosition.z);
                
            }else if (movement.transform.position.y < player.minMovementPosition.y)
            {
                movement.transform.position = new Vector3(oldPosition.x, player.minMovementPosition.y, oldPosition.z);
            }                                                                      
        }

        private static bool IsOutsideOfBounds(PlayerBrainComponent player, Vector3 position)
        {
            return position.y > player.maxMovementPosition.y ||
                   position.y < player.minMovementPosition.y;                                                  
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
