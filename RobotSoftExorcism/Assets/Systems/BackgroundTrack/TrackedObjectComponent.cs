using SystemBase;
using UnityEngine;

namespace Systems.BackgroundTrack
{
    public class TrackedObjectComponent : GameComponent
    {
        public Vector3 point;
        public Vector3 orthogonalVector;
        public float currentPoint;
        public float speed;
        
        private void OnDrawGizmos()
        {                
            Gizmos.DrawLine(point, point+ orthogonalVector.normalized);
        }
    }
}