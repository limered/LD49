using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Systems.BackgroundTrack
{
    public class TrackHelper : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

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

        public void Rotate()
        {
            BackgroundTrackComponent comp = GetComponent<BackgroundTrackComponent>();
            BackgroundTrackSystem system = new BackgroundTrackSystem();
            system._positions = system.CreateEllipse(comp.width, comp.height, comp.centerX, comp.centerY, comp.rotation,
                comp.pointCount);
            foreach (TrackedObjectComponent trackedObject in comp.trackObjects)
            {
                comp.speed = 5;
                system.CalculatePosition(trackedObject);
            }

            
            comp.cameraVelocity = 1;
            
            system.RotateTrack(comp);
        }
    }
}