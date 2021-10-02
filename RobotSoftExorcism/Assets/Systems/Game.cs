using GameState.States;
using SystemBase;
using SystemBase.StateMachineBase;
using GameState.Messages;
using StrongSystems.Audio.Helper;
using UniRx;
using UnityEngine;
using Utils;

namespace Systems
{
    public class Game : GameBase
    {
        private readonly StateContext<Game> _gameStateContext = new StateContext<Game>();

        private void Awake()
        {
            IoC.RegisterSingleton(this);

            _gameStateContext.Start(new Loading());

            InstantiateSystems();

            Init();

            MessageBroker.Default.Publish(new GameMsgFinishedLoading());
            MessageBroker.Default.Publish(new GameMsgStart());
            UnityEngine.Cursor.visible = true;
        }

        private void Start()
        {
           // MessageBroker.Default.Publish(new GameMsgStart());
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 60;
        }

        public override void Init()
        {
            base.Init();

            IoC.RegisterSingleton<ISFXComparer>(()=> new SFXComparer());
        }
    }
}