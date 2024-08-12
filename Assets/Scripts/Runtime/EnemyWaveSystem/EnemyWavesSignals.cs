using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class EnemyWavesSignals : MonoSingleton<EnemyWavesSignals>
    {
        public UnityAction onGoblinDied = delegate { };

    }
}
