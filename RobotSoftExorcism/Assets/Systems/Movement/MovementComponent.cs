using System.Collections.Generic;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Movement
{
    public class MovementComponent : GameComponent
    {
        public float Speed;
        public float Friction;
        public float MaxSpeed;
        public Collider Collider;

        public Vector2ReactiveProperty Direction = new Vector2ReactiveProperty(Vector2.zero);
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }

        public Vector2 Force = Vector2.zero;

        public void Stop()
        {
            Direction.Value = Vector2.zero;
            Velocity = Vector2.zero;
            Acceleration = Vector2.zero;
        }

        public void AddForce(Vector2 force)
        {
            Force += force;
        }
    }
}