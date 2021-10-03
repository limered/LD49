using SystemBase;
using UniRx;

namespace Systems.Score
{
    public class ScoreComponent : GameComponent
    {
        public IntReactiveProperty crime = new IntReactiveProperty(0);
    }
}