using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[CreateAssetMenuAttribute(fileName = "InteractionObjectData", menuName = "InteractionObjectData")]
public class IOData : ScriptableObject
{
    [Header("Names & states")]
    public string ioName;
    public bool isNPC;
    public bool isItem;
    public bool isWarpObject;

    [Header("Layer & Tags")]
    public LayerMask interactionLayer;
    public string objectTag;

    [Header("NPC values")]
    public List<string> messages = new List<string>();

    [Header("Item values")]
    public Sprite itemSprite;
    public string itemInformation;
}