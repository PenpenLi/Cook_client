using System.IO;
using UnityEngine;
using Cook_lib;
using System;
using System.Collections.Generic;

public class DishClientCore : MonoBehaviour, IClient
{
    public static float MAX_TIME;

    public static float TICK_SPAN;

    [SerializeField]
    private RequirementContainer requirementContainer;

    [SerializeField]
    private PlayerDataUnit mPlayerData;

    [SerializeField]
    private PlayerDataUnit oPlayerData;

    [SerializeField]
    private Background bg;

    private Cook_client client;

    private Action<MemoryStream> sendData;

    private Action<MemoryStream, Action<BinaryReader>> sendDataWithReply;

    public int tick
    {
        get
        {
            return client.GetTick();
        }
    }

    public void Init(Action<MemoryStream> _sendData, Action<MemoryStream, Action<BinaryReader>> _sendDataWithReply)
    {
        sendData = _sendData;

        sendDataWithReply = _sendDataWithReply;

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

        client = new Cook_client();

        client.Init(this);

        mPlayerData.Init(this, true);

        oPlayerData.Init(this, false);

        requirementContainer.Init(this, client.GetRequirement());

        bg.Init(this);
    }

    public void RefreshData()
    {
        Clear();

        PlayerData playerData = client.GetPlayerData(client.clientIsMine);

        mPlayerData.RefreshData(playerData);

        playerData = client.GetPlayerData(!client.clientIsMine);

        oPlayerData.RefreshData(playerData);

        requirementContainer.RefreshData();
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
        }
        else if (_event is CommandChangeWorkerPos)
        {
            CommandChangeWorkerPos command = (CommandChangeWorkerPos)_event;

            GetCommandChangeWorkerPos(command);
        }
        else if (_event is CommandCompleteDish)
        {

        }
        else if (_event is CommandCompleteRequirement)
        {

        }
    }

    private void GetCommandChangeResultPos(CommandChangeResultPos _command)
    {
        PlayerDataUnit unit = _command.isMine == client.clientIsMine ? mPlayerData : oPlayerData;

        
    }

    private void GetCommandChangeWorkerPos(CommandChangeWorkerPos _command)
    {
        PlayerDataUnit unit = _command.isMine == client.clientIsMine ? mPlayerData : oPlayerData;

        unit.ChangeWorkerPos(_command);
    }

    public void UpdateCallBack()
    {
        mPlayerData.UpdateCallBack();

        oPlayerData.UpdateCallBack();

        requirementContainer.UpdateCallBack();
    }

    private void Clear()
    {
        mPlayerData.Clear();

        oPlayerData.Clear();

        requirementContainer.Clear();
    }

    public void GetPackage(BinaryReader _br)
    {
        client.ClientGetPackage(_br);
    }

    public void RequestRefreshData()
    {
        client.RefreshData();
    }

    private ControlUnit downUnit;

    private ControlUnit enterUnit;

    private bool hasExit;

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
            enterUnit = downUnit = _unit;
        }

        hasExit = false;
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
    }

    private void ClearSelectedUnitList()
    {
        for (int i = 0; i < selectedUnitList.Count; i++)
        {
            selectedUnitList[i].SetSelected(false);
        }

        selectedUnitList.Clear();
    }

    private void DragControlUnit(ControlUnit _startUnit, ControlUnit _endUnit)
    {
        //Debug.Log("DragControlUnit:" + _startUnit + "----->" + _endUnit);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("OnPointerUp  hasExit:" + hasExit + "    downUnit:" + downUnit);

            if (downUnit != null)
            {
                if (!hasExit && enterUnit == downUnit)
                {
                    ClickControlUnit(enterUnit);
                }
                else
                {
                    if (enterUnit != null && enterUnit != downUnit)
                    {
                        DragControlUnit(downUnit, enterUnit);
                    }
                }
            }

            downUnit = enterUnit = null;

            hasExit = false;
        }
        else if (Input.GetMouseButton(0))
        {

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
                //send command

                client.ChangeWorkerPos((lastSelectedUnit as SeatUnit).GetWorker().index, _seatUnit.index);

                ClearSelectedUnitList();
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

    }
}
