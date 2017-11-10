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

        }
        else if (_event is CommandChangeWorkerPos)
        {

        }
        else if (_event is CommandCompleteDish)
        {

        }
        else if (_event is CommandCompleteRequirement)
        {

        }
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
}
