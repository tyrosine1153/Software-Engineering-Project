using System.Collections;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private const float DefaultSize = 5f;
    private const float DefaultPadding = 0.2f;
    private const float MinSize = 2f;
    private const float ScreenRatio = 9f / 16f;
    
    [SerializeField] private CinemachineVirtualCamera mainVirtualCamera;
    [SerializeField] private Transform followObject;

    public int zoomStartActorIndex;
    public int zoomEndActorIndex;
    private void Start()
    {
        print("Frame Lock : 120 fps");
        Application.targetFrameRate = 120;
        ResetCamera();
    }

    public void ResetCamera()
    {
        followObject.position = Vector3.zero;
        mainVirtualCamera.m_Lens.OrthographicSize = DefaultSize;
    }
    
    public void ZoomCamera(int startIndex, int endIndex, float time = 1)
    {
        if (startIndex > endIndex) (startIndex, endIndex) = (endIndex, startIndex);
        
        var actors = GameTest.Instance.actors.Select(actor => actor.transform).ToArray();
        if (startIndex < 0 || endIndex > actors.Length) return;

        followObject.position = CameraPosition(actors[startIndex].position, actors[endIndex].position);

        var startPosition = actors[startIndex].position - actors[startIndex].lossyScale * 0.5f;
        var endPosition = actors[endIndex].position + actors[endIndex].lossyScale * 0.5f;

        StartCoroutine(CoSetCameraSize(CameraSize(startPosition, endPosition), time));
    }

    private IEnumerator CoSetCameraSize(float endSize, float time)
    {
        var currentSize = mainVirtualCamera.m_Lens.OrthographicSize;
        
        while (Mathf.Abs(currentSize - endSize) > 0.1f)
        {
            currentSize = Mathf.Lerp(currentSize, endSize, Time.deltaTime);
            mainVirtualCamera.m_Lens.OrthographicSize = currentSize;
            yield return null;
        }
        mainVirtualCamera.m_Lens.OrthographicSize = endSize;
    }
    
    private static Vector3 CameraPosition(Vector3 start, Vector3 end)
    {
        var mid = Vector3.Lerp(start, end, 0.5f); 
        mid.z = -10;
        
        return mid;
    }
    
    private static float CameraSize(Vector3 start, Vector3 end)
    {
        // camera size : 카메라 중심부터 위쪽 끝 점 까지의 거리
        var xSize = Mathf.Abs(start.x - end.x) * 0.5f * ScreenRatio;
        var ySize = Mathf.Abs(start.y - end.y) * 0.5f;
        
        return Mathf.Max(xSize, ySize, MinSize) + DefaultPadding;
    }
}
