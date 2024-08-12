using DG.Tweening;
using Runtime;
using Runtime.Controller;
using Runtime.Managers;
using Runtime.Signals;
using Runtime.Signals.Game;
using Runtime.Signals.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mouse : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private GameObject mouse, cancelButton;
    [SerializeField]
    private LayerMask gridLayerMask;

    [SerializeField]
    private float cameraSpeed = 5f, padding = 50;

    [SerializeField]
    private Camera cam;

    private Vector3 mouseCurrentPos, mousePos;

    private bool _shiftPressed, _placeEntity, _targetChange;
    private Ray _ray;
    private RaycastHit _hit;
    private TroopsSO _troop;
    private BuildSO _build;

    private void Update()
    {
        MoveCamera();
        MouseRay();
    }

    private void OnLMBDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }

        if (mouse.activeSelf)
        {
            if (!GamaManager.AffordCost(-_build.entity.entityCost) || _hit.transform.gameObject.CompareTag("Player")) return;
            GridGenerator.PlaceItem(_build, new Vector3Int((int)mouseCurrentPos.x, (int)mouseCurrentPos.y, (int)mouseCurrentPos.z));
        }

        //Place entity
        else if (_placeEntity && !_hit.transform.gameObject.CompareTag("Player"))
        {
            TroopSignals.Instance.onSummonTroop?.Invoke(_troop, _hit.point);
        }

        //Select entity
        else if (Physics.Raycast(_ray, out RaycastHit _summonHit))
        {
            GameObject entity = _summonHit.transform.gameObject;
            if (_targetChange)
            {
                if (entity.CompareTag("Enemy") || entity.CompareTag("Build"))
                {
                    TroopSignals.Instance.onSwitchTarget?.Invoke(entity);
                }
            }

            if (!_shiftPressed)
            {
                TroopSignals.Instance.onDeselectTroops?.Invoke();
                TroopSignals.Instance.onDeselectBuilds?.Invoke();
            }

            if (entity.GetComponent<TroopController>())
            {
                TroopSignals.Instance.onDeselectBuilds?.Invoke();
                TroopSignals.Instance.onSelectTroop?.Invoke(entity.GetComponent<TroopController>());
            }
            else if (entity.GetComponent<BuildController>())
            {
                TroopSignals.Instance.onDeselectTroops?.Invoke();
                TroopSignals.Instance.onSelectBuild?.Invoke(entity.GetComponent<BuildController>());
            }
        }
    }

    private void OnSelectEntity(TroopsSO troop)
    {
        UISignals.Instance.onCancel = CancelSummon;
        cancelButton.SetActive(true);
        _placeEntity = true;
        _troop = troop;
    }

    private void CancelSummon()
    {
        cancelButton.SetActive(false);
        _placeEntity = false;
    }

    private void AdjustMouseScale(BuildSO building)
    {
        UISignals.Instance.onCancel = CancelBuild;
        GameSignals.Instance.onToggleGridTiles?.Invoke(true);
        cancelButton.SetActive(true);
        mouse.SetActive(true);
        _build = building;
        mouse.transform.localScale = new Vector3(_build.gridSize.x, mouse.transform.localScale.y, _build.gridSize.y);
    }

    private void CancelBuild()
    {
        mouse.SetActive(false);
        cancelButton.SetActive(false);
        GameSignals.Instance.onToggleGridTiles?.Invoke(false);
    }
    private void MouseRay()
    {
        mousePos = inputManager.mousePos;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        _ray = cam.ScreenPointToRay(mousePos);

        if (Physics.Raycast(_ray, out _hit, 999f, gridLayerMask))
        {
            mouseCurrentPos = _hit.point;
            mouseCurrentPos.y = 0;
        }

        if (!mouse.activeSelf) return;
        mouse.transform.position = new Vector3((int)mouseCurrentPos.x, 0, (int)mouseCurrentPos.z);
        if (_hit.transform == null) return;
        if (!GamaManager.AffordCost(-_build.entity.entityCost) || _hit.transform.gameObject.CompareTag("Player"))
        {
            mouse.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            return;
        }
        mouse.GetComponentInChildren<MeshRenderer>().material.color = 
            GridGenerator.CheckFreeSpace(_build.gridSize, Vector3Int.FloorToInt(mouse.transform.position)) ? Color.green : Color.red;
    }

    private void OnShiftState(bool state)
    {
        _shiftPressed = state;
    }

    //Move camera based on mouse position
    private void MoveCamera()
    {
        float verticalSpeed = 0f;
        float horizontalSpeed = 0f;
        float clampX = Mathf.Min(Screen.width, mousePos.x);
        float clampY = Mathf.Min(Screen.height, mousePos.y);

        verticalSpeed = CameraVerticalMovement(verticalSpeed, clampX);
        horizontalSpeed = CameraHorizontalMovement(horizontalSpeed, clampY);

        Vector3 movement = new Vector3(Mathf.Abs(verticalSpeed), 0, Mathf.Abs(horizontalSpeed)).normalized;
        cam.transform.position += new Vector3(verticalSpeed * movement.x * Time.deltaTime, 0, horizontalSpeed * movement.z * Time.deltaTime);
    }

    private float CameraVerticalMovement(float verticalSpeed, float clampX)
    {
        if (mousePos.x > Screen.width - padding && cam.transform.position.x < 40)
        {
            float min = Screen.width - padding;
            verticalSpeed = cameraSpeed * Mathf.Abs(clampX - min);
        }

        else if (mousePos.x < padding && cam.transform.position.x > 0)
        {
            verticalSpeed = -cameraSpeed * Mathf.Abs(clampX - padding);
        }

        verticalSpeed = Mathf.Clamp(verticalSpeed, -cameraSpeed * 100, cameraSpeed * 100);
        return verticalSpeed;
    }
    private float CameraHorizontalMovement(float horizontalSpeed, float clampY)
    {
        if (mousePos.y > Screen.height - padding && cam.transform.position.z < 40)
        {
            float min = Screen.height - padding;
            horizontalSpeed = cameraSpeed * Mathf.Abs(clampY - min);
        }

        else if (mousePos.y < padding && cam.transform.position.z > -5)
        {
            horizontalSpeed = -cameraSpeed * Mathf.Abs(clampY - padding);
        }

        horizontalSpeed = Mathf.Clamp(horizontalSpeed, -cameraSpeed * 100, cameraSpeed * 100);
        return horizontalSpeed;
    }

    private void OnCursorChanged(bool state, Texture2D texture)
    {
        _targetChange = state;
    }

    private void OnEnable()
    {
        InputSignals.Instance.onLMBDown += OnLMBDown;
        InputSignals.Instance.onShiftState += OnShiftState;
        InputSignals.Instance.onSelectEntity += OnSelectEntity;
        UISignals.Instance.onBuildingSelect += AdjustMouseScale;
        GameSignals.Instance.onCursorChange += OnCursorChanged;
    }
    private void OnDisable()
    {
        if (InputSignals.Instance != null)
        {
            InputSignals.Instance.onLMBDown -= OnLMBDown;
            InputSignals.Instance.onShiftState -= OnShiftState;
            InputSignals.Instance.onSelectEntity -= OnSelectEntity;
        }
        if (UISignals.Instance != null) UISignals.Instance.onBuildingSelect -= AdjustMouseScale;
        
        if (GameSignals.Instance != null) GameSignals.Instance.onCursorChange -= OnCursorChanged;
    }
}