using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Environment
{
    public class KickableComponent : GameComponent
    {
        public KickEffect kickEffect;
        public readonly ReactiveCommand HasBeenKickedTrigger = new ReactiveCommand();
        public float kickStrength = 500;
        public Vector3 OldPosition { get; set; }
    }
}