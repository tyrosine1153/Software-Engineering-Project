using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraControl))]
public class CameraTest : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Camera Reset"))
        {
            CameraControl.Instance.CameraReset();
        }
        
        if (GUILayout.Button("Camera Zoom in"))
        {
            CameraControl.Instance.CameraZoomInOut(
                CameraControl.Instance.zoomStartActorIndex,
                CameraControl.Instance.zoomEndActorIndex
            );
        }
    }
}
