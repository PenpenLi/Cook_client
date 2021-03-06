﻿using System.IO;
using UnityEngine;
using Cook_lib;
using System;
using System.Collections.Generic;
using gameObjectFactory;
using superFunction;
using UnityEngine.UI;

public class DishClientCore : MonoBehaviour, IClient
{
    public const string BATTLE_START = "battleStart";

    public const string BATTLE_OVER = "battleOver";

    public static float MAX_TIME { private set; get; }

    public static float TICK_SPAN { private set; get; }

    public static DishClientCore Instance { private set; get; }

    [SerializeField]
    public Canvas canvas;

    [SerializeField]
    private Background bg;

    [SerializeField]
    private Transform mPlayerDataContainer;

    [SerializeField]
    private Transform oPlayerDataContainer;

    [SerializeField]
    private Transform requirementContainerContainer;

    [SerializeField]
    private Text timeText;

    private Cook_client client;

    private Action<MemoryStream> sendData;

    private Action<MemoryStream, Action<BinaryReader>> sendDataWithReply;

    private RequirementContainer requirementContainer;

    private PlayerDataUnit mPlayerData;

    private PlayerDataUnit oPlayerData;

    public int tick
    {
        get
        {
            return client.GetTick();
        }
    }

    private bool isInit = false;

    void Awake()
    {
        Instance = this;

        Log.Init(Debug.Log);

        Dictionary<int, DishSDS> dic = StaticData.GetDic<DishSDS>();

        IEnumerator<DishSDS> enumerator = dic.Values.GetEnumerator();

        while (enumerator.MoveNext())
        {
            DishSDS sds = enumerator.Current;

            float time = sds.prepareTime + sds.cookTime + sds.optimizeTime;

            if (time > MAX_TIME)
            {
                MAX_TIME = time;
            }
        }

        TICK_SPAN = 1.0f / CookConst.TICK_NUM_PER_SECOND;

        Cook_client.Init(dic, StaticData.GetDic<ResultSDS>());

        client = new Cook_client();

        client.Init(this);

        GameObject go = GameObjectFactory.Instance.GetGameObject("Assets/Resource/prefab/playerDataUnit.prefab", null);

        go.transform.SetParent(mPlayerDataContainer, false);

        mPlayerData = go.GetComponent<PlayerDataUnit>();

        mPlayerData.Init(this, true);

        go = GameObjectFactory.Instance.GetGameObject("Assets/Resource/prefab/playerDataUnit.prefab", null);

        go.transform.SetParent(oPlayerDataContainer, false);

        oPlayerData = go.GetComponent<PlayerDataUnit>();

        oPlayerData.Init(this, false);

        go = GameObjectFactory.Instance.GetGameObject("Assets/Resource/prefab/requirementContainer.prefab", null);

        go.transform.SetParent(requirementContainerContainer, false);

        requirementContainer = go.GetComponent<RequirementContainer>();

        requirementContainer.Init(this, client.GetRequirement());

        bg.Init(this);

        gameObject.SetActive(false);
    }

    public void Init(Action<MemoryStream> _sendData, Action<MemoryStream, Action<BinaryReader>> _sendDataWithReply)
    {
        sendData = _sendData;

        sendDataWithReply = _sendDataWithReply;
    }

    public void RefreshData()
    {
        if (!isInit)
        {
            isInit = true;

            SuperFunction.Instance.DispatchEvent(gameObject, BATTLE_START);
        }

        Clear();

        PlayerData playerData = client.GetPlayerData(client.clientIsMine);

        mPlayerData.RefreshData(playerData);

        playerData = client.GetPlayerData(!client.clientIsMine);

        oPlayerData.RefreshData(playerData);

        requirementContainer.RefreshData();

        RefreshTime();
    }

    public void SendData(MemoryStream _ms)
    {
        sendData(_ms);
    }

    public void SendData(MemoryStream _ms, Action<BinaryReader> _callBack)
    {
        sendDataWithReply(_ms, _callBack);
    }

    public void TriggerEvent(ValueType _event)
    {
        if (_event is CommandChangeResultPos)
        {
            CommandChangeResultPos command = (CommandChangeResultPos)_event;

            GetCommandChangeResultPos(command);
        }
        else if (_event is CommandChangeWorkerPos)
        {
            CommandChangeWorkerPos command = (CommandChangeWorkerPos)_event;

            GetCommandChangeWorkerPos(command);
        }
        else if (_event is CommandCompleteDish)
        {
            CommandCompleteDish command = (CommandCompleteDish)_event;

            GetCommandCompleteDish(command);
        }
        else if (_event is CommandCompleteRequirement)
        {
            CommandCompleteRequirement command = (CommandCompleteRequirement)_event;

            GetCommandCompleteRequirement(command);
        }
    }

