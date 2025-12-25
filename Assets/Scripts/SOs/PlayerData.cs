using UnityEngine;

[CreateAssetMenuAttribute(fileName = "PlayerData", menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Interaction")]
    public float interactionRadius;

    [Header("Layer & Tags")]
    public LayerMask _npcLayer;
    public LayerMask _itemLayer;
    public LayerMask _interactableSceneObjectsLayer;
}