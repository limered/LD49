using System.Numerics;
using SystemBase;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Systems.Camera;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Systems.BackgroundTrack
{    
    [GameSystem]
    public class BackgroundTrackSystem : GameSystem<BackgroundTrackComponent, TrackedObjectComponent>
    {
        private Vector3[] _positions;
        private CameraFollowComponent _cameraFollowComponent;

        public override void Register(BackgroundTrackComponent component)
        {
            _cameraFollowComponent = Object.FindObjectOfType<CameraFollowComponent>();
            _positions = CreateEllipse(component.width, component.height, component.centerX, component.centerY,
                component.rotation, component.pointCount);
            
            component.FixedUpdateAsObservable()
                .Subscribe(_ => RotateTrack(component))
                .AddTo(component);
            
            component.UpdateAsObservable()
                .Subscribe(_ => FollowCamera(component))
                .AddTo(component);
            
            component.UpdateAsObservable()
                .Subscribe(_ => RotateWithPlayer(component))
                .AddTo(component);
            
            component.FixedUpdateAsObservable()
                .Subscribe(_ => SetFollowPosition(component))
                .AddTo(component);
        }
        
        public override void Register(TrackedObjectComponent component)
        {
            component.FixedUpdateAsObservable()
                .Subscribe(_ => CalculatePosition(component))
                .AddTo(component);
        }

        private void RotateWithPlayer(BackgroundTrackComponent component)
        {
            component.cameraVelocity = _cameraFollowComponent.cameraVelocity;
        }
        
        private void SetFollowPosition(BackgroundTrackComponent component)
        {
            Vector3 currentPosition = component.transform.position;
            component.followPosition = currentPosition;
            component.followPosition.x = _cameraFollowComponent.transform.position.x;
        }

        private void FollowCamera(BackgroundTrackComponent component)
        {
            Vector3 currentPosition = component.transform.position;
            component.transform.position = Vector3.Lerp(currentPosition, component.followPosition, 0.6f);
        }
        
        private void RotateTrack(BackgroundTrackComponent component)
        {
            foreach (var trackedObject in component.trackObjects)
            {
                trackedObject.speed = component.speed * component.cameraVelocity;
                trackedObject.transform.localPosition = trackedObject.point + trackedObject.orthogonalVector;

                Quaternion q = Quaternion.FromToRotation(Vector3.up, -trackedObject.orthogonalVector);

                trackedObject.transform.localRotation = q;
            }
        }

        private void CalculatePosition(TrackedObjectComponent component)
        {
            if (_positions.Length >= 2)
            {
                component.currentPoint += component.speed;// * Time.deltaTime;
                Debug.Log(component.currentPoint);
                if (component.currentPoint < 0)
                {
                    component.currentPoint = _positions.Length - 1;
                }
                int point = (int) component.currentPoint % (_positions.Length - 1);
                Vector3 pointA = _positions[point];
                Vector3 pointB = _positions[point + 1];
                
                component.orthogonalVector = Vector3.Cross(pointB - pointA, Vector3.forward).normalized;
                component.point = pointA;
            }
        }
        
        private Vector3[] CreateEllipse(float width, float height, float centerX, float centerY, float rotation, int pointCount)
        {
            Vector3[] positions = new Vector3[pointCount+1];
            Quaternion q = Quaternion.AngleAxis (rotation, Vector3.forward);
            Vector3 center = new Vector3(centerX,centerY,0.0f);
 
            for (int i = 0; i <= pointCount; i++) {
                float angle = (float)i / (float)pointCount * 2.0f * Mathf.PI;
                positions[i] = new Vector3(width * Mathf.Cos (angle), height * Mathf.Sin (angle), 0.0f);
                positions[i] = q * positions[i] + center;
            }
            return positions;
        }
    }
}