using System;
using UnityEngine;
using System.IO;
using gameObjectFactory;
#if USE_ASSETBUNDLE
using wwwManager;
using assetManager;
using assetBundleManager;
#endif

public static class ResourceLoader
{
    private static string[] strs = new string[]
    {
        "Assets/Resource/prefab/core.prefab",
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

    private static Action callBack;

    private static int num;

    public static void Load(Action _callBack)
    {
        callBack = _callBack;

#if USE_ASSETBUNDLE

        int num = 2;

        Action dele = delegate ()
        {
            num--;

            if (num == 0)
            {
                ConfigLoadOver();
            }
        };

        LoadConfig(dele);

        AssetManager.Instance.Init(dele);
#else

        LoadConfig(ConfigLoadOver);
#endif
    }

    private static void ConfigLoadOver()
    {
        num = 3;

        LoadTables();

        PreloadPrefabs();

        OneLoadOver();
    }

    public static void LoadConfig(Action _callBack)
    {
#if !USE_ASSETBUNDLE

        LoadConfigLocal();

        if (_callBack != null)
        {
            _callBack();
        }
#else
        Action<WWW> dele = delegate (WWW _www)
        {
            ConfigDictionary.Instance.SetData(_www.text);

            if (_callBack != null)
            {
                _callBack();
            }
        };

        WWWManager.Instance.Load("local.xml", dele);
#endif
    }

    public static void LoadConfigLocal()
    {
        ConfigDictionary.Instance.LoadLocalConfig(Path.Combine(Application.streamingAssetsPath, "local.xml"));
    }

    private static void LoadTables()
    {
#if !USE_ASSETBUNDLE

        LoadTablesLocal();

        OneLoadOver();
#else
        StaticData.LoadCsvDataFromFile(OneLoadOver, LoadCsv.Init);
#endif
    }

    public static void LoadTablesLocal()
    {
        StaticData.path = ConfigDictionary.Instance.table_path;

        StaticData.Dispose();

        StaticData.Load<ResultSDS>("result");

        StaticData.Load<DishSDS>("dish");
    }

    private static void PreloadPrefabs()
    {
#if USE_ASSETBUNDLE
        AssetBundleManager.Instance.Load("texture", null);
#endif

        GameObjectFactory.Instance.PreloadGameObjects(strs, OneLoadOver);
    }

    private static void OneLoadOver()
    {
        num--;

        if (num == 0)
        {
            GameObjectFactory.Instance.GetGameObject("Assets/Resource/prefab/core.prefab", null);

            if (callBack != null)
            {
                Action tmpCb = callBack;

                callBack = null;

                tmpCb();
            }
        }
    }
}
