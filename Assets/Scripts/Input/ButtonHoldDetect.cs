using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoldDetect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    private bool _isHolding = false;

    public bool IsHolding { get => _isHolding; }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isHolding = true;
        //Debug.Log("Button is being held down.");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isHolding = false;
        //Debug.Log("Button is released.");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHolding = false;
        //Debug.Log("Button hold cancelled.");
    }
}
