using SystemBase;
using UnityEngine.SceneManagement;

namespace Systems.Ui
{
    public class CurtainComponent : GameComponent
    {
        public void GoToNextScene()
        {
            var currentScene = SceneManager.GetActiveScene().name;
            switch (currentScene)
            {
                case "BarScene":
                    GoOutside();
                    break;
                case "CityScene":
                    ShowEnd();
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

        private void ShowEnd()
        {
            if (true)
            {
                GoToHappyEnd();
            }
            else
            {
                GoToSadEnd();
            }
        }

        private void GoToHappyEnd()
        {
            SceneManager.LoadScene("HappyEnd");
        }

        public void GoToSadEnd()
        {
            SceneManager.LoadScene("SadEnd");
        }
    }
}
