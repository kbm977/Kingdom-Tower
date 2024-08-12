using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "Builds", menuName = "New Entity/Building")]
    public class BuildSO : ScriptableObject
    {
        public EntityOS entity;
        public int health, buildTime, wealthReturn;
        public Vector2Int gridSize;
        public BuildType buildType;
    }

    public enum BuildType
    {
        Barrier,
        Tower,
        Castle
    }
}