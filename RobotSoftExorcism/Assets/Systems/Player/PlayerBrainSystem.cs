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
                .Subscribe(CheckPlayerMovement)
                .AddTo(component);
        }

        private void CheckPlayerMovement(PlayerBrainComponent player)
        {
            var movement = player.GetComponent<MovementComponent>();
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