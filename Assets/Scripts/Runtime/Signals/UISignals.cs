using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals.UI
{
    public class UISignals : MonoSingleton<UISignals>
    {
        public UnityAction onSwitchingToBuild = delegate { };
        public UnityAction onSwitchingToDefend = delegate { };

        public UnityAction<int> onUpdateOrb = delegate { };

        public UnityAction onAddTroop = delegate { };
        public UnityAction<bool> onRemoveTroop = delegate { };

        public UnityAction<int, int, int> onUpdateCounters = delegate { };
        public UnityAction onTroopControlToggle = delegate { };

        public UnityAction<BuildSO> onBuildingSelect = delegate { };
        public UnityAction onCancel = delegate { };

        public UnityAction<AudioClip> onPlaySound = delegate { };
    }
}
