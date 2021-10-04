using SystemBase;
using Systems.Player.Events;
using Systems.Score;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils;
using Utils.Plugins;

namespace Systems.Ui
{
    [GameSystem]
    public class StickSystem : GameSystem<StickConfigComponent, StickComponent>
    {
        public override void Register(StickConfigComponent component)
        {
            RegisterWaitable(component);
        }

        public override void Register(StickComponent component)
        {
            WaitOn<StickConfigComponent>()
                .Subscribe(config =>
                {
                    var stick = GameObject.Instantiate(config.stickPrefab);
                    var renderer = stick.GetComponentInChildren<SpriteRenderer>();
                    var fromAbove = component.transform.position.y > config.upAndDownThreshold;

                    component.UpdateAsObservable()
                        .Subscribe(_ =>
                        {
                            stick.transform.position =
                                component.transform.position + component.attachOffset + new Vector3(0,
                                    fromAbove
                                        ? renderer.bounds.extents.y
                                        : -renderer.bounds.extents.y);
                        })
                        .AddTo(component);
                })
                .AddTo(component);
        }
    }
}