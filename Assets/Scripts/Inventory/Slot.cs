using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, ISelectHandler
{
    public GameObject currentItem; //The item currently held in this inventory slot

    public void OnEnable()
    {
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(currentItem != null) currentItem.GetComponent<ItemDragHandler>().ShowInfo();

    }
    
}
