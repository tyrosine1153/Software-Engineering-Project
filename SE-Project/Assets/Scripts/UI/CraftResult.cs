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
    [SerializeField] private Text unlockedPotionText;
    private Potion _potion;

    private void Start()
    {
        retryButton.onClick.AddListener(() =>
        {
            GameCanvas.Instance.RetryCraft();
        });
        
        submitButton.onClick.AddListener(() =>
        {
            if(_potion == null) return;
            
            GameCanvas.Instance.GetPotionStory(_potion);
            _potion = null;
        });
    }

    public void ShowResult(bool success, Potion result = null)
    {
        unlockedPotionText.text = "";
        _potion = result;
        if (success && result != null)
        {
            if (!RecipeModel.Instance.Recipes[result.id].IsRecipeUnlock)
            {
                RecipeModel.Instance.Recipes[result.id].IsRecipeUnlock = true;
                GameCanvas.Instance.RecipeBookUpdate();
                // 신규 해금 텍스트 추가
                unlockedPotionText.text = "[신규 포션 레시피 해금]";
            }

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
