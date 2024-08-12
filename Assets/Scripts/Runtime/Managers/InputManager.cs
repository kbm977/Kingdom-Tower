using Runtime.Signals;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Managers
{
    public class InputManager : MonoBehaviour
    {
        private  PlayerInputAction playerInputAction;

        public Vector2 mousePos;

        private void Awake()
        {
            playerInputAction = new();
            playerInputAction.Play.Enable();
            playerInputAction.Play.Shift.performed += ShiftClick;
            playerInputAction.Play.Shift.canceled += ShiftUp;
            playerInputAction.Play.MouseClick.performed += MouseClick;
        }

        private void Update()
        {
            mousePos = playerInputAction.Play.MousePos.ReadValue<Vector2>();
        }

        private void ShiftUp(InputAction.CallbackContext context)
        {
            if (!InputSignals.Instance) return;
            InputSignals.Instance.onShiftState?.Invoke(false);
        }

        private void ShiftClick(InputAction.CallbackContext context)
        {
            if (!InputSignals.Instance) return;
            InputSignals.Instance.onShiftState?.Invoke(true);
        }

        private void MouseClick(InputAction.CallbackContext context)
        {
            //if (EventSystem.current.IsPointerOverGameObject()) return;
            if (!InputSignals.Instance) return;
            InputSignals.Instance.onLMBDown?.Invoke();
        }
    }
}
