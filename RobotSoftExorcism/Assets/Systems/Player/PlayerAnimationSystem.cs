using System;
using SystemBase;
using Systems.Animation;
using Systems.Player.Events;
using UniRx;

namespace Systems.Player
{
    internal enum AnimationIndex
    {
        Normal,
        HalfDrink,
        Drink,
        Fall,
        Poebel
    }
    
    [GameSystem]
    public class PlayerAnimationSystem : GameSystem<PlayerBrainComponent>
    {
        public override void Register(PlayerBrainComponent component)
        {
            MessageBroker.Default.Receive<PlayerStateChangeEvent>()
                .Select(ev => (component.GetComponents<BasicToggleAnimationComponent>(), ev.PlayerState))
                .Subscribe(AnimatePlayer)
                .AddTo(component);
        }

        private static void AnimatePlayer((BasicToggleAnimationComponent[] PlayerAnimators, PlayerState PlayerState) t)
        {
            var (playerAnimators, playerState) = t;
            switch (playerState)
            {
                case PlayerState.Normal:
                    Array.ForEach(
                        playerAnimators,
                        component =>
                            component.SetSpriteWithoutAnimation((int)AnimationIndex.Normal));
                    break;
                case PlayerState.Falling:
                    Array.ForEach(
                        playerAnimators,
                        component =>
                            component.SetSpriteWithoutAnimation((int)AnimationIndex.Fall));
                    break;
                case PlayerState.Puking:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}