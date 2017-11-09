using UnityEngine;
using UnityEngine.UI;

public class SeatUnit : MonoBehaviour, IWorkerContainer
{
    [SerializeField]
    private Image img;

    private DishClientCore core;

    private int index;

    private WorkerUnit workerUnit;

    public void Init(DishClientCore _core, int _index, bool _canControl)
    {
        core = _core;

        index = _index;

        img.raycastTarget = _canControl;
    }

    public void SetWorker(WorkerUnit _workerUnit)
    {
        workerUnit = _workerUnit;

        if (workerUnit != null)
        {
            workerUnit.transform.SetParent(transform, false);

            (workerUnit.transform as RectTransform).anchoredPosition = Vector2.zero;

            if (workerUnit.container != null)
            {
                workerUnit.container.SetWorker(null);
            }

            workerUnit.container = this;
        }
    }
}
