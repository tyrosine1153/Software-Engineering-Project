using UnityEngine;

[CreateAssetMenu(fileName = "Game Data Scriptable Object", 
    menuName = "Scriptable Object/Game Data Scriptable Object", order = 0)]
public class GameDataScriptableObject : ScriptableObject
{
    public Potion[] potions;
    public Character[] characters;
}