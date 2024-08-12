using Runtime.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Runtime
{
    public class UIOnMouseHovering : MonoBehaviour
    {
        public TextMeshProUGUI tipText;
        public RectTransform tipWindow;

        public static Action<string, Vector2> OnMouseHover;
        public static Action OnMouseLoseFocus;

        private void OnEnable()
        {
            OnMouseHover += ShowTip;
            OnMouseLoseFocus += HideTip;
        }
        private void OnDisable()
        {
            OnMouseHover -= ShowTip;
            OnMouseLoseFocus -= HideTip;
        }
        private void ShowTip(string tip, Vector2 mousePos)
        {
            tipText.text = tip;
            tipWindow.sizeDelta = new Vector2(tipText.preferredWidth > 400 ? 400 : tipText.preferredWidth, 
                tipText.preferredHeight < 50 ? 50 : tipText.preferredHeight);

            tipWindow.gameObject.SetActive(true);
            PositionTooltip(mousePos);
            //tipWindow.transform.position = new Vector2(mousePos.x, mousePos.y);
        }

        private void HideTip()
        {
            tipText.text = default;
            tipWindow.gameObject.SetActive(false);
        }

        private void PositionTooltip(Vector2 mousePosition)
        {
            Vector2 pivot = new Vector2(0.5f, 0.5f);

            // Calculate the pivot point based on screen position
            if (mousePosition.x + tipWindow.rect.width > Screen.width)
            {
                pivot.x = 1.0f;
            }
            else if (mousePosition.x - tipWindow.rect.width < 0)
            {
                pivot.x = 0.0f;
            }

            if (mousePosition.y + tipWindow.rect.height > Screen.height)
            {
                pivot.y = 1.0f;
            }
            else if (mousePosition.y - tipWindow.rect.height < 0)
            {
                pivot.y = 0.0f;
            }

            tipWindow.pivot = pivot;

            // Adjust the tooltip position
            Vector3 adjustedPosition = mousePosition;
            tipWindow.transform.position = adjustedPosition;
        }

        private void Start()
        {
            HideTip();
        }
    }
}
