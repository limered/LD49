using SystemBase;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Systems.Player;
using Systems.Movement;

namespace Systems.Camera
{
    [GameSystem]
    public class CameraFollowSystem : GameSystem<CameraFollowComponent>
    {
        private PlayerBrainComponent _player;

        private MovementComponent _movementComponent;

        public override void Register(CameraFollowComponent component)
        {
            _player = GameObject.FindObjectOfType<PlayerBrainComponent>();
            _movementComponent = _player.GetComponent<MovementComponent>();
            component.FixedUpdateAsObservable()
                .Subscribe(_ => Follow(component));
        }

        private void Follow(CameraFollowComponent component)
        {
            Vector3 currentPostion = component.transform.position;
            Vector3 distance = _movementComponent.transform.position - currentPostion;
            float velocity = Mathf.Abs(_movementComponent.Velocity.x) > 0.05f ? _movementComponent.Velocity.x : Mathf.Sign(_movementComponent.Velocity.x) * 0.5f;
            if (Mathf.Abs(distance.x) > 4)
            {
                Debug.Log(velocity);
                Vector3 newPostion = component.transform.position;
                newPostion.x += velocity * Time.deltaTime * 1.4f;
                component.transform.position = Vector3.Lerp(currentPostion, newPostion, component.lerpFraction);
            }
        }
    }
}
