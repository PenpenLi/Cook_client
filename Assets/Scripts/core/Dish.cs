using System.Collections.Generic;
using UnityEngine;
using Cook_lib;

public class Dish : MonoBehaviour
{
    public const float MAX_LENGTH = 360;

    public static float MAX_TIME;

    public static float TICK_SPAN;

    public static void InitData()
    {
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
    }

    [SerializeField]
    private DishContainer container;

    [SerializeField]
    private DishUnit prepare;

    [SerializeField]
    private DishUnit cook;

    [SerializeField]
    private DishUnit optimize;

    private DishData dishData;

    public void Init(DishData _dishData)
    {
        dishData = _dishData;

        float time = dishData.sds.GetPrepareTime() + dishData.sds.GetCookTime() + dishData.sds.GetOptimizeTime();

        (container.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, time / MAX_TIME * MAX_LENGTH);

        prepare.Init(dishData.sds.GetPrepareTime());

        float prepareLength = dishData.sds.GetPrepareTime() / MAX_TIME * MAX_LENGTH;

        (prepare.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, prepareLength);

        float cookLength = 0;

        if (dishData.sds.GetCookTime() > 0)
        {
            if (!cook.gameObject.activeSelf)
            {
                cook.gameObject.SetActive(true);
            }

            cook.Init(dishData.sds.GetCookTime());

            cookLength = dishData.sds.GetCookTime() / MAX_TIME * MAX_LENGTH;

            (cook.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cookLength);

            (cook.transform as RectTransform).anchoredPosition = new Vector2(prepareLength, 0);
        }
        else
        {
            if (cook.gameObject.activeSelf)
            {
                cook.gameObject.SetActive(false);
            }
        }

        optimize.Init(dishData.sds.GetOptimizeTime());

        float optimizeLength = dishData.sds.GetOptimizeTime() / MAX_TIME * MAX_LENGTH;

        Debug.Log("optimizeLength:" + optimizeLength + "     " + dishData.sds.GetOptimizeTime());

        (optimize.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, optimizeLength);

        (optimize.transform as RectTransform).anchoredPosition = new Vector2(prepareLength + cookLength, 0);
    }

    public void Refresh()
    {
        switch (dishData.state)
        {
            case DishState.NULL:

                prepare.SetTime(0);

                if (dishData.sds.GetCookTime() > 0)
                {
                    cook.SetTime(0);
                }

                optimize.SetTime(0);

                break;

            case DishState.PREPAREING:

                prepare.SetTime(dishData.time);

                if (dishData.sds.GetCookTime() > 0)
                {
                    cook.SetTime(0);
                }

                optimize.SetTime(0);

                break;

            case DishState.COOKING:

                prepare.SetTime(0);

                cook.SetTime(dishData.time);

                optimize.SetTime(0);

                break;

            default:

                prepare.SetTime(0);

                if (dishData.sds.GetCookTime() > 0)
                {
                    cook.SetTime(0);
                }

                optimize.SetTime(dishData.time);

                break;
        }
    }
}
