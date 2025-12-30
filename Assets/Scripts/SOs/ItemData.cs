using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Item values")]
    public string itemName;
    public Sprite itemSprite;
    public string itemInformation;
}
