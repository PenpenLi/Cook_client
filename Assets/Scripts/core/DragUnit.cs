using UnityEngine;
using publicTools;

public class DragUnit : MonoBehaviour
{
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

        CanvasGroup cg = copy.AddComponent<CanvasGroup>();

        cg.alpha = 0.5f;

        cg.interactable = false;

        cg.blocksRaycasts = false;
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
