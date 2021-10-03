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
        private Vector3 _newPosition;
        private Vector3 _startPosition;

        public override void Register(CameraFollowComponent component)
        {
            _player = Object.FindObjectOfType<PlayerBrainComponent>();
            _movementComponent = _player.GetComponent<MovementComponent>();
            _startPosition = component.transform.position;
            _newPosition = _startPosition;
                
            component.FixedUpdateAsObservable()
                .Subscribe(_ => SetFollowPosition(component))
                .AddTo(component);
            
            component.UpdateAsObservable()
                .Subscribe(_ => Follow(component))
                .AddTo(component);
        }

        private void Follow(CameraFollowComponent component)
        {
            component.transform.position = Vector3.Lerp(_startPosition, _newPosition, component.lerpFraction);
        }

        private void SetFollowPosition(CameraFollowComponent component)
        {
            Vector3 currentPosition = component.transform.position;
            Vector3 distance = _movementComponent.transform.position - currentPosition;
            
            if (Mathf.Abs(distance.x) > component.followThreshold)
            {
                _startPosition = currentPosition;
                _newPosition = currentPosition;
                _newPosition.x += (Mathf.Abs(distance.x) - Mathf.Abs(component.followThreshold)) *
                                  Mathf.Sign(distance.x);
            }
        }
    }
}
