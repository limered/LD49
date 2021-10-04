using SystemBase;
using Systems.Score;
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
        }
        
        private void GoToBar()
        {
            SceneManager.LoadScene("BarScene");
        }

        private void GoOutside()
        {
            SceneManager.LoadScene("CityScene");
        }

        private void GoToHappyEnd()
        {
            ResetCrime();
            SceneManager.LoadScene("HappyEnd");
        }

        public void GoToSadEnd()
        {
            ResetCrime();
            SceneManager.LoadScene("SadEnd");
        }

        private void ResetCrime()
        {
            IoC.Game.GetComponent<ScoreComponent>().crime.Value = 0;
        }
    }
}
