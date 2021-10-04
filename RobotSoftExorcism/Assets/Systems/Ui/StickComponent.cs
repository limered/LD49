using System;
using SystemBase;
using Systems.Movement;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.Ui
{
    public class StickComponent : GameComponent
    {
        public StickOrientation orientation = StickOrientation.Auto;
        public Vector3 attachOffset = Vector3.zero;
    }

    [Serializable]
    public enum StickOrientation
    {
        Auto,
        FromAbove,
        FromBelow,
    }
}