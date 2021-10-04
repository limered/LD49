#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Systems.BackgroundTrack
{
    [CustomEditor(typeof(TrackHelper))]
    public class TrackHelperInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            TrackHelper trackHelper = target as TrackHelper;
            if (GUILayout.Button("Do"))
            {
                Undo.RecordObject(trackHelper, "Do");
                trackHelper.Do();
                EditorUtility.SetDirty(trackHelper);
            }
            
            if (GUILayout.Button("Rotate"))
            {
                Undo.RecordObject(trackHelper, "Rotate");
                trackHelper.Rotate();
                EditorUtility.SetDirty(trackHelper);
            }
        }
    }
}
#endif