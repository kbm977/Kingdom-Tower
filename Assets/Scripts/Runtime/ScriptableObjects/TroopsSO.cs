using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "Troops", menuName = "New Entity/Troops")]
    public class TroopsSO : ScriptableObject
    {
        public EntityOS entity;
        public PlayerData data;
        public AgentType type;
    }

    [Serializable]
    public enum AgentType
    {
        Knight,
        Engineer,
        Magician
    }
}
