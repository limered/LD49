using SystemBase;
using UniRx;

namespace Systems.Environment
{
    public class PoebelComponent : GameComponent
    {
        public ReactiveCommand WasPoebeledOnTrigger = new ReactiveCommand();
    }
}