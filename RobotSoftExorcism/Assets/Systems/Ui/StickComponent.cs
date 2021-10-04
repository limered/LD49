using SystemBase;
using Systems.Movement;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.Ui
{
    [RequireComponent(typeof(MovementComponent))]
    public class StickComponent : GameComponent
    {
        public Vector3 attachOffset = Vector3.zero;
    }
}