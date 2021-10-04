using SystemBase;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.Ui
{
    public class StickConfigComponent : GameComponent
    {
        public GameObject stickPrefab;
        public float upAndDownThreshold = -3.5f;
    }
}