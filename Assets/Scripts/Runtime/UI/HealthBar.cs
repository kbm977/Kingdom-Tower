using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;

        private Camera cam;

        private void Awake()
        {
            cam = Camera.main;
            transform.position = target.position + offset;
        }

        public void UpdateHealthBar(float currentVal, float maxVal)
        {
            currentVal = Mathf.Max(0, currentVal);
            currentVal = ((int)currentVal);
            Debug.Log(currentVal);
            slider.value = currentVal / maxVal;
        }

        private void Update()
        {
            transform.rotation = cam.transform.rotation;
        }
    }
}
