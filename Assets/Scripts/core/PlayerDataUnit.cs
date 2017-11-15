using System.Collections.Generic;
using UnityEngine;
using Cook_lib;
using gameObjectFactory;

public class PlayerDataUnit : MonoBehaviour
{
    [SerializeField]
    private float dishResultGap;

    [SerializeField]
    private float seatGap;

    [SerializeField]
    private float dishGap;

    [SerializeField]
    private RectTransform dishContainer;

    [SerializeField]
    private RectTransform dishResultContainer;

    [SerializeField]
    private RectTransform seatContainer;

    [SerializeField]
    private TrashContainer trashContainer;

    [HideInInspector]
    public DishResultContainer[] dishResultContainerArr = new DishResultContainer[CookConst.RESULT_STATE.Length];

    [HideInInspector]
    public List<Dish> dishList = new List<Dish>();

    private WorkerUnit[] workerArr = new WorkerUnit[CookConst.WORKER_NUM];

    private SeatUnit[] seatArr = new SeatUnit[CookConst.WORKER_NUM];

    private DishClientCore core;

    private PlayerData playerData;

    private bool canControl;

    public void Init(DishClientCore _core, bool _canControl)
    {
        core = _core;

        canControl = _canControl;

        trashContainer.Init(core);

        for (int i = 0; i < CookConst.RESULT_STATE.Length; i++)
        {
            GameObject go = GameObjectFactory.Instance.GetGameObject("Assets/Resource/prefab/resultContainer.prefab", null);

            go.transform.SetParent(dishResultContainer, false);

            (go.transform as RectTransform).anchoredPosition = new Vector2(i * dishResultGap, 0);

            DishResultContainer drc = go.GetComponent<DishResultContainer>();

            drc.Init(core, i, canControl);

            dishResultContainerArr[i] = drc;
        }

        for (int i = 0; i < CookConst.WORKER_NUM; i++)
        {
            GameObject go = GameObjectFactory.Instance.GetGameObject("Assets/Resource/prefab/workerUnit.prefab", null);

            WorkerUnit workerUnit = go.GetComponent<WorkerUnit>();

            workerArr[i] = workerUnit;

            workerUnit.Init(core, i);

            go = GameObjectFactory.Instance.GetGameObject("Assets/Resource/prefab/seat.prefab", null);

            go.transform.SetParent(seatContainer, false);

            (go.transform as RectTransform).anchoredPosition = new Vector2(i * seatGap, 0);

            SeatUnit seatUnit = go.GetComponent<SeatUnit>();

            seatUnit.Init(core, -i - 1, canControl);

            seatArr[i] = seatUnit;

            seatUnit.SetWorker(workerArr[i]);
        }
    }

    public void RefreshData(PlayerData _playerData)
    {
        Clear();

        playerData = _playerData;

        List<DishData> dishDataList = playerData.dish;

        for (int i = 0; i < dishDataList.Count; i++)
        {
            DishData dishData = dishDataList[i];

            GameObject go = GameObjectFactory.Instance.GetGameObject("Assets/Resource/prefab/dish.prefab", null);

            go.transform.SetParent(dishContainer, false);

            (go.transform as RectTransform).anchoredPosition = new Vector2(0, -i * dishGap);

            Dish dish = go.GetComponent<Dish>();

            dish.Init(core, dishData, i, canControl);

            dishList.Add(dish);
        }

        DishResult[] dishResultArr = playerData.result;

        for (int i = 0; i < CookConst.RESULT_STATE.Length; i++)
        {
            DishResultContainer dishResultContainer = dishResultContainerArr[i];

            dishResultContainer.SetData(dishResultArr);

            DishResult dishResult = dishResultArr[i];

            if (dishResult != null)
            {
                GameObject go = GameObjectFactory.Instance.GetGameObject("Assets/Resource/prefab/dishResult.prefab", null);

                DishResultUnit dishResultUnit = go.GetComponent<DishResultUnit>();

                dishResultUnit.Init(core, dishResult);

                dishResultContainer.SetResult(dishResultUnit);
            }
        }

        for (int i = 0; i < CookConst.WORKER_NUM; i++)
        {
            Worker worker = playerData.workers[i];

            WorkerUnit workerUnit = workerArr[i];

            if (worker.pos < 0)
            {
                SeatUnit seat = seatArr[-worker.pos - 1];

                seat.SetWorker(workerUnit);
            }
            else
            {
                dishList[worker.pos].SetWorker(workerUnit);
            }
        }
    }

    public void Clear()
    {
        for (int i = 0; i < CookConst.RESULT_STATE.Length; i++)
        {
            DishResultContainer drc = dishResultContainerArr[i];

            drc.Clear();
        }

        for (int i = 0; i < dishList.Count; i++)
        {
            Destroy(dishList[i].gameObject);
        }

        dishList.Clear();

        for (int i = 0; i < CookConst.WORKER_NUM; i++)
        {
            seatArr[i].SetWorker(workerArr[i]);
        }
    }

    public void UpdateCallBack()
    {
        for (int i = 0; i < CookConst.RESULT_STATE.Length; i++)
        {
            dishResultContainerArr[i].Refresh();
        }

        for (int i = 0; i < dishList.Count; i++)
        {
            dishList[i].Refresh();
        }
    }

    public void ChangeWorkerPos(CommandChangeWorkerPos _command)
    {
        WorkerUnit workerUnit = workerArr[_command.workerIndex];

        SeatUnit seatUnit;

        if (_command.targetPos < 0)
        {
            seatUnit = seatArr[-_command.targetPos - 1];
        }
        else
        {
            seatUnit = dishList[_command.targetPos].dishWorkerBt;
        }

        seatUnit.SetWorker(workerUnit);
    }

    public void ChangeResultPos(CommandChangeResultPos _command)
    {
        DishResultContainer dishResultContainer = dishResultContainerArr[_command.pos];

        if (_command.targetPos == -1)
        {
            dishResultContainer.Clear();
        }
        else
        {
            DishResultContainer targetDishResultContainer = dishResultContainerArr[_command.targetPos];

            if (targetDishResultContainer.result == null)
            {
                targetDishResultContainer.SetResult(dishResultContainer.result);

                dishResultContainer.SetResult(null);
            }
            else
            {
                DishResultUnit tmpDishResult = targetDishResultContainer.result;

                targetDishResultContainer.SetResult(dishResultContainer.result);

                dishResultContainer.SetResult(tmpDishResult);
            }
        }
    }

    public void CompleteDish(CommandCompleteDish _command)
    {
        Dish dish = dishList[_command.pos];

        if (_command.targetPos != -1)
        {
            DishResultContainer dishResultContainer = dishResultContainerArr[_command.targetPos];

            dishResultContainer.SetResult(dish.resultUnit);

            dish.RemoveDishResult();
        }
        else
        {
            dish.DestroyDishResult();
        }
    }
}
