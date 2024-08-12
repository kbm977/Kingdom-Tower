using Runtime;
using Runtime.Signals.Game;
using Runtime.Signals.UI;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public static Cell[,] grid;

    [SerializeField]
    private int width, height;

    private static List<GameObject> buildings = new();

    private void Awake()
    {
        grid = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                //Vector3 position = new Vector3(x, 0, z);
                grid[x, z] = new Cell(false, CellType.ground);
            }
        }
    }

    public static void PlaceItem(BuildSO build, Vector3Int mousePos)
    {
        if (CheckFreeSpace(build.gridSize, mousePos))
        {
            for (int x = 0; x < build.gridSize.x; x++)
            {
                for (int z = 0; z < build.gridSize.y; z++)
                {
                    grid[mousePos.x + x, mousePos.z + z].ChangeOccupation(true);
                }
            }
            GameSignals.Instance.onAddOrbs?.Invoke(-build.entity.entityCost);
            GameObject building = Instantiate(build.entity.entityPrefab, mousePos, Quaternion.identity);
            buildings.Add(building);
        }
    }

    public static bool CheckFreeSpace(Vector2Int size, Vector3Int mousePos)
    {
        if (mousePos.x < 0 || mousePos.z < 0) return false;
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                if (mousePos.x + x >= grid.GetLength(0) || mousePos.z + z >= grid.GetLength(1)) return false;
                if (grid[mousePos.x + x, mousePos.z + z].Occupied)
                {
                    //Debug.Log($"Occupied at cell: {mousePos.x + x}, {mousePos.z + z}"); 
                    return false;
                }
            }
        }

        return true;
    }

    private void ToggleGridTiles(bool state)
    {
        GetComponent<MeshRenderer>().enabled = state;
    }

    private void OnNextLevel()
    {
        foreach (GameObject building in buildings)
        {
            if (building == null) continue;
            if (building.GetComponent<BuildController>().alive) GameSignals.Instance.onAddOrbs?.Invoke(100);
            
            Destroy(building);
        }

        FreeGrid();
    }

    private void FreeGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                //Vector3 position = new Vector3(x, 0, z);
                grid[x, z].ChangeOccupation(false);
            }
        }
    }

    private void OnEnable()
    {
        GameSignals.Instance.onNextLevel += OnNextLevel;
        GameSignals.Instance.onToggleGridTiles += ToggleGridTiles;
    }

    private void OnDisable()
    {
        if (!GameSignals.Instance) return;
        GameSignals.Instance.onNextLevel -= OnNextLevel;
        GameSignals.Instance.onToggleGridTiles -= ToggleGridTiles;
    }
}