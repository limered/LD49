using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Systems.BackgroundTrack
{
    public class TrackHelper : MonoBehaviour
    {

        public int RotateSlider = 0; 
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private int old = 0;
        private void OnValidate()
        {
            // Rotate(old - RotateSlider);
            // old = RotateSlider;
        }

        public void Do()
        {
            List<TrackedObjectComponent> trackedObjects = GetComponentsInChildren<TrackedObjectComponent>().ToList();
            BackgroundTrackComponent comp = GetComponent<BackgroundTrackComponent>();
            BackgroundTrackSystem system = new BackgroundTrackSystem();
            comp.trackObjects = new List<TrackedObjectComponent>();
            comp.trackObjects = trackedObjects;
            foreach (TrackedObjectComponent trackedObject in trackedObjects)
            {
                trackedObject.currentPoint 
                        = trackedObject.GetClosestIndex(comp.positions, trackedObject.transform.localPosition);
                trackedObject.distance = Vector3.Distance(trackedObject.transform.localPosition,
                    comp.positions[(int) trackedObject.currentPoint]); 
              
                system.CalculatePositionAndDirection(trackedObject, comp.positions);
                system.SetPositionAndRotation(trackedObject);
            }
            
        }

        public void Rotate(int speed = 5)
        {
            BackgroundTrackComponent comp = GetComponent<BackgroundTrackComponent>();
            BackgroundTrackSystem system = new BackgroundTrackSystem();
            system._positions = system.CreateEllipse(comp.width, comp.height, comp.centerX, comp.centerY, comp.rotation,
                comp.pointCount);
            foreach (TrackedObjectComponent trackedObject in comp.trackObjects)
            {
                comp.speed = speed;
                system.CalculatePosition(trackedObject);
            }

            
            comp.cameraVelocity = 1;
            
            system.RotateTrack(comp);
        }
    }
}