using SystemBase;
using UniRx;
using UnityEngine;
using Utils.Plugins;
using Random = UnityEngine.Random;

namespace Systems.Movement
{
    [GameSystem]
    public class WobbleSystem : GameSystem<RunWobbleComponent>
    {
        public override void Register(RunWobbleComponent component)
        {
            var movement = component.GetComponent<MovementComponent>();
            var startTime = Time.time - Random.value * 10000;

            SystemUpdate()
                .Select(_ => Time.time - startTime)
                .Select(time =>
                {
                    var a = Mathf.Sin(time / component.wobbleInterval) * movement.Velocity.magnitude *
                        component.wobbleFactor;

                    return Mathf.Abs(a) + a;
                })
                .Subscribe(sinus =>
                {
                    var target = component.wobbleTarget ? component.wobbleTarget : component.gameObject;
                    target.transform.localPosition = component.wobbleAxis * sinus;
                })
                .AddToLifecycleOf(component);
        }
    }
}