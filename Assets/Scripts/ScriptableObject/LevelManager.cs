using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName ="LevelManager", menuName ="Manager/LevelManager")]
public class LevelManager : ScriptableObject
{
    [HideInInspector]public int collisionCount;

    [HideInInspector]public bool nextLevelAccess, gameStarted;

    [HideInInspector] public float highScore;

    public Material[] wallColors;
    public int objCollectedForNextLevel;
}
