using SystemBase;
using UnityEngine;

namespace Systems.Camera
{
    public class CameraFollowComponent : GameComponent
    {
        public float followThreshold = 2.0f;
        public float lerpFraction = 0.6f;
        public float cameraVelocity = 0.0f;
    }
}
