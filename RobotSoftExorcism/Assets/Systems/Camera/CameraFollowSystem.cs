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
        // Start is called before the first frame update

        private Vector3 _vel = Vector3.zero;

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
            if (Mathf.Abs(distance.x) > 2)
            {
                Vector3 newPostion = component.transform.position;
                newPostion.x += _movementComponent.Velocity.x * Time.deltaTime * 1.4f;
                component.transform.position = Vector3.Lerp(currentPostion, newPostion, component.lerpFraction);
            }
        }
    }
}
