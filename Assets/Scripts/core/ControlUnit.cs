using UnityEngine;
using UnityEngine.EventSystems;

public class ControlUnit : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [HideInInspector]
    public DishClientCore core;

    [SerializeField]
    private GameObject selectedIcon;

    public void Init(DishClientCore _core)
    {
        core = _core;

        SetSelected(false);
    }

    public void OnPointerClick(PointerEventData _data)
    {
        if (core != null)
        {
            core.OnPointerClick(this);
        }
    }

    public void OnPointerDown(PointerEventData _data)
    {
        if (core != null)
        {
            core.OnPointerDown(this);
        }
    }

    public void OnPointerEnter(PointerEventData _data)
    {
        if (core != null)
        {
            core.OnPointerEnter(this);
        }
    }

    public void OnPointerExit(PointerEventData _data)
    {
        if (core != null)
        {
            core.OnPointerExit(this);
        }
    }

    public void OnPointerUp(PointerEventData _data)
    {
        if (core != null)
        {
            core.OnPointerUp(this);
        }
    }

    public void SetSelected(bool _b)
    {
        if (selectedIcon != null)
        {
            selectedIcon.SetActive(_b);
        }
    }
}
