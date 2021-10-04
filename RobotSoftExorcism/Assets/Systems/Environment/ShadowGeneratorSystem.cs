using SystemBase;
using UnityEngine;

namespace Systems.Environment
{
    [GameSystem]
    public class ShadowGeneratorSystem : GameSystem<SimpleShadowComponent>
    {
        public override void Register(SimpleShadowComponent component)
        {
            
        }
    }

    public class SimpleShadowComponent : GameComponent
    {
        public GameObject model;
    }
}