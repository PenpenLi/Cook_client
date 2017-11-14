using System.Collections.Generic;
using UnityEngine;
using Cook_lib;
using gameObjectFactory;
using superTween;

public class RequirementContainer : MonoBehaviour
{
    [SerializeField]
    private float max_length;

    [SerializeField]
    private RectTransform container;

    private DishClientCore core;

    private Dictionary<int, DishRequirement> dataDic;

    public Dictionary<int, RequirementUnitContainer> dic = new Dictionary<int, RequirementUnitContainer>();

    private float moveDisPerTick;

    private int tweenID = -1;

    private float fix;

    public void Init(DishClientCore _core, Dictionary<int, DishRequirement> _dataDic)
    {
        core = _core;

        dataDic = _dataDic;

        moveDisPerTick = max_length / (CookConst.REQUIRE_EXCEED_TIME * CookConst.TICK_NUM_PER_SECOND);

        Canvas c = transform.GetComponentInParent<Canvas>();

        

        //fix = c.pixelRect.width / c.;
    }

    public void RefreshData()
    {
        Refresh();

        float x = -moveDisPerTick * core.tick;

        container.anchoredPosition = new Vector2(x, 0);
    }

    public void UpdateCallBack()
    {
        RefreshData();

        StopTween();

        tweenID = SuperTween.Instance.To(container.anchoredPosition.x, container.anchoredPosition.x - moveDisPerTick, DishClientCore.TICK_SPAN, To, Over);
    }

    private void Refresh()
    {
        IEnumerator<DishRequirement> enumerator = dataDic.Values.GetEnumerator();

        while (enumerator.MoveNext())
        {
            DishRequirement requirement = enumerator.Current;

            RequirementUnitContainer unit;

            if (!dic.TryGetValue(requirement.uid, out unit))
            {
                GameObject go = GameObjectFactory.Instance.GetGameObject("Assets/Resource/prefab/requirement.prefab", null);

                go.transform.SetParent(container, false);

                unit = go.GetComponent<RequirementUnitContainer>();

                unit.Init(core, requirement);

                dic.Add(requirement.uid, unit);

                int bornTick = core.tick - requirement.time;

                float x = moveDisPerTick * bornTick + max_length * 0.5f;

                (unit.transform as RectTransform).anchoredPosition = new Vector2(x, 0);
            }
        }

        List<int> delList = null;

        IEnumerator<int> enumerator2 = dic.Keys.GetEnumerator();

        while (enumerator2.MoveNext())
        {
            if (!dataDic.ContainsKey(enumerator2.Current))
            {
                if (delList == null)
                {
                    delList = new List<int>();
                }

                delList.Add(enumerator2.Current);
            }
        }

        if (delList != null)
        {
            for (int i = 0; i < delList.Count; i++)
            {
                int uid = delList[i];

                Disappear(dic[uid]);

                dic.Remove(uid);
            }
        }
    }

    //void LateUpdate()
    //{
    //    container.anchoredPosition = new Vector2(Mathf.Round(container.anchoredPosition.x), 0);
    //}

    private void To(float _v)
    {
        container.anchoredPosition = new Vector2(_v, 0);
    }

    private void Over()
    {
        tweenID = -1;
    }

    private void StopTween()
    {
        if (tweenID != -1)
        {
            SuperTween.Instance.Remove(tweenID);

            tweenID = -1;
        }
    }

    private void Disappear(RequirementUnitContainer _unit)
    {
        Destroy(_unit.gameObject);
    }

    public void Clear()
    {
        container.anchoredPosition = Vector2.zero;

        IEnumerator<RequirementUnitContainer> enumerator = dic.Values.GetEnumerator();

        while (enumerator.MoveNext())
        {
            Destroy(enumerator.Current.gameObject);
        }

        dic.Clear();
    }
}
