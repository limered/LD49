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
        public float distance;

        public int GetClosestIndex(Vector3[] positions, Vector3 position)
        {
            float min = float.MaxValue;
            int index = -1; 
            for(int i = 0; i < positions.Length; i++)
            {
                Vector3 dist = position - positions[i];
                float buffer = dist.sqrMagnitude;
                if (buffer < min)
                {
                    min = buffer;
                    index = i;
                }
            }
            return index;
        }
        
        // private void OnDrawGizmos()
        // {
        //     BackgroundTrackComponent comp = GetComponentInParent<BackgroundTrackComponent>();
        //     Gizmos.DrawLine(comp.positions[(int) currentPoint], transform.localPosition);
        // }
    }
}