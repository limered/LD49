using System;
using System.Collections.Generic;
using SystemBase;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

namespace Systems.BackgroundTrack
{
    public class BackgroundTrackComponent : GameComponent
    {
        public List<TrackedObjectComponent> trackObjects;
        public float width = 10;
        public float height = 3;
        public float centerX = 0;
        public float centerY = 6;
        public float rotation = 0;
        public int pointCount = 30;
        public float speed = 10.0f;
        public Vector3 followPosition;
        public float cameraVelocity = 0.0f;
        
        public Vector3[] positions;

        private void OnDrawGizmos()
        {
            BackgroundTrackSystem system = new BackgroundTrackSystem();
            positions = system.CreateEllipse(width, height, centerX, centerY, rotation, pointCount);
            Vector3 currentPoint = positions[0];
            for (int i = 1; i < positions.Length; i++)
            {
                Gizmos.DrawLine(currentPoint, positions[i]);
                currentPoint = positions[i];
            }
        }
    }
}