using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems.Ui
{
    public class CurtainComponent : MonoBehaviour
    {
        public void PlayAgain()
        {
            SceneManager.LoadScene("BarScene");
        }
    }
}
