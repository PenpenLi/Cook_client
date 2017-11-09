using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Cook_lib;
using System;
using gameObjectFactory;

public class DishClientCore : MonoBehaviour, IClient
{
    [SerializeField]
    private float dishResultGap;

    [SerializeField]
    private float seatGap;

    [SerializeField]
    private RectTransform requirementContainer;

    [SerializeField]
    private RectTransform mDishContainer;

    [SerializeField]
    private RectTransform oDishContainer;

    [SerializeField]
    private RectTransform mDishResultContainer;

    [SerializeField]
    private RectTransform oDishResultContainer;

    [SerializeField]
    private RectTransform mSeatContainer;

    [SerializeField]
    private RectTransform oSeatContainer;

    private Cook_client client;

    private DishResultContainer[] mDishResultContainerArr = new DishResultContainer[CookConst.RESULT_STATE.Length];

    private DishResultContainer[] oDishResultContainerArr = new DishResultContainer[CookConst.RESULT_STATE.Length];

    private List<Dish> mDishList = new List<Dish>();

    private List<Dish> oDishList = new List<Dish>();

    private List<RequirementContainer> requirementList = new List<RequirementContainer>();

    private WorkerUnit[] mWorkerArr = new WorkerUnit[CookConst.WORKER_NUM];

    private WorkerUnit[] oWorkerArr = new WorkerUnit[CookConst.WORKER_NUM];

    private SeatUnit[] mSeatArr = new SeatUnit[CookConst.WORKER_NUM];

    private SeatUnit[] oSeatArr = new SeatUnit[CookConst.WORKER_NUM];

    void Awake()
    {
        client = new Cook_client();

        client.Init(this);

        for (int i = 0; i < CookConst.RESULT_STATE.Length; i++)
        {
            GameObject go = GameObjectFactory.Instance.GetGameObject("Assets/Resource/prefab/resultContainer.prefab", null);

            go.transform.SetParent(mDishResultContainer, false);

            (go.transform as RectTransform).anchoredPosition = new Vector2(i * dishResultGap, 0);

            DishResultContainer drc = go.GetComponent<DishResultContainer>();

            drc.Init(this, i, true);

            mDishResultContainerArr[i] = drc;

            go = GameObjectFactory.Instance.GetGameObject("Assets/Resource/prefab/resultContainer.prefab", null);

            go.transform.SetParent(oDishResultContainer, false);

            (go.transform as RectTransform).anchoredPosition = new Vector2(i * dishResultGap, 0);

            drc = go.GetComponent<DishResultContainer>();

            drc.Init(this, i, false);

            oDishResultContainerArr[i] = drc;
        }

        for (int i = 0; i < CookConst.WORKER_NUM; i++)
        {
            GameObject go = GameObjectFactory.Instance.GetGameObject("Assets/Resource/prefab/workerUnit.prefab", null);

            WorkerUnit workerUnit = go.GetComponent<WorkerUnit>();

            mWorkerArr[i] = workerUnit;

            go = GameObjectFactory.Instance.GetGameObject("Assets/Resource/prefab/workerUnit.prefab", null);

            workerUnit = go.GetComponent<WorkerUnit>();

            oWorkerArr[i] = workerUnit;



            go = GameObjectFactory.Instance.GetGameObject("Assets/Resource/prefab/seat.prefab", null);

            go.transform.SetParent(mSeatContainer, false);

            (go.transform as RectTransform).anchoredPosition = new Vector2(i * seatGap, 0);

            SeatUnit seatUnit = go.GetComponent<SeatUnit>();

            seatUnit.Init(this, i, true);

            mSeatArr[i] = seatUnit;

            seatUnit.SetWorker(mWorkerArr[i]);

            go = GameObjectFactory.Instance.GetGameObject("Assets/Resource/prefab/seat.prefab", null);

            go.transform.SetParent(oSeatContainer, false);

            (go.transform as RectTransform).anchoredPosition = new Vector2(i * seatGap, 0);

            seatUnit = go.GetComponent<SeatUnit>();

            seatUnit.Init(this, i, false);

            oSeatArr[i] = seatUnit;

            seatUnit.SetWorker(oWorkerArr[i]);
        }
    }

    public void RefreshData()
    {
        Clear();


    }

    public void SendData(MemoryStream _ms)
    {

    }

    public void SendData(MemoryStream _ms, Action<BinaryReader> _callBack)
    {

    }

    public void TriggerEvent(ValueType _event)
    {

    }

    public void UpdateCallBack()
    {

    }

    private void Clear()
    {
        for (int i = 0; i < CookConst.RESULT_STATE.Length; i++)
        {
            DishResultContainer drc = mDishResultContainerArr[i];

            drc.Clear();

            drc = oDishResultContainerArr[i];

            drc.Clear();
        }

        for (int i = 0; i < mDishList.Count; i++)
        {
            Destroy(mDishList[i].gameObject);
        }

        mDishList.Clear();

        for (int i = 0; i < oDishList.Count; i++)
        {
            Destroy(oDishList[i].gameObject);
        }

        oDishList.Clear();

        for (int i = 0; i < requirementList.Count; i++)
        {
            Destroy(requirementList[i].gameObject);
        }

        requirementList.Clear();

        for (int i = 0; i < CookConst.WORKER_NUM; i++)
        {
            mSeatArr[i].SetWorker(mWorkerArr[i]);

            oSeatArr[i].SetWorker(oWorkerArr[i]);
        }
    }
}