    private void GetCommandChangeResultPos(CommandChangeResultPos _command)
    {
        PlayerDataUnit unit = _command.isMine == client.clientIsMine ? mPlayerData : oPlayerData;

        unit.ChangeResultPos(_command);
    }

    private void GetCommandChangeWorkerPos(CommandChangeWorkerPos _command)
    {
        PlayerDataUnit unit = _command.isMine == client.clientIsMine ? mPlayerData : oPlayerData;

        unit.ChangeWorkerPos(_command);
    }

    private void GetCommandCompleteDish(CommandCompleteDish _command)
    {
        PlayerDataUnit unit = _command.isMine == client.clientIsMine ? mPlayerData : oPlayerData;

        unit.CompleteDish(_command);
    }

    private void GetCommandCompleteRequirement(CommandCompleteRequirement _command)
    {
        PlayerDataUnit unit = _command.isMine == client.clientIsMine ? mPlayerData : oPlayerData;

        unit.CompleteRequirement(_command);

        for (int i = requirementContainer.containerList.Count - 1; i > -1; i--)
        {
            RequirementUnitContainer container = requirementContainer.containerList[i];

            if (container.requirement.uid == _command.requirementUid)
            {
                requirementContainer.containerList.RemoveAt(i);

                Destroy(container.gameObject);

                break;
            }
        }
    }

    public void UpdateCallBack(GameResult _gameResult)
    {
        mPlayerData.UpdateCallBack();

        oPlayerData.UpdateCallBack();

        requirementContainer.UpdateCallBack();

        if (_gameResult != GameResult.NOT_OVER)
        {
            Debug.Log("game over:" + _gameResult);
        }
        else
        {
            RefreshTime();
        }
    }

    private void RefreshTime()
    {
        timeText.text = ((int)(CookConst.MAX_TIME - client.GetTick() / CookConst.TICK_NUM_PER_SECOND)).ToString();
    }

    private void Clear()
    {
        mPlayerData.Clear();

        oPlayerData.Clear();

        requirementContainer.Clear();
    }

    public void GetPackage(BinaryReader _br)
    {
        if (isInit)
        {
            client.ClientGetPackage(_br);
        }
    }

    public void RequestRefreshData()
    {
        client.RefreshData();
    }






















    private Vector2 downPos;

    private ControlUnit downUnit;

    private ControlUnit enterUnit;

    private bool hasExit;

    private bool hasDragCheck;

    private DragUnit dragUnit;

    private List<ControlUnit> selectedUnitList = new List<ControlUnit>();

    public void OnPointerClick(ControlUnit _unit)
    {
        //Debug.Log("OnPointerClick:" + _unit);
    }

    public void OnPointerDown(ControlUnit _unit)
    {
        //Debug.Log("OnPointerDown:" + _unit);

        if (_unit is Background)
        {
            ClickControlUnit(_unit);

            downUnit = enterUnit = null;
        }
        else
        {
            downPos = Input.mousePosition;

            enterUnit = downUnit = _unit;
        }

        hasExit = false;

        hasDragCheck = false;

        dragUnit = null;
    }

    public void OnPointerEnter(ControlUnit _unit)
    {
        //Debug.Log("OnPointerEnter:" + _unit);

        enterUnit = _unit;
    }

    public void OnPointerExit(ControlUnit _unit)
    {
        //Debug.Log("OnPointerExit:" + _unit);

        enterUnit = null;

        if (!hasExit)
        {
            hasExit = true;
        }
    }

    public void OnPointerUp(ControlUnit _unit)
    {
        //Debug.Log("OnPointerUp:" + _unit);
    }

    private void ClickControlUnit(ControlUnit _unit)
    {
        //Debug.Log("ClickControlUnit:" + _unit);

        int index = selectedUnitList.IndexOf(_unit);

        if (index != -1)
        {
            selectedUnitList[index].SetSelected(false);

            selectedUnitList.RemoveAt(index);

            return;
        }

        if (_unit is Background)
        {
            ClearSelectedUnitList();
        }
        else if (_unit is SeatUnit)
        {
            ClickSeatUnit(_unit as SeatUnit);
        }
        else if (_unit is DishResultBt)
        {
            ClickDishResultBt(_unit as DishResultBt);
        }
        else if (_unit is DishResultContainer)
        {
            ClickDishResultContainer(_unit as DishResultContainer);
        }
        else if (_unit is RequirementUnitContainer)
        {
            ClickRequirementUnitContainer(_unit as RequirementUnitContainer);
        }
        else if (_unit is TrashContainer)
        {
            ClickTrashContainer();
        }
    }

