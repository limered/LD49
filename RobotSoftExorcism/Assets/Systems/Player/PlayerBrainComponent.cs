using SystemBase;
using Systems.Movement;
using UnityEngine;
using Utils.Data;

namespace Systems.Player
{
    [RequireComponent(typeof(MovementComponent))]
    public class PlayerBrainComponent : GameComponent
    {
        public float goFriction;
        public float stopFriction;
        public Vector2 maxMovementPosition;
        public Vector2 minMovementPosition;
        public float maxRotation;

        public float criticalSwayFactor;
        public float SwayFactor { get; set; }
        public float SwayPercent { get; set; }
        public RingBuffer<Vector2> VelocityCache { get; } = new RingBuffer<Vector2>(10);
        public Vector2 SwayDirection { get; set; }
    }
}