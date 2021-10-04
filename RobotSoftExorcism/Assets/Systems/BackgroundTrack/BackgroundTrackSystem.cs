using System;
using System.Numerics;
using SystemBase;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Systems.Camera;
using Object = UnityEngine.Object;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Systems.BackgroundTrack
{    
    [GameSystem]
    public class BackgroundTrackSystem : GameSystem<BackgroundTrackComponent, TrackedObjectComponent>
    {
        public Vector3[] _positions;
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
        
        public void RotateTrack(BackgroundTrackComponent component)
        {
            foreach (var trackedObject in component.trackObjects)
            {
                trackedObject.speed = component.speed * component.cameraVelocity;
                SetPositionAndRotation(trackedObject);
            }
        }

        public void SetPositionAndRotation(TrackedObjectComponent component)
        {
            component.transform.localPosition = component.point;
            Quaternion q = Quaternion.FromToRotation(Vector3.up, -component.orthogonalVector);
            component.transform.localRotation = q;
        }

        public void CalculatePosition(TrackedObjectComponent component)
        {
            if (_positions.Length >= 2)
            {
                component.currentPoint += component.speed;// * Time.deltaTime;
                CalculatePositionAndDirection(component, _positions);
            }
        }

        public void CalculatePositionAndDirection(TrackedObjectComponent component, Vector3[] positions)
        {
            Tuple<Vector3, Vector3> pointPair = GetPointPair(component, positions);
            component.orthogonalVector = GetOrth(pointPair);
            component.point = pointPair.Item1 + component.orthogonalVector * component.distance;
            // component.point = component.transform.localPosition;
        }

        public Vector3 GetOrth(Tuple<Vector3, Vector3> pointPair)
        {
            return Vector3.Cross(pointPair.Item2 - pointPair.Item1, Vector3.forward).normalized;
        }

        public Tuple<Vector3, Vector3> GetPointPair(TrackedObjectComponent component, Vector3[] positions)
        {
            Vector2Int pointPair = GetIndexPair(component, positions.Length);
            Vector3 pointA = positions[pointPair.x];
            Vector3 pointB = positions[pointPair.y];
            return new Tuple<Vector3, Vector3>(pointA, pointB);
        }
        

        private Vector2Int GetIndexPair(TrackedObjectComponent component, int positionsLength)
        {
            Vector2Int pair = new Vector2Int(-1, -1);
            if (positionsLength >= 2)
            {
                if (component.currentPoint < 0)
                {
                    component.currentPoint = positionsLength - 1;
                }
                pair.x = (int) component.currentPoint % (positionsLength - 1);
                pair.y = pair.x + 1;
            }

            return pair;
        }
        
        public Vector3[] CreateEllipse(float width, float height, float centerX, float centerY, float rotation, int pointCount)
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