using Runtime.Controller;
using Runtime.Managers;
using Runtime.Signals;
using Runtime.Signals.Game;
using Runtime.Signals.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class UISkilltree : MonoBehaviour
    {
        [SerializeField] private GameObject[] links;
        [SerializeField] private GameObject myLink;
        [SerializeField] private GameObject[] circles;
        [SerializeField] private GameObject myCircle;
        [SerializeField] private GameObject[] childrenSkills;
        [SerializeField] private UISkilltreeSO colorData;
        [SerializeField] private Image elementImage;
        [SerializeField] private GameObject endPoint;
        [SerializeField] private int cost;

        public TroopController troop;
        public int rank;
        public bool locked = true, active;

        private void Start()
        {
            if (locked) elementImage.color = colorData.lockedColor;
            else if (!active) elementImage.color = colorData.unlockedSkill;
            else elementImage.color = colorData.activeSkill;
        }

        public void ClickeUpon()
        {
            if (locked) return;
            //Check if he can buy

            Unlock();
        }

        private void Unlock()
        {
            if (active) { return; }
            if (!GamaManager.AffordCost(-cost)) return;
            GameSignals.Instance.onAddOrbs?.Invoke(-cost);
            TroopSignals.Instance.onUnlockSkill?.Invoke(troop, rank, this);
            UISignals.Instance.onPlaySound?.Invoke(AudioLibrary.Instance.UnlockSkillSFX);
            if (endPoint)
            {
                endPoint.GetComponent<UIEndPoint>().KeyActivate(true, colorData);
            }
            else
            { 
                for (int i = 0; i < links.Length; i++)
                {
                    links[i].GetComponent<Image>().color = colorData.unlockedPath;
                    circles[i].GetComponent<Image>().color = colorData.unlockedConnectCircle;

                    childrenSkills[i].GetComponent<UISkilltree>().locked = false;
                    childrenSkills[i].GetComponent<Image>().color = colorData.unlockedSkill;
                }
            }

            active = true;
            GetComponent<Image>().color = colorData.activeSkill;
            if (myCircle) myCircle.GetComponent<Image>().color = colorData.activeCircle;
            if (myLink) myLink.GetComponent<Image>().color = colorData.activeePath;
        }

        public void Lock()
        {
            active = false;
            UISignals.Instance.onPlaySound?.Invoke(AudioLibrary.Instance.LockSkillSFX);
            GameSignals.Instance.onAddOrbs?.Invoke((int)(cost / 2));
            GetComponent<Image>().color = colorData.unlockedSkill;
            if (myCircle) myCircle.GetComponent<Image>().color = colorData.unlockedPath;
            if (myLink) myLink.GetComponent<Image>().color = colorData.unlockedConnectCircle;
            for (int i = 0; i < links.Length; ++i)
            {
                links[i].GetComponent<Image>().color = colorData.lockedColor;
                circles[i].GetComponent<Image>().color = colorData.lockedColor; 
                
                if (i >= childrenSkills.Length) continue;
                if (childrenSkills[i].GetComponent<UISkilltree>() == null) continue;
                childrenSkills[i].GetComponent<UISkilltree>().locked = true;
                childrenSkills[i].GetComponent<Image>().color = colorData.lockedColor;
            }
        }
    }
}
