using SystemBase;
using Systems.Movement;
using UnityEngine;

namespace Systems.Player
{
    [RequireComponent(typeof(MovementComponent))]
    public class PlayerBrainComponent : GameComponent
    {
        public float goFriction;
        public float stopFriction;
    }
}