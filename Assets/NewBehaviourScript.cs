using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Cook_lib;
using gameObjectFactory;
using System;

public class NewBehaviourScript : MonoBehaviour
{

    [SerializeField]
    private DishClientCore core;

    private int num;

    private Cook_server server = new Cook_server();

    private static string[] strs = new string[]
    {
        "Assets/Resource/prefab/dish.prefab",
        "Assets/Resource/prefab/dishResult.prefab",
        "Assets/Resource/prefab/playerDataUnit.prefab",
        "Assets/Resource/prefab/requirement.prefab",
        "Assets/Resource/prefab/requirementContainer.prefab",
        "Assets/Resource/prefab/requirementUnit.prefab",
        "Assets/Resource/prefab/resultContainer.prefab",
        "Assets/Resource/prefab/seat.prefab",
        "Assets/Resource/prefab/workerUnit.prefab",
    };

    // Use this for initialization
    void Awake()
    {

        Log.Init(Debug.Log);

        ResourceLoader.Load(LoadOver);

        GameObjectFactory.Instance.PreloadGameObjects(strs, LoadOver);

        Time.fixedDeltaTime = (float)1 / CookConst.TICK_NUM_PER_SECOND;


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
        server.ServerUpdate();
    }

    private void LoadOver()
    {
        num++;

        if (num == 2)
        {
            Debug.Log("loadover");

            ServerStart();

            core.Init(SendData, SendDataWithReply);

            core.RequestRefreshData();
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
        Cook_server.Init(StaticData.GetDic<DishSDS>());

        server.ServerSetCallBack(ServerCallBack);

        server.ServerStart(new List<int>() { 1, 2, 3, 4 }, new List<int>() { 1, 2, 3, 4 });
    }
}
