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

    private DishResultContainer[] dishResultContainerArr = new DishResultContainer[CookConst.RESULT_STATE.Length];

    private List<Dish> dishList = new List<Dish>();

    private WorkerUnit[] workerArr = new WorkerUnit[CookConst.WORKER_NUM];

    private SeatUnit[] seatArr = new SeatUnit[CookConst.WORKER_NUM];

    private DishClientCore core;

    private PlayerData playerData;

    private bool canControl;

    public void Init(DishClientCore _core, bool _canControl)
    {
        core = _core;

        canControl = _canControl;

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



            go = GameObjectFactory.Instance.GetGameObject("Assets/Resource/prefab/seat.prefab", null);

            go.transform.SetParent(seatContainer, false);

            (go.transform as RectTransform).anchoredPosition = new Vector2(i * seatGap, 0);

            SeatUnit seatUnit = go.GetComponent<SeatUnit>();

            seatUnit.Init(core, i, canControl);

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

                dishResultUnit.Init(dishResult);

                dishResultContainer.SetResult(dishResultUnit);
            }
        }

        for (int i = 0; i < CookConst.WORKER_NUM; i++)
        {
            Worker worker = playerData.workers[i];

            WorkerUnit workerUnit = workerArr[i];

            if (worker.pos == -1)
            {
                for (int m = 0; m < CookConst.WORKER_NUM; m++)
                {
                    SeatUnit seat = seatArr[m];

                    if (!seat.GetWorker())
                    {
                        seat.SetWorker(workerUnit);

                        break;
                    }
                }
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
}
