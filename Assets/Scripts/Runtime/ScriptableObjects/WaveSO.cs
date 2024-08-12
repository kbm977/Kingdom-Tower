using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "Wave Data", menuName = "Wave/New wave")]
    public class WaveSO : ScriptableObject
    {
        public WaveData[] waveGoblins;

        public GameObject goblin, ogre;
        public int spawnPosCount;
    }
}
