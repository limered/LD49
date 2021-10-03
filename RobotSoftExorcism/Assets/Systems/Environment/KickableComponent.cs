using SystemBase;
using UniRx;

namespace Systems.Environment
{
    public class KickableComponent : GameComponent
    {
        public KickEffect kickEffect;
        public readonly ReactiveCommand HasBeenKickedTrigger = new ReactiveCommand();
        public float rekickTime = 500;
    }
}