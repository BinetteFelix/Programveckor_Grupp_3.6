using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "NPCData")]
public class NPCData : ScriptableObject
{
    [Space(15)]
    public string name;

    [Header("Interactable")]
    public bool isInteractable;
    public List<string> messages = new List<string>();

    [Header("Hostile Settings")]
    public bool isHostile;
    public float baseDamage;
    public float baseHealth;

}