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

    private List<DishRequirement> dataList;

    public List<RequirementUnitContainer> containerList = new List<RequirementUnitContainer>();

    private float moveDisPerTick;

    private int tweenID = -1;

    private float fix;

    public void Init(DishClientCore _core, List<DishRequirement> _dataList)
    {
        core = _core;

        dataList = _dataList;

        moveDisPerTick = max_length / (CookConst.REQUIRE_EXCEED_TIME * CookConst.TICK_NUM_PER_SECOND);
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
        for (int i = 0; i < dataList.Count; i++)
        {
            DishRequirement requirement = dataList[i];

            bool getContainer = false;

            for (int m = 0; m < containerList.Count; m++)
            {
                if (containerList[m].requirement == requirement)
                {
                    getContainer = true;

                    break;
                }
            }

            if (!getContainer)
            {
                GameObject go = GameObjectFactory.Instance.GetGameObject("Assets/Resource/prefab/requirement.prefab", null);

                go.transform.SetParent(container, false);

                RequirementUnitContainer unit = go.GetComponent<RequirementUnitContainer>();

                unit.Init(core, requirement);

                containerList.Add(unit);

                int bornTick = core.tick - requirement.time;

                float x = moveDisPerTick * bornTick + max_length * 0.5f;

                (unit.transform as RectTransform).anchoredPosition = new Vector2(x, 0);
            }
        }

        for (int i = containerList.Count - 1; i > -1; i--)
        {
            RequirementUnitContainer unit = containerList[i];

            if (!dataList.Contains(unit.requirement))
            {
                containerList.RemoveAt(i);

                Disappear(unit);
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

        for (int i = 0; i < containerList.Count; i++)
        {
            Destroy(containerList[i].gameObject);
        }

        containerList.Clear();
    }
}
