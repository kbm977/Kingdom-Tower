using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "UIColors", menuName = "UI/Colors")]
    public class UISkilltreeSO : ScriptableObject
    {
        public Color lockedColor;

        [Header("Skill")]
        public Color unlockedSkill, activeSkill, expensiveSkill;
        [Header("Connect Path")]
        public Color unlockedPath, activeePath, expensivePath;
        [Header("Connect Circles")]
        public Color unlockedConnectCircle, activeCircle, expensiveCircle;
        [Header("End Point")]
        public Color endPoint;
    }
}
