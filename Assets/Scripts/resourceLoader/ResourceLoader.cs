using System;
using UnityEngine;
using System.IO;
#if USE_ASSETBUNDLE
using wwwManager;
#endif

public static class ResourceLoader
{
    private static Action callBack;

    private static int num;

    public static void Load(Action _callBack)
    {
        callBack = _callBack;

        LoadConfig(ConfigLoadOver);
    }

    private static void ConfigLoadOver()
    {
        num = 2;

        LoadTables();

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

        StaticData.Load<DishSDS>("dish");
    }

    private static void OneLoadOver()
    {
        num--;

        if (num == 0)
        {
            if (callBack != null)
            {
                Action tmpCb = callBack;

                callBack = null;

                tmpCb();
            }
        }
    }
}
