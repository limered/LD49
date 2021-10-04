using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Environment
{
    public class PoebelComponent : GameComponent
    {
        public ReactiveCommand WasPoebeledOnTrigger = new ReactiveCommand();

        public Vector2 Target { get; set; }
    }
}