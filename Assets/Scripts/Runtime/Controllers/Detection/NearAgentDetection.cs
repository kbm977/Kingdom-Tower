using Runtime.Controller;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Runtime
{
    public class NearAgentDetection : MonoBehaviour
    {
        [SerializeField] private TroopController controller;
        [SerializeField] private string tagEnemy;

        //private GameObject _target;
        private bool _focused = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(tagEnemy) && !_focused)
            {
                controller.ChangeTarget(other.gameObject);
                //_target = other.gameObject;
                _focused = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _focused = false;
        }
    }
}
