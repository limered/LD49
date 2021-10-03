using System;
using System.Linq;
using SystemBase;
using SystemBase.StateMachineBase;
using Systems.Movement;
using Systems.Player.Events;
using Systems.Player.States;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Systems.Player
{
    [GameSystem]
    public class PlayerBrainSystem : GameSystem<PlayerBrainComponent>
    {
        private const int CalculateSwayTime = 100;
        private const int SwayDirectionChangeTime = 500;
        private const int StayDownDuration = 2000;
        private const float PukeIncreaseThreshold = 0.7f;
        private const float FallThreshold = 1.5f;
        private const float FallPossibility = 0.8f;

        private IDisposable _fallDisposable;

        public override void Register(PlayerBrainComponent component)
        {
            component.State = new StateContext<PlayerBrainComponent>(component);
            component.State.Start(new PlayerStateNormal());
            
            SystemFixedUpdate()
                .Select(_ => component)
                .Subscribe(ControlPlayer)
                .AddTo(component);

            SystemFixedUpdate()
                .Sample(TimeSpan.FromMilliseconds(CalculateSwayTime))
                .Subscribe(_ => CalculateSway(component))
                .AddTo(component);
            
            SystemUpdate()
                .Sample(TimeSpan.FromMilliseconds(SwayDirectionChangeTime))
                .Subscribe(_ => SetSwayDirection(component))
                .AddTo(component);
            
            SystemUpdate(component)
                .Subscribe(CheckSpecialActions)
                .AddTo(component);
        }

        private void CheckSpecialActions(PlayerBrainComponent player)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                player.State.GoToState(new PlayerStatePoebling());
            }
        }

        private static void ControlPlayer(PlayerBrainComponent player)
        {
            var movement = player.GetComponent<MovementComponent>();
            if (player.State.CurrentState.Value is PlayerStateNormal)
            {
                SetPlayerMovement(movement);
                RotatePlayerDependingOfMovement(player, movement);
                ApplySway(player, movement);
            }
            StopPlayerIfItIsNotMoving(player, movement);
            StopPlayerOnBoundary(player, movement);
            CalculatePukeFactor(player);
            if (player.State.CurrentState.Value is PlayerStateFallen)
            {
                movement.transform.localRotation = Quaternion
                    .Slerp(movement.transform.localRotation,
                        Quaternion.AngleAxis(70, Vector3.right), 0.5f);
            }
        }

        private static void CalculatePukeFactor(PlayerBrainComponent player)
        {
            if (player.SwayPercent > PukeIncreaseThreshold)
            {
                player.PukeFactor += player.pukeIncreaseValue * Time.fixedDeltaTime * player.SwayPercent;
                player.PukeFactor = Math.Min(player.PukeFactor, player.maxPukeFactor);
            }
            else
            {
                player.PukeFactor -= player.pukeDecreaseValue * Time.fixedDeltaTime;
                player.PukeFactor = Math.Max(player.PukeFactor, 0);
            }

            player.PukePercentage = player.PukeFactor / player.maxPukeFactor;
            MessageBroker.Default.Publish(new PlayerPukeUpdateEvent{PukePercent = player.PukePercentage});

            if (!(player.PukePercentage > 1)) return;

            player.State.GoToState(new PlayerStatePuking());
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
                Quaternion.AngleAxis(player.maxRotation * Math.Min(player.SwayPercent, 0.3f),
                    movement.Velocity.x < 0 ? Vector3.forward : Vector3.back);
            
            movement.transform.rotation *= newRotation;
            movement.Direction.Value += player.SwayDirection * player.SwayPercent;
            if (player.SwayPercent <= FallThreshold || Random.value > FallPossibility) return;

            player.State.GoToState(new PlayerStateFallen());
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
