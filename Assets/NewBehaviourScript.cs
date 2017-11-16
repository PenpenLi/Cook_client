using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Cook_lib;
using System;

public class NewBehaviourScript : MonoBehaviour
{

    [SerializeField]
    private DishClientCore core;

    private Cook_server server = new Cook_server();

    private bool initOK = false;

    // Use this for initialization
    void Awake()
    {
        Log.Init(Debug.Log);

        Time.fixedDeltaTime = (float)1 / CookConst.TICK_NUM_PER_SECOND;

        ResourceLoader.Load(LoadOver);
    }

    private void ServerCallBack(bool _isMine, bool _isPush, MemoryStream _ms)
    {
        if (_isMine)
        {
            _ms.Position = 0;

            if (_isPush)
            {
                using (BinaryReader br = new BinaryReader(_ms))
                {
                    core.GetPackage(br);
                }
            }
            else
            {
                if (tmpCB != null)
                {
                    using (BinaryReader br = new BinaryReader(_ms))
                    {
                        tmpCB(br);
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (initOK)
        {
            //server.ServerUpdate();

            server.ServerUpdateTo();
        }
    }

    private void LoadOver()
    {
        Debug.Log("loadover");

        ServerStart();

        core.Init(SendData, SendDataWithReply);

        core.RequestRefreshData();
    }

    private void SendData(MemoryStream _ms)
    {
        _ms.Position = 0;

        using (BinaryReader br = new BinaryReader(_ms))
        {
            server.ServerGetPackage(true, br);
        }
    }

    private Action<BinaryReader> tmpCB;

    private void SendDataWithReply(MemoryStream _ms, Action<BinaryReader> _callBack)
    {
        tmpCB = _callBack;

        _ms.Position = 0;

        using (BinaryReader br = new BinaryReader(_ms))
        {
            server.ServerGetPackage(true, br);
        }
    }

    private void ServerStart()
    {
        Cook_server.Init(StaticData.GetDic<DishSDS>(), StaticData.GetDic<ResultSDS>());

        server.ServerSetCallBack(ServerCallBack);

        server.ServerStart(new List<int>() { 1, 2, 3, 4, 5 }, new List<int>() { 1, 2, 3, 4, 5 });

        initOK = true;
    }
}
