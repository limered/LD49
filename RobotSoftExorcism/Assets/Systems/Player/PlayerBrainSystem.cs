using System;
using System.Linq;
using SystemBase;
using Systems.Movement;
using Systems.Player.Events;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

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

            SystemFixedUpdate()
                .Sample(TimeSpan.FromMilliseconds(100))
                .Subscribe(_ => CalculateSway(component))
                .AddTo(component);
            
            SystemUpdate()
                .Sample(TimeSpan.FromMilliseconds(500))
                .Subscribe(_ => SetSwayDirection(component))
                .AddTo(component);
        }

        private static void ControlPlayer(PlayerBrainComponent player)
        {
            var movement = player.GetComponent<MovementComponent>();
            SetPlayerMovement(movement);
            StopPlayerIfItIsNotMoving(player, movement);
            StopPlayerOnBoundary(player, movement);
            RotatePlayerDependingOfMovement(player, movement);
            ApplySway(player, movement);
            CalculatePukeFactor(player, movement);
        }

        private static void CalculatePukeFactor(PlayerBrainComponent player, MovementComponent movement)
        {
            if (player.SwayPercent > 0.7f)
            {
                player.PukeFactor += player.pukeIncreaseValue * Time.fixedDeltaTime * player.SwayPercent;
                player.PukeFactor = player.PukeFactor > player.maxPukeFactor ? player.maxPukeFactor : player.PukeFactor;
            }
            else
            {
                player.PukeFactor -= player.pukeDecreaseValue * Time.fixedDeltaTime;
                player.PukeFactor = player.PukeFactor < 0 ? 0 : player.PukeFactor;
            }

            player.PukePercentage = player.PukeFactor / player.maxPukeFactor;
            MessageBroker.Default.Publish(new PlayerPukeUpdateEvent{PukePercent = player.PukePercentage});
            
            if (player.PukePercentage > 1)
            {
                // Start Puking
            }
        }

        private static void CalculateSway(PlayerBrainComponent player)
        {
            var movement = player.GetComponent<MovementComponent>();
            player.VelocityCache.Add(movement.Velocity);
            var swayFactor = player.VelocityCache.Buffer.Aggregate((vector2, vector3) =>
                new Vector2(vector2.x + vector3.x, vector2.y + vector3.y)).sqrMagnitude;
            player.SwayPercent = swayFactor / player.criticalSwayFactor;
            MessageBroker.Default.Publish(new PlayerSwayUpdateEvent{SwayPercent = player.SwayPercent});
        }
        
        private static void SetSwayDirection(PlayerBrainComponent player)
        {
            var vel = player.GetComponent<MovementComponent>().Velocity;
            player.SwayDirection = Random.value > 0.5f ? new Vector2(-vel.y, vel.x).normalized : 
                new Vector2(vel.y, -vel.x).normalized;
        }

        private static void ApplySway(PlayerBrainComponent player, MovementComponent movement)
        {
            var newRotation =
                Quaternion.AngleAxis(player.maxRotation * Math.Min(player.SwayPercent, 0.5f),
                    movement.Velocity.x < 0 ? Vector3.forward : Vector3.back);
            movement.transform.rotation *= newRotation;
            movement.Direction.Value += player.SwayDirection * player.SwayPercent;  
            if (player.SwayPercent > 1.0f)
            {
                MessageBroker.Default.Publish(new PlayerFallEvent());
            }
        }

        private static void RotatePlayerDependingOfMovement(PlayerBrainComponent player, MovementComponent movement)
        {
            var rotation = movement.Velocity.x / 2 * player.maxRotation;
            if (rotation > 0)
            {
                rotation = Math.Min(rotation, player.maxRotation);
            }
            else if (rotation < 0)
            {
                rotation = Math.Max(rotation, -player.maxRotation);
            }
            player.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.up);
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
