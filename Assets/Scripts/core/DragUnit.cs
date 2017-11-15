using UnityEngine;
using publicTools;

public class DragUnit : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup cg;

    private DishClientCore core;

    private GameObject copy;

    public virtual void Init(DishClientCore _core)
    {
        core = _core;
    }

    public void StartDrag()
    {
        copy = Instantiate(gameObject);

        copy.transform.SetParent(core.transform, false);

        (copy.transform as RectTransform).anchoredPosition = PublicTools.MousePositionToCanvasPosition(core.canvas, Input.mousePosition);

        DragUnit du = copy.GetComponent<DragUnit>();

        du.cg.alpha = 0.5f;
    }

    void Update()
    {
        if (copy != null)
        {
            (copy.transform as RectTransform).anchoredPosition = PublicTools.MousePositionToCanvasPosition(core.canvas, Input.mousePosition);
        }
    }

    public void EndDrag()
    {
        Destroy(copy);

        copy = null;
    }
}
