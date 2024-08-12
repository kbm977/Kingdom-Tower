using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class InputSignals : MonoSingleton<InputSignals>
    {
        public UnityAction<bool> onShiftState = delegate { };
        public UnityAction onLMBDown = delegate { };
        public UnityAction<TroopsSO> onSelectEntity = delegate { };
    }
}
