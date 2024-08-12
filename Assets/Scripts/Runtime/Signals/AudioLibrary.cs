using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Runtime
{
    public class AudioLibrary : MonoSingleton<AudioLibrary>
    {
        [Header("Music Tracks")]
        public AudioClip BattleMusic;
        public AudioClip MainMusic;

        [Header("Spawn effects")]
        public AudioClip PlaceBuildingSFX;
        public AudioClip SpawnKnightSFX;
        public AudioClip SpawnGoblinSFX;
        public AudioClip SpawnWizardSFX;
        public AudioClip SpawnEngineerSFX;
        public AudioClip SpawnOgreSFX;

        [Header("Attack effects")]
        public AudioClip GoblinStabSFX;
        public AudioClip SwordStabSFX;
        public AudioClip WizardAttackSFX;
        public AudioClip EngineerBuildSFX;

        [Header("Death Effects")]
        public AudioClip KnightDiesSFX;
        public AudioClip GoblinDiesSFX;
        public AudioClip DestroyBuilding;

        [Header("UI Effects")]
        public AudioClip UIClickSFX;
        public AudioClip UIPopSFX;
        public AudioClip[] SpeakGibberish;
        public AudioClip IncreaseOrbs;
        public AudioClip DecreaseOrbs;

        [Header("Skill Effects")]
        public AudioClip UnlockSkillSFX;
        public AudioClip LockSkillSFX;
        public AudioClip UnlockFinalKeySFX;
    }
}
