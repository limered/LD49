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

        private float _t = 0.0f;

        public override void Register(CameraFollowComponent component)
        {
            _player = GameObject.FindObjectOfType<PlayerBrainComponent>();
            _movementComponent = _player.GetComponent<MovementComponent>();
            _startPosition = component.transform.position;
            _newPosition = _startPosition;
                
            component.FixedUpdateAsObservable()
                .Subscribe(_ => SetFollowPostion(component))
                .AddTo(component);
            
            component.UpdateAsObservable()
                .Subscribe(_ => Follow(component))
                .AddTo(component);
        }

        private void Follow(CameraFollowComponent component)
        {
            _t += Time.deltaTime;
            component.transform.position = Vector3.Lerp(_startPosition, _newPosition, _t);
        }

        private void SetFollowPostion(CameraFollowComponent component)
        {
            Vector3 currentPosition = component.transform.position;
            Vector3 distance = _movementComponent.transform.position - currentPosition;

            if (Mathf.Abs(distance.x) > 2.0)
            {
                _startPosition = currentPosition;
                _newPosition = currentPosition;
                _newPosition.x += distance.x;
                _t = 0.0f;
            }
        }
    }
}
