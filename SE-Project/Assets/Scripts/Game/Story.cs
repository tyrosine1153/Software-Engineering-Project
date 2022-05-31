using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Story : MonoBehaviour
{
    [SerializeField] private Text speakerText;
    [SerializeField] private Text contentText;
    
    [SerializeField] private Button prevScenarioButton;
    [SerializeField] private Button nextScenarioButton;
    
    private void Start()
    {
        prevScenarioButton.onClick.AddListener(GameTest.Instance.SetPrevStory);
        nextScenarioButton.onClick.AddListener(GameTest.Instance.SetNextStory);
    }

    public void ShowStory(StoryScenario currentStoryScenario)
    {
        // Hide Buttons 
        prevScenarioButton.gameObject.SetActive(currentStoryScenario.PrevId != -1);
        nextScenarioButton.gameObject.SetActive(currentStoryScenario.NextId != -1);

        // Display Text
        speakerText.text = currentStoryScenario.Speaker;
        contentText.text = currentStoryScenario.Content;
        print($"{currentStoryScenario.PrevId} <- {currentStoryScenario.ID} -> {currentStoryScenario.NextId}");
        
        // Todo : Add Effects
        if (currentStoryScenario.Effects[(int)EffectType.None] != 1) { }
        else { }

        foreach (var scenarioCharacter in currentStoryScenario.Characters)
        {
            if(scenarioCharacter == null) continue;
            
            var characterId = scenarioCharacter.CharacterId;
            var character = DataManager.Instance.Characters.FirstOrDefault(c => c.ID == characterId);
            if (character.Emotion.All(emotion => emotion != scenarioCharacter.Emotion))
            {
                Debug.LogError("Emotion not found");
            }
        }
    }
}
