using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Cook_lib;
using System;
using gameObjectFactory;

public class DishClientCore : MonoBehaviour, IClient
{
    [SerializeField]
    private RectTransform requirementContainer;

    [SerializeField]
    private PlayerDataUnit mPlayerData;

    [SerializeField]
    private PlayerDataUnit oPlayerData;

    private Cook_client client;

    private Dictionary<int, RequirementContainer> requirementList = new Dictionary<int, RequirementContainer>();

    void Awake()
    {
        client = new Cook_client();

        client.Init(this);

        mPlayerData.Init(this, true);

        oPlayerData.Init(this, false);
    }

    public void RefreshData()
    {
        Clear();

        PlayerData playerData = client.GetPlayerData(client.clientIsMine);

        mPlayerData.RefreshData(playerData);

        playerData = client.GetPlayerData(!client.clientIsMine);

        oPlayerData.RefreshData(playerData);


    }

    public void SendData(MemoryStream _ms)
    {

    }

    public void SendData(MemoryStream _ms, Action<BinaryReader> _callBack)
    {

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
        else
        {

        }
    }

    public void UpdateCallBack()
    {
        mPlayerData.UpdateCallBack();

        oPlayerData.UpdateCallBack();


    }

    private void Clear()
    {
        mPlayerData.Clear();

        oPlayerData.Clear();

        IEnumerator<RequirementContainer> enumerator = requirementList.Values.GetEnumerator();

        while (enumerator.MoveNext())
        {
            Destroy(enumerator.Current.gameObject);
        }

        requirementList.Clear();
    }
}
