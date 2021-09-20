using UnityEngine;

namespace FireRidesClone.ScriptableObject
{
    [CreateAssetMenu(fileName ="LevelManager", menuName ="Manager/LevelManager")]
    public class LevelManager : UnityEngine.ScriptableObject
    {
        [HideInInspector]public int collisionCount;

        [HideInInspector]public bool nextLevelAccess, gameStarted;

        [HideInInspector] public float highScore;

        public Material[] wallColors;
        public int objCollectedForNextLevel;
    }
}
