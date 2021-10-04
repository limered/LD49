using System;
using SystemBase;
using SystemBase.StateMachineBase;
using Systems.Animation;
using Systems.Player.States;
using UniRx;

namespace Systems.Player
{
    internal enum AnimationIndex
    {
        Normal,
        HalfDrink,
        Drink,
        Fall,
        Poebel,
        Puke,
        Kick,
        Coffee
    }
    
    [GameSystem]
    public class PlayerAnimationSystem : GameSystem<PlayerBrainComponent>
    {
        public override void Register(PlayerBrainComponent component)
        {
            component.State.CurrentState
                .Select(state => (component.GetComponents<BasicToggleAnimationComponent>(), state))
                .Subscribe(AnimatePlayer)
                .AddTo(component);
        }

        private static void AnimatePlayer((BasicToggleAnimationComponent[] PlayerAnimators, BaseState<PlayerBrainComponent> PlayerState) t)
        {
            var (playerAnimators, playerState) = t;
            switch (playerState)
            {
                case PlayerStateNormal _:
                    Array.ForEach(
                        playerAnimators,
                        component =>
                            component.SetSpriteWithoutAnimation((int)AnimationIndex.Normal));
                    break;
                case PlayerStateFallen _:
                    Array.ForEach(
                        playerAnimators,
                        component =>
                            component.SetSpriteWithoutAnimation((int)AnimationIndex.Fall));
                    break;
                case PlayerStatePuking _:
                    Array.ForEach(
                        playerAnimators,
                        component =>
                            component.SetSpriteWithoutAnimation((int)AnimationIndex.Puke));
                    break;
                case PlayerStateKicking _:
                    Array.ForEach(
                        playerAnimators,
                        component =>
                            component.SetSpriteWithoutAnimation((int)AnimationIndex.Kick));
                    break;
                case PlayerStatePoebling _:
                    Array.ForEach(
                        playerAnimators,
                        component =>
                            component.SetSpriteWithoutAnimation((int)AnimationIndex.Poebel));
                    break;
                case PlayerStateDrinking _:
                    Array.ForEach(
                        playerAnimators,
                        component =>
                            component.SetSpriteWithoutAnimation((int)AnimationIndex.Coffee));
                    break;
            }
        }
    }
}