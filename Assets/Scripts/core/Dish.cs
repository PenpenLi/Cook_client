using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    public const float MAX_LENGTH = 360;

    public static float MAX_TIME;

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
    }

    [SerializeField]
    private DishUnit prepare;

    [SerializeField]
    private DishUnit cook;

    [SerializeField]
    private DishUnit optimize;

    private DishSDS sds;

    public void Init(DishSDS _sds)
    {
        sds = _sds;

        float time = sds.prepareTime + sds.cookTime + sds.optimizeTime;

        (transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, time / MAX_TIME * MAX_LENGTH);

        prepare.Init(sds.prepareTime);

        float prepareLength = sds.prepareTime / MAX_TIME * MAX_LENGTH;

        (prepare.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, prepareLength);

        float cookLength = 0;

        if (sds.cookTime > 0)
        {
            cook.gameObject.SetActive(true);

            cook.Init(sds.cookTime);

            cookLength = sds.cookTime / MAX_TIME * MAX_LENGTH;

            (cook.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cookLength);

            (cook.transform as RectTransform).anchoredPosition = new Vector2(prepareLength, 0);
        }

        optimize.Init(sds.optimizeTime);

        float optimizeLength = sds.optimizeTime / MAX_TIME * MAX_LENGTH;

        Debug.Log("optimizeLength:" + optimizeLength);

        (optimize.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, optimizeLength);

        (optimize.transform as RectTransform).anchoredPosition = new Vector2(prepareLength + cookLength, 0);
    }
}
