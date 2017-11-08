using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Cook_lib;
using System;

public class DishClientCore : MonoBehaviour, IClient
{
    private Cook_client client;

    void Awake()
    {
        client = new Cook_client();

        client.Init(this);
    }

    public void RefreshData()
    {

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
}
