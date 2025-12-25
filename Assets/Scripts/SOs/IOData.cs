using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenuAttribute(fileName = "InteractionObjectData", menuName = "InteractionObjectData")]
public class IOData : ScriptableObject
{
    [Header("Names & Values")]
    public string itemName;
    public Image itemSprite;

    public int interactMethod;

    [Header("Layer & Tags")]
    public LayerMask interactionLayer;
    public string objectTag;
}