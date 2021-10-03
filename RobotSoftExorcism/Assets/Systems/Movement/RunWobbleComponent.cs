using SystemBase;
using SystemBase.StateMachineBase;
using UnityEngine;

namespace Systems.Movement
{
    [RequireComponent(typeof(MovementComponent))]
    public class RunWobbleComponent : GameComponent
    {
        public float wobbleInterval = 0.02f;
        public float wobbleFactor = 0.02f;
        
        /// otherwise this.gameObject is used
        public GameObject wobbleTarget;

        public Vector3 wobbleAxis = Vector3.up;

    }
}