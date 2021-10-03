using SystemBase;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Systems.Camera;

namespace Systems.BackgroundTrack
{    
    [GameSystem]
    public class BackgroundTrackSystem : GameSystem<BackgroundTrackComponent, TrackedObjectComponent>
    {
        private Vector3[] _positions;
        private int _pointsPerUpdate;

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
                .Subscribe(_ => ChainToCamera(component))
                .AddTo(component);
        }
        
        public override void Register(TrackedObjectComponent component)
        {
            component.FixedUpdateAsObservable()
                .Subscribe(_ => CalculatePosition(component))
                .AddTo(component);
        }

        private void ChainToCamera(BackgroundTrackComponent component)
        {
            Vector3 position = component.transform.position; 
            position.x = _cameraFollowComponent.transform.position.x;
            component.transform.position = position;
        }
        
        private void RotateTrack(BackgroundTrackComponent component)
        {
            foreach (var trackedObject in component.trackObjects)
            {
                trackedObject.speed = component.speed;
                trackedObject.transform.position = trackedObject.point + trackedObject.orthogonalVector;

                Quaternion q = Quaternion.FromToRotation(Vector3.up, -trackedObject.orthogonalVector);

                trackedObject.transform.rotation = q;
            }
        }

        private void CalculatePosition(TrackedObjectComponent component)
        {
            if (_positions.Length >= 2)
            {
                component.currentPoint += component.speed * Time.deltaTime;
                int point = (int) component.currentPoint % (_positions.Length - 1);
                // component.currentPoint %= ;
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

        private Vector3 GetPoint(BackgroundTrackComponent component, int point)
        {
            return component.transform.position + _positions[point];
        }
    }
}