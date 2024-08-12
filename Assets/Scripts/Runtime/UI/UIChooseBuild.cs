using Runtime.Managers;
using Runtime.Signals.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.UI
{
    public class UIChooseBuild : MonoBehaviour
    {
        public void ChooseBuild(BuildSO buildingData)
        {
            UISignals.Instance.onPlaySound?.Invoke(AudioLibrary.Instance.UIClickSFX);
            UISignals.Instance.onBuildingSelect?.Invoke(buildingData);
        }
    }
}
