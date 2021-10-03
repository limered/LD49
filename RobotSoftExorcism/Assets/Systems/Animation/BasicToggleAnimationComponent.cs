using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Animation
{
    public class BasicToggleAnimationComponent : GameComponent
    {
        public const int NotAnimating = -1;
        public bool startAnimationOnAwake;
        public float animationTime = 1f;
        public GameObject[] sprites;
        public GameObject endSprite;
        public bool reverse;
        public bool isLoop;

        #region Helpers
        public IntReactiveProperty OnSpriteIndexWithoutAnimation { get; } = new IntReactiveProperty(0);

        public ReactiveCommand OnShowEndSprite { get; } = new ReactiveCommand();

        public int currentSprite = NotAnimating;

        public void SetSpriteWithoutAnimation(int index)
        {
            OnSpriteIndexWithoutAnimation.Value = index;
        }

        public void ShowEndSprite()
        {
            OnShowEndSprite.Execute();
        }

        public void StartAnimation()
        {
            currentSprite = reverse ? sprites.Length - 1 : 0;
        }

        public void StopAnimation()
        {
            currentSprite = NotAnimating;
            OnShowEndSprite.Execute();
        }
        #endregion
    }

}