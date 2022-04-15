using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField] private BGMType bgmClip;
    private void Start()
    {
        AudioManager.Instance.PlayBGM(bgmClip);
    }
}