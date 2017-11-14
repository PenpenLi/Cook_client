using UnityEngine;
using UnityEngine.EventSystems;

public class ControlUnit : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    protected DishClientCore core;

    [SerializeField]
    private GameObject selectedIcon;

    protected void Init(DishClientCore _core)
    {
        core = _core;

        SetSelected(false);
    }

    public void OnPointerClick(PointerEventData _data)
    {
        core.OnPointerClick(this);
    }

    public void OnPointerDown(PointerEventData _data)
    {
        core.OnPointerDown(this);
    }

    public void OnPointerEnter(PointerEventData _data)
    {
        core.OnPointerEnter(this);
    }

    public void OnPointerExit(PointerEventData _data)
    {
        core.OnPointerExit(this);
    }

    public void OnPointerUp(PointerEventData _data)
    {
        core.OnPointerUp(this);
    }

    public void SetSelected(bool _b)
    {
        if (selectedIcon != null)
        {
            selectedIcon.SetActive(_b);
        }
    }
}
