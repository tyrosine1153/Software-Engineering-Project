using UnityEngine;
using UnityEngine.UI;

public class CraftResult : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button submitButton;
    [Header("Potion Information")]
    [SerializeField] private Image potionImage;
    [SerializeField] private Text potionNameText;
    [SerializeField] private Text potionDescriptionText;
    private Potion _potion;

    private void Start()
    {
        retryButton.onClick.AddListener(() =>
        {
            
        });
        submitButton.onClick.AddListener(() =>
        {
            if(_potion == null) return;
            
            GameTest.Instance.GetPotionStory(_potion);
            _potion = null;
        });
    }

    public void ShowResult(bool success, Potion result = null)
    {
        _potion = result;
        if (success && result != null)
        {
            potionImage.sprite = SpriteUtil.LoadPotionSprite(result.id);
            potionNameText.text = result.name;
            potionDescriptionText.text = result.description;
            submitButton.gameObject.SetActive(true);
        }
        else
        {
            potionImage.sprite = SpriteUtil.LoadPotionSprite(0);
            potionNameText.text = "정체불명의 물약";
            potionDescriptionText.text = "뭔가 이상하다. 정말 이상하다.";
            submitButton.gameObject.SetActive(false);
        }
    }
}
