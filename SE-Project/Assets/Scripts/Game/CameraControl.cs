using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraControl : Singleton<CameraControl>
{
    private const float DefaultCameraSize = 5;
    private const float DefaultCameraX = 17.77778f;
    private const float DefaultCameraY = 10f;
    private const float DefaultPadding = 0.2f;
    [SerializeField] private CinemachineVirtualCamera mainVirtualCamera;
    [SerializeField] private GameObject followObject;

    public int zoomStartActorIndex;
    public int zoomEndActorIndex;
    private void Start()
    {
        followObject.transform.position = Vector3.zero;
        // 해상도에 맞게 Default Value 설정
        // Todo : x = width * 17.77778 / 1920
        // Todo : y = height * 10 / 1080
    }

    public void CameraReset()
    {
        followObject.transform.position = Vector3.zero;
        mainVirtualCamera.m_Lens.OrthographicSize = DefaultCameraSize;
    }

    private Vector2 GetSpriteHalfWidth(Transform sprite)
    {
        var startSpriteScale = sprite.lossyScale;   
        return new Vector2(startSpriteScale.x / 2, startSpriteScale.y / 2);
    }
    
    public void CameraZoomInOut(int firstActorIndex, int secondActorIndex, float time = 1)
    {
        // 인덱스 정렬
        var first = Mathf.Min(firstActorIndex, secondActorIndex);
        var second = Mathf.Max(firstActorIndex, secondActorIndex);
        if (first < 0 || second > GameTest.Instance.actors.Length) return;
        
        // 스프라이트와 위치 가져오기
        var firstActor = GameTest.Instance.actors[first];
        var secondActor = GameTest.Instance.actors[second];
        var firstActorPosition = firstActor.transform.position;
        var secondActorPosition = secondActor.transform.position;
        
        // 스프라이트의 scale에 맞는 범위 구하기
        var firstSpriteHalfWidth = GetSpriteHalfWidth(firstActor.gameObject.transform);
        var secondSpriteHalfWidth = GetSpriteHalfWidth(secondActor.gameObject.transform);
        var startY =
            Mathf.Min(
                firstActorPosition.y - firstSpriteHalfWidth.y,
                secondActorPosition.y - secondSpriteHalfWidth.y
            );
        var endY =
            Mathf.Max(
                firstActorPosition.y + firstSpriteHalfWidth.y,
                secondActorPosition.y + secondSpriteHalfWidth.y
            );
        
        // 두 오브젝트의 시작위치와 끝위치
        var startVector = new Vector2(firstActorPosition.x - firstSpriteHalfWidth.x, startY);
        var endVector = new Vector2(secondActorPosition.x + secondSpriteHalfWidth.x, endY);
        CameraZoomInOut(startVector, endVector, time);
    }

    private void CameraZoomInOut(Vector2 startActorPosition, Vector2 endActorPosition, float time = 1) =>
        CameraZoomInOut(
            startActorPosition.x,
            startActorPosition.y,
            endActorPosition.x,
            endActorPosition.y,
            time
        );

    private void CameraZoomInOut(float x0, float y0, float x1, float y1, float time = 1)
    {
        // Default size = 5 (1920 : 1080 = 17.77778 : 10)
        var distanceX = Mathf.Abs(x0 - x1);
        var distanceY = Mathf.Abs(y0 - y1);

        // 중앙 위치 구하기
        var center = new Vector3((x0 + x1) / 2, (y0 + y1) / 2);
        followObject.transform.position = center;

        // 거리 계산 & 기준 축 구하기
        var distance = Mathf.Max(distanceX, distanceY);
        var isXPivot = distanceX > distanceY;

        // Todo: 해상도에 변화에 따른 점화식 수정 필요. (1920 * 1080 기준 점화식)
        // Todo: 자세한 사항은 Start 함수 참고 
        var cameraSize = mainVirtualCamera.m_Lens.OrthographicSize;
        var currentCameraRatio = cameraSize / DefaultCameraSize;
        var currentDistance = (isXPivot ? DefaultCameraX : DefaultCameraY) * currentCameraRatio;
        var changeSize = cameraSize * distance / currentDistance;

        // 연출 시간은 임의로 1초로 맞춰놓음
        mainVirtualCamera.m_Lens.OrthographicSize = changeSize + DefaultPadding;
        // StartCoroutine(ChangeCameraSize(cameraSize, changeSize, time));
    }

    // 근데 시간에 따라 천천히 값 바꾸는건 DoTween에 있던거 같은데....못찾겠네;;;
    private readonly WaitForSeconds waitFrameSecond = new WaitForSeconds(0.02f);
    
    private IEnumerator ChangeCameraSize(float startSize, float endSize, float time)
    {
        var n = time / 0.02f;

        var currentSize = startSize;
        endSize += DefaultPadding;
        while (currentSize < endSize)
        {
            var deltaTime = Time.deltaTime;
            time -= deltaTime;
            currentSize = Mathf.Lerp(startSize, endSize, time);
            mainVirtualCamera.m_Lens.OrthographicSize = currentSize;
            yield return waitFrameSecond;
        }
        mainVirtualCamera.m_Lens.OrthographicSize = endSize;
    }
}
