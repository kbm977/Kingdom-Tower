using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Runtime.Managers;

namespace Runtime
{
    public class UIHoverTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public InputManager inputManager;
        public string tipToShow;
        private const float TIMETOWAIT = 0.5f;
        public void OnPointerEnter(PointerEventData eventData)
        {
            StopAllCoroutines();
            StartCoroutine(StartTimer());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StopAllCoroutines();
            UIOnMouseHovering.OnMouseLoseFocus();
        }

        private void ShowMessage()
        {
            UIOnMouseHovering.OnMouseHover(tipToShow, inputManager.mousePos);
        }

        private IEnumerator StartTimer()
        {
            yield return new WaitForSeconds(TIMETOWAIT);
            ShowMessage();
        }
    }
}