    private void ClearSelectedUnitList()
    {
        for (int i = 0; i < selectedUnitList.Count; i++)
        {
            selectedUnitList[i].SetSelected(false);
        }

        selectedUnitList.Clear();
    }

    private void StartDrag()
    {
        hasDragCheck = true;

        if (downUnit is SeatUnit)
        {
            WorkerUnit worker = (downUnit as SeatUnit).GetWorker();

            if (worker != null)
            {
                dragUnit = worker;
            }
        }
        else if (downUnit is DishResultContainer)
        {
            DishResultUnit unit = (downUnit as DishResultContainer).result;

            if (unit != null)
            {
                dragUnit = unit;
            }
        }
        else if (downUnit is DishResultBt)
        {
            DishResultUnit unit = (downUnit as DishResultBt).dish.resultUnit;

            if (unit != null)
            {
                dragUnit = unit;
            }
        }

        if (dragUnit != null)
        {
            dragUnit.StartDrag();

            ClearSelectedUnitList();
        }
    }

    private void DragControlUnit(ControlUnit _startUnit, ControlUnit _endUnit)
    {
        if (_startUnit is SeatUnit)
        {
            SeatUnit unit = _startUnit as SeatUnit;

            if (_endUnit is SeatUnit)
            {
                SeatUnit endUnit = _endUnit as SeatUnit;

                if (endUnit.GetWorker() == null)
                {
                    //send command
                    client.ChangeWorkerPos(unit.GetWorker().index, endUnit.index);
                }
            }
        }
        else if (_startUnit is DishResultContainer)
        {
            DishResultContainer unit = _startUnit as DishResultContainer;

            if (_endUnit is DishResultContainer)
            {
                DishResultContainer endUnit = _endUnit as DishResultContainer;

                //send command
                client.ChangeResultPos(unit.index, endUnit.index);
            }
            else if (_endUnit is TrashContainer)
            {
                //send command
                client.ChangeResultPos(unit.index, -1);
            }
        }
        else
        {
            DishResultBt unit = _startUnit as DishResultBt;

            if (_endUnit is DishResultContainer)
            {
                DishResultContainer endUnit = _endUnit as DishResultContainer;

                if (endUnit.result == null)
                {
                    //send command
                    client.CompleteDish(unit.dish.index, endUnit.index);
                }
            }
            else if (_endUnit is TrashContainer)
            {
                //send command
                client.CompleteDish(unit.dish.index, -1);
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("OnPointerUp  hasExit:" + hasExit + "    downUnit:" + downUnit);

            if (dragUnit != null)
            {
                dragUnit.EndDrag();

                if (enterUnit != downUnit)
                {
                    DragControlUnit(downUnit, enterUnit);
                }
            }
            else if (downUnit != null)
            {
                if (!hasExit && enterUnit == downUnit)
                {
                    ClickControlUnit(enterUnit);
                }
            }

            downUnit = enterUnit = null;

            hasExit = false;

            hasDragCheck = false;

            dragUnit = null;
        }
        else if (Input.GetMouseButton(0))
        {
            if (!hasDragCheck && downUnit != null && Vector2.Distance(downPos, Input.mousePosition) > 10.0f)
            {
                StartDrag();
            }
        }

        if (Input.GetKeyUp(KeyCode.F5))
        {
            RequestRefreshData();
        }
    }

    public void ResultDisappear(ControlUnit _unit)
    {
        UnselectControlUnit(_unit);
    }

    private void UnselectControlUnit(ControlUnit _unit)
    {
        int index = selectedUnitList.IndexOf(_unit);

        if (index != -1)
        {
            selectedUnitList[index].SetSelected(false);

            selectedUnitList.RemoveAt(index);
        }
    }

    private void ClickSeatUnit(SeatUnit _seatUnit)
    {
        if (selectedUnitList.Count == 0)
        {
            if (_seatUnit.GetWorker() != null)
            {
                selectedUnitList.Add(_seatUnit);

                _seatUnit.SetSelected(true);
            }
        }
        else if (selectedUnitList.Count == 1)
        {
            ControlUnit lastSelectedUnit = selectedUnitList[0];

            if (lastSelectedUnit is SeatUnit)
            {
                ClearSelectedUnitList();

                if (_seatUnit.GetWorker() == null)
                {
                    //send command
                    client.ChangeWorkerPos((lastSelectedUnit as SeatUnit).GetWorker().index, _seatUnit.index);
                }
                else
                {
                    if (_seatUnit.GetWorker() != null)
                    {
                        selectedUnitList.Add(_seatUnit);

                        _seatUnit.SetSelected(true);
                    }
                }
            }
            else
            {
                ClearSelectedUnitList();

                if (_seatUnit.GetWorker() != null)
                {
                    selectedUnitList.Add(_seatUnit);

                    _seatUnit.SetSelected(true);
                }
            }
        }
        else
        {
            ClearSelectedUnitList();

            if (_seatUnit.GetWorker() != null)
            {
                selectedUnitList.Add(_seatUnit);

                _seatUnit.SetSelected(true);
            }
        }
    }

    private void ClickDishResultBt(DishResultBt _bt)
    {
        if (_bt.dish.resultUnit != null)
        {
            if (selectedUnitList.Count == 0)
            {
                selectedUnitList.Add(_bt);

                _bt.SetSelected(true);
            }
            else if (selectedUnitList.Count == 1)
            {
                ControlUnit lastSelectedUnit = selectedUnitList[0];

                if (lastSelectedUnit == _bt)
                {
                    ClearSelectedUnitList();
                }
                else
                {
                    if (lastSelectedUnit is DishResultBt || lastSelectedUnit is DishResultContainer)
                    {

                    }
                    else
                    {
                        ClearSelectedUnitList();
                    }

                    selectedUnitList.Add(_bt);

                    _bt.SetSelected(true);
                }
            }
            else
            {
                //ClearSelectedUnitList();

                int index = selectedUnitList.IndexOf(_bt);

                if (index == -1)
                {
                    selectedUnitList.Add(_bt);

                    _bt.SetSelected(true);
                }
                else
                {
                    selectedUnitList.RemoveAt(index);

                    _bt.SetSelected(false);
                }
            }
        }
        else
        {
            ClearSelectedUnitList();
        }
    }

    private void ClickDishResultContainer(DishResultContainer _dishResultContainer)
    {
        if (selectedUnitList.Count == 0)
        {
            if (_dishResultContainer.result != null)
            {
                selectedUnitList.Add(_dishResultContainer);

                _dishResultContainer.SetSelected(true);
            }
        }
        else if (selectedUnitList.Count == 1)
        {
            ControlUnit lastSelectedUnit = selectedUnitList[0];

            if (lastSelectedUnit is DishResultBt)
            {
                if (_dishResultContainer.result == null)
                {
                    DishResultBt dishResultBt = lastSelectedUnit as DishResultBt;

                    //send command
                    client.CompleteDish(dishResultBt.dish.index, _dishResultContainer.index);

                    ClearSelectedUnitList();
                }
                else
                {
                    //ClearSelectedUnitList();

                    selectedUnitList.Add(_dishResultContainer);

                    _dishResultContainer.SetSelected(true);
                }
            }
            else if (lastSelectedUnit is DishResultContainer)
            {
                if (lastSelectedUnit == _dishResultContainer)
                {
                    ClearSelectedUnitList();
                }
                else
                {
                    if (_dishResultContainer.result == null)
                    {
                        DishResultContainer lastDishResultContainer = lastSelectedUnit as DishResultContainer;

                        //send command
                        client.ChangeResultPos(lastDishResultContainer.index, _dishResultContainer.index);

                        ClearSelectedUnitList();
                    }
                    else
                    {
                        selectedUnitList.Add(_dishResultContainer);

                        _dishResultContainer.SetSelected(true);
                    }
                }
            }
            else
            {
                ClearSelectedUnitList();

                if (_dishResultContainer.result != null)
                {
                    selectedUnitList.Add(_dishResultContainer);

                    _dishResultContainer.SetSelected(true);
                }
            }
        }
        else
        {
            if (_dishResultContainer.result != null)
            {
                int index = selectedUnitList.IndexOf(_dishResultContainer);

                if (index == -1)
                {
                    selectedUnitList.Add(_dishResultContainer);

                    _dishResultContainer.SetSelected(true);
                }
                else
                {
                    selectedUnitList.RemoveAt(index);

                    _dishResultContainer.SetSelected(false);
                }
            }
            else
            {
                ClearSelectedUnitList();
            }
        }
    }

    private void ClickRequirementUnitContainer(RequirementUnitContainer _container)
    {
        if (selectedUnitList.Count == _container.requirement.dishArr.Length)
        {
            List<int> list = new List<int>();

            for (int i = 0; i < selectedUnitList.Count; i++)
            {
                ControlUnit controlUnit = selectedUnitList[i];

                if (controlUnit is DishResultContainer)
                {
                    DishResultContainer container = controlUnit as DishResultContainer;

                    list.Add(container.index);
                }
                else
                {
                    DishResultBt bt = controlUnit as DishResultBt;

                    list.Add(-bt.dish.index - 1);
                }
            }

            if (client.CheckCanCompleteRequirement(list, _container.requirement))
            {
                //send command
                client.CompleteRequirement(list, _container.requirement.uid);
            }
        }

        ClearSelectedUnitList();
    }

    private void ClickTrashContainer()
    {
        ClearSelectedUnitList();
    }
}
