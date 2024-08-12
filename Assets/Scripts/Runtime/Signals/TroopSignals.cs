using Runtime.Controller;
using Runtime.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class TroopSignals : MonoSingleton<TroopSignals>
    {
        public UnityAction<TroopsSO, Vector3> onSummonTroop = delegate { };
        public UnityAction onSacrificeTroops = delegate { };

        public UnityAction<GameObject> onSwitchTarget = delegate { };

        public UnityAction<TroopController> onSelectTroop = delegate { };
        public UnityAction<BuildController> onSelectBuild = delegate { };
        public UnityAction onDeselectTroops = delegate { };
        public UnityAction onDeselectBuilds = delegate { };

        public UnityAction<TroopController, int, UISkilltree> onUnlockSkill = delegate { };
        public UnityAction onLockSkill = delegate { };
        public UnityAction onUpdateStats = delegate { };

        public UnityAction onUpgradeBuildings = delegate { };
        public UnityAction onTowerReward = delegate { };
    }
}
