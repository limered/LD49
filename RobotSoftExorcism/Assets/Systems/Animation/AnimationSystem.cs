using System;
using System.Collections;
using SystemBase;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Animation
{
    [GameSystem]
    public class AnimationSystem : GameSystem<BasicToggleAnimationComponent>
    {
        public override void Register(BasicToggleAnimationComponent component)
        {
            component.FixedUpdateAsObservable()
                .Select(_ => component.currentSprite != BasicToggleAnimationComponent.NotAnimating)
                .DistinctUntilChanged()
                .SelectMany(animating => animating ? Observable.FromCoroutine(() => Animate(component)) : Observable.Empty<Unit>())
                .Subscribe()
                .AddTo(component);

            component.OnSpriteIndexWithoutAnimation
                .Subscribe(index =>
                {
                    for (var s = 0; s < component.sprites.Length; s++)
                    {
                        component.sprites[s].SetActive(s == index);
                    }
                })
                .AddTo(component);

            component.OnShowEndSprite
                .Subscribe(_ =>
                {
                    if (!component.endSprite) return;
                    
                    foreach (var t in component.sprites)
                    {
                        t.SetActive(false);
                    }

                    component.endSprite.SetActive(true);
                })
                .AddTo(component);

            if (component.startAnimationOnAwake) component.StartAnimation();
        }

        private IEnumerator Animate(BasicToggleAnimationComponent component)
        {
            var steps = component.sprites.Length;
            var time = component.animationTime;
            var delta = time / steps;

            component.StartAnimation();
            if (component.endSprite) component.endSprite.SetActive(true);

            for (var i = 0; i < steps; i++)
            {
                if (component.currentSprite == BasicToggleAnimationComponent.NotAnimating) break;
                for (var s = 0; s < component.sprites.Length; s++)
                {
                    component.sprites[s].SetActive(component.currentSprite == s);
                }

                yield return new WaitForSeconds(delta);
                if (component.currentSprite == BasicToggleAnimationComponent.NotAnimating) break;

                component.currentSprite += component.reverse ? -1 : 1;
                component.currentSprite = Math.Max(0, component.currentSprite);
                component.currentSprite = Math.Min(component.currentSprite, steps - 1);

                if (i + 1 != steps || !component.isLoop) continue;
                
                i = -1;
                component.currentSprite = component.reverse ? steps - 1 : 0;
            }

            if (component.currentSprite == BasicToggleAnimationComponent.NotAnimating) yield break;
            
            if (component.endSprite)
            {
                foreach (var sprite in component.sprites)
                {
                    sprite.SetActive(false);
                }

                component.endSprite.SetActive(true);
            }

            component.StopAnimation();
        }
    }
}