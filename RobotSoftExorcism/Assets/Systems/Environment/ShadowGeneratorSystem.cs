using SystemBase;
using UnityEngine;

namespace Systems.Environment
{
    [GameSystem]
    public class ShadowGeneratorSystem : GameSystem<SimpleShadowComponent>
    {
        private readonly Vector3 _shadowPosition = new Vector3(0, -0.3f, 0);
        private readonly Vector3 _shadowScale = new Vector3(1, 0.2f, 1);
        private readonly Color _shadowColor = new Color(0, 0, 0, 0.28f);
        
        public override void Register(SimpleShadowComponent component)
        {
            var shadow = Object.Instantiate(component.model, Vector3.zero, Quaternion.identity, component.transform);
            shadow.transform.localPosition = _shadowPosition;
            shadow.transform.localScale = _shadowScale;
            var renderer = shadow.GetComponent<SpriteRenderer>();
            renderer.color = _shadowColor;
            renderer.sortingOrder = 0;
        }
    }
}