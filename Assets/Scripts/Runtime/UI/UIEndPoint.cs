using Runtime.Managers;
using Runtime.Signals.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class UIEndPoint : MonoBehaviour
    {
        [SerializeField] private sbyte keyN;
        [SerializeField] private GameObject[] links;
        [SerializeField] private GameObject[] circles;
        //[SerializeField] private GameObject finalSkill;
        //[SerializeField] private GameObject bg;
        public void KeyActivate(bool active, UISkilltreeSO color)
        {
            if (active) keyN--;
            else keyN++;

            CheckAvailablity(color);
        }

        private void CheckAvailablity(UISkilltreeSO color)
        {
            if (keyN <= 0)
            {
                UISignals.Instance.onPlaySound?.Invoke(AudioLibrary.Instance.UnlockFinalKeySFX);
                GetComponent<Image>().color = color.endPoint;
                for (int i = 0; i < links.Length; i++)
                {
                    links[i].GetComponent<Image>().color = color.endPoint;
                    circles[i].GetComponent<Image>().color = color.endPoint;
                }
                /*if (finalSkill)
                {
                    finalSkill.GetComponent<UIEndPoint>().KeyActivate(true, color);
                }
                else
                {
                    bg.GetComponent<Image>().color = color.endPoint;
                }*/
            }
            else
            {
                GetComponent<Image>().color = color.lockedColor;
                for (int i = 0; i < links.Length; i++)
                {
                    links[i].GetComponent<Image>().color = color.lockedColor;
                    circles[i].GetComponent<Image>().color = color.lockedColor;
                }
            }
        }
    }
}
