using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotHighlighting : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{   
    [SerializeField] private Sprite _sprite0;
    [SerializeField] private Sprite _sprite1;
    [SerializeField] private Sprite _sprite2;

    Image slotImage;
    public void OnPointerClick(PointerEventData eventData)
    {
        slotImage = GetComponent<Image>();
        slotImage.sprite = _sprite2;  
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        slotImage = GetComponent<Image>();
        slotImage.sprite = _sprite1;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        slotImage = GetComponent<Image>();
        slotImage.sprite = _sprite0;
    }
}