using System.Collections;
using SystemBase;
using Assets.Utils.Math;
using ExampleSystems.Example.States;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils;
using Utils.Plugins;

namespace Systems.Camera
{
    public class CameraFollowComponent : GameComponent
    {
        public float lerpFraction = 0.6f;
    }
}
