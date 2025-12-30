using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    public ItemData itemData;
    Transform originalParent;
    CanvasGroup canvasGroup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; // enables raycast
        canvasGroup.alpha = 1f;

        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>();
        if(dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;
            if (dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<Slot>();
            }
        }
        Slot originalSlot = originalParent.GetComponent<Slot>();

        if (dropSlot != null)
        {
            // There is a slot under drop point
            if (dropSlot.currentItem != null)
            {
                // Slot already has an item
                dropSlot.currentItem.transform.SetParent(originalParent.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                originalSlot.currentItem = null;
            }

            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            // No slot under drop point
            transform.SetParent(originalParent);
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // center the item
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject itemInformationParent = GameObject.FindGameObjectWithTag("ItemInformation");

        itemInformationParent.GetComponent<ItemInformationContent>().selectedItemSprite.gameObject.SetActive(true);
        itemInformationParent.GetComponent<ItemInformationContent>().selectedItemName.gameObject.SetActive(true);
        itemInformationParent.GetComponent<ItemInformationContent>().selectedItemDescription.gameObject.SetActive(true);

        itemInformationParent.GetComponent<ItemInformationContent>().selectedItemSprite.sprite = itemData.itemSprite;
        itemInformationParent.GetComponent<ItemInformationContent>().selectedItemName.text = itemData.itemName;
        itemInformationParent.GetComponent<ItemInformationContent>().selectedItemDescription.text = itemData.itemInformation;
    }
}