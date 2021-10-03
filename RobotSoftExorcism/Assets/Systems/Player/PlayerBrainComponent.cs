using SystemBase;
using SystemBase.StateMachineBase;
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

        public StateContext<PlayerBrainComponent> State;

        // Sway Stuff
        public float criticalSwayFactor;
        public float SwayPercent { get; set; }
        public RingBuffer<Vector2> VelocityCache { get; } = new RingBuffer<Vector2>(10);
        public Vector2 SwayDirection { get; set; }
        
        // Puke Stuff
        public float pukeIncreaseValue;
        public float pukeDecreaseValue;
        public float maxPukeFactor;
        public float PukeFactor { get; set; }
        public float PukePercentage { get; set; }
        public ParticleSystem pukeParticles;


        public float fallenDuration = 2000;
    }
}