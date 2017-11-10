using UnityEngine;
using UnityEngine.UI;

public class SeatUnit : ControlUnit, IWorkerContainer
{
    [SerializeField]
    private Image img;

    private int index;

    private WorkerUnit workerUnit;

    public void Init(DishClientCore _core, int _index, bool _canControl)
    {
        Init(_core);

        index = _index;

        img.raycastTarget = _canControl;
    }

    public void SetWorker(WorkerUnit _workerUnit)
    {
        if (workerUnit != _workerUnit)
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

    public bool GetWorker()
    {
        return workerUnit != null;
    }
}
