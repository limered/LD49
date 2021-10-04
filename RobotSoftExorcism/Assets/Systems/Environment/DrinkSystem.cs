using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Environment
{
    [GameSystem]
    public class DrinkSystem : GameSystem<DrinkComponent>
    {
        public override void Register(DrinkComponent component)
        {
            SystemUpdate(component)
                .Subscribe(Animate)
                .AddTo(component);
        }

        private void Animate(DrinkComponent drink)
        {
            var time = Time.realtimeSinceStartup;
            var x = Mathf.Sin(time) * 0.2f + 0.8f;
            var y = Mathf.Cos(time) * 0.2f + 0.8f;
            drink.transform.localScale = new Vector3(x, y, 0);
            drink.transform.rotation = Quaternion.AngleAxis(Mathf.Sin(time) * 360f, Vector3.up);
        }
    }
}