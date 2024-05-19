using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "New Level Data", menuName = "ScriptableObjects/GameConfig/GameData")]
    public class LevelData: ScriptableObject
    {
        public new string name;
        public int time;
        public int score;
    }
}