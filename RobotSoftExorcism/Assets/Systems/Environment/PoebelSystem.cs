using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Environment
{
    [GameSystem]
    public class PoebelSystem : GameSystem<PoebelComponent>
    {
        public override void Register(PoebelComponent component)
        {
            component.WasPoebeledOnTrigger
                .Subscribe(_ => Debug.Log("Poebeled"))
                .AddTo(component);
        }
    }
}