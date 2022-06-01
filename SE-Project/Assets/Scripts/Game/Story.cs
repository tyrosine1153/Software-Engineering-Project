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
        prevScenarioButton.gameObject.SetActive(currentStoryScenario.prevId != -1);
        nextScenarioButton.gameObject.SetActive(currentStoryScenario.nextId != -1);

        // Display Text
        speakerText.text = currentStoryScenario.speaker;
        contentText.text = currentStoryScenario.content;
        print($"{currentStoryScenario.prevId} <- {currentStoryScenario.id} -> {currentStoryScenario.nextId}");
        
        // Todo : Add Effects
        if (currentStoryScenario.effects[(int)EffectType.None] != 1) { }
        else { }

        foreach (var scenarioCharacter in currentStoryScenario.characters)
        {
            if(scenarioCharacter == null) continue;
            
            var characterId = scenarioCharacter.characterId;
            var character = DataManager.Instance.characters.FirstOrDefault(c => c.id == characterId);
            if (character.emotion.All(emotion => emotion != scenarioCharacter.emotion))
            {
                Debug.LogError("Emotion not found");
            }
        }
    }
}
