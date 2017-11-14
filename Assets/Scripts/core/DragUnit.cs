using UnityEngine;
using publicTools;

public class DragUnit : MonoBehaviour
{
    private RectTransform lastParent;

    private Vector2 lastPos;

    private DishClientCore core;

    public virtual void Init(DishClientCore _core)
    {
        core = _core;
    }

    public void StartDrag()
    {
        lastParent = transform.parent as RectTransform;

        lastPos = (transform as RectTransform).anchoredPosition;

        transform.SetParent(core.transform, false);

        (transform as RectTransform).anchoredPosition = PublicTools.MousePositionToCanvasPosition(core.canvas, Input.mousePosition);
    }

    void Update()
    {
        if (lastParent != null)
        {
            (transform as RectTransform).anchoredPosition = PublicTools.MousePositionToCanvasPosition(core.canvas, Input.mousePosition);
        }
    }

    public void EndDrag()
    {
        transform.SetParent(lastParent, false);

        (transform as RectTransform).anchoredPosition = lastPos;

        lastParent = null;
    }
}
