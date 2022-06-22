using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraControl))]
public class CameraTest : Editor
{
    private CameraControl cameraControl;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        cameraControl ??= target as CameraControl;
        if (cameraControl == null) return;

        if (GUILayout.Button("Reset Camera"))
        {
            cameraControl.ResetCamera();
        }

        if (GUILayout.Button("Zoom Camera"))
        {
            cameraControl.ZoomCamera(
                cameraControl.zoomStartActorIndex,
                cameraControl.zoomEndActorIndex
            );
        }
    }
}
