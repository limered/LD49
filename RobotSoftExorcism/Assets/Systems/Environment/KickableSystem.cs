using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Environment
{
    [GameSystem]
    public class KickableSystem : GameSystem<KickableComponent>
    {
        public override void Register(KickableComponent component)
        {
            component.HasBeenKickedTrigger
                .Subscribe(_ => Debug.Log("Poebeled"))
                .AddTo(component);
        }
    }
}