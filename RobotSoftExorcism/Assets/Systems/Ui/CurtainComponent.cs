using GameState.Messages;
using SystemBase;
using Systems.SceneManagement.Events;
using Systems.Score;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Systems.Ui
{
    public class CurtainComponent : GameComponent
    {
        public void GoToNextScene()
        {
            var crimeValue = IoC.Game.GetComponent<ScoreComponent>().crime.Value;
            if (crimeValue >= 100)
            {
                GoToSadEnd();
                return;
            }

            var currentScene = SceneManager.GetActiveScene().name;
            switch (currentScene)
            {
                case "BarScene":
                    GoOutside();
                    break;
                case "CityScene":
                    GoToHappyEnd();
                    break;
                case "StartScene":
                case "HappyEnd":
                case "SadEnd":
                    GoToBar();
                    break;
                default:
                    ShowStart();
                    break;
            }
        }

        private void ShowStart()
        {
            ResetCrime();
            SceneManager.LoadScene("StartScene");
            MessageBroker.Default.Publish(new StartEvent());
        }
        
        private void GoToBar()
        {
            SceneManager.LoadScene("BarScene");
            MessageBroker.Default.Publish(new BarEnterEvent());
        }

        private void GoOutside()
        {
            SceneManager.LoadScene("CityScene");
            MessageBroker.Default.Publish(new CityEnterEvent());
        }

        private void GoToHappyEnd()
        {
            ResetCrime();
            SceneManager.LoadScene("HappyEnd");
            MessageBroker.Default.Publish(new HomeEnterEvent());
        }

        public void GoToSadEnd()
        {
            ResetCrime();
            SceneManager.LoadScene("SadEnd");
            MessageBroker.Default.Publish(new PrisonEnterEvent());
        }

        private void ResetCrime()
        {
            IoC.Game.GetComponent<ScoreComponent>().crime.Value = 0;
        }
    }
}
