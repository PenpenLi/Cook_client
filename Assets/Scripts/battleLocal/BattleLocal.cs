using System.IO;
using System;
using UnityEngine;
using Cook_lib;
using System.Collections.Generic;

public class BattleLocal : MonoBehaviour
{
    public static BattleLocal Instance { private set; get; }

    private Cook_server server = new Cook_server();

    private Action<BinaryReader> clientCallBack;

    void Awake()
    {
        Instance = this;

        server = new Cook_server();

        server.ServerSetCallBack(ServerCallBack);

        Time.fixedDeltaTime = 1f / CookConst.TICK_NUM_PER_SECOND;

        gameObject.SetActive(false);
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
                    DishClientCore.Instance.GetPackage(br);
                }
            }
            else
            {
                if (clientCallBack != null)
                {
                    using (BinaryReader br = new BinaryReader(_ms))
                    {
                        clientCallBack(br);
                    }
                }
            }
        }
    }

    private void SendData(MemoryStream _ms)
    {
        _ms.Position = 0;

        using (BinaryReader br = new BinaryReader(_ms))
        {
            server.ServerGetPackage(true, br);
        }
    }

    private void SendDataWithReply(MemoryStream _ms, Action<BinaryReader> _callBack)
    {
        clientCallBack = _callBack;

        _ms.Position = 0;

        using (BinaryReader br = new BinaryReader(_ms))
        {
            server.ServerGetPackage(true, br);
        }
    }

    public void ServerStart()
    {
        gameObject.SetActive(true);

        Cook_server.Init(StaticData.GetDic<DishSDS>(), StaticData.GetDic<ResultSDS>());

        server.ServerStart(new List<int>() { 1, 2, 3, 4, 5 }, new List<int>() { 1, 2, 3, 4, 5 });

        DishClientCore.Instance.Init(SendData, SendDataWithReply);

        DishClientCore.Instance.RequestRefreshData();

        DishClientCore.Instance.gameObject.SetActive(true);
    }

    void FixedUpdate()
    {
        GameResult gameResult = server.ServerUpdateTo();

        if (gameResult != GameResult.NOT_OVER)
        {
            gameObject.SetActive(false);
        }
    }
}
