using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "Entity/BuildEntity")]
public class EntityOS : ScriptableObject
{
    public string entityName;
    public int entityID, entityCost;
    public GameObject entityPrefab;
    public Sprite entityImage;
}