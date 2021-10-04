using System;
using Assets.Utils.Math;
using SystemBase;
using Systems.Movement;
using Systems.Player;
using UniRx;
using UnityEngine;
using Utils.Plugins;

namespace Systems.Environment
{
    [GameSystem]
    public class PoebelSystem : GameSystem<PoebelComponent, PlayerBrainComponent>
    {
        private readonly ReactiveProperty<PlayerBrainComponent> _player = new ReactiveProperty<PlayerBrainComponent>();
        public override void Register(PoebelComponent component)
        {
            _player.WhereNotNull()
                .Subscribe(_ =>
                {
                    component.WasPoebeledOnTrigger
                        .Subscribe(_ => UpdatePoebler(component))
                        .AddTo(component);
                    
                })
                .AddTo(component);
        }

        private static void AnimatePoebler(PoebelComponent poebler)
        {
            var direction = poebler.transform.position.DirectionTo(poebler.Target);
            var movement = poebler.GetComponent<MovementComponent>();
            movement.Direction.Value += direction.XY();
        }
        
        private void UpdatePoebler(PoebelComponent poebler)
        {
            var target = _player.Value.transform.position.DirectionTo(poebler.transform.position) * 5;
            if (target.y > 0)
            {
                target.y = 0;
            }

            if (target.y < -5)
            {
                target.y = -5;
            }

            if (target.x < 0)
            {
                target.x = 0;
            }

            poebler.Target = target;
                
            SystemUpdate(poebler)
                .TakeUntil(Observable.Timer(TimeSpan.FromMilliseconds(2000)))
                .DoOnCompleted(() => poebler.Target = Vector2.zero)
                .Subscribe(AnimatePoebler)
                .AddTo(poebler);
        }

        public override void Register(PlayerBrainComponent component)
        {
            _player.Value = component;
        }
    }
}