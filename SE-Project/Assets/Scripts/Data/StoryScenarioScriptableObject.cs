using UnityEngine;

[CreateAssetMenu(fileName = "Story Scenario Scriptable Object",
    menuName = "Scriptable Object/Story Scenario Scriptable Object", order = 1)]
public class StoryScenarioScriptableObject : ScriptableObject
{
    public StoryScenario[] scenarios;
}