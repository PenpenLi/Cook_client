﻿using UnityEngine;
using Cook_lib;
using gameObjectFactory;

public class RequirementUnitContainer : ControlUnit
{
    public DishRequirement requirement { private set; get; }

    public void Init(DishClientCore _core, DishRequirement _requirement)
    {
        Init(_core);

        requirement = _requirement;

        int column = (requirement.dishArr.Length + 1) / 2;

        (transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, column * 35 + (column + 1) * 10);

        for (int i = 0; i < requirement.dishArr.Length; i++)
        {
            GameObject go = GameObjectFactory.Instance.GetGameObject("Assets/Resource/prefab/requirementUnit.prefab", null);

            RequirementUnit unit = go.GetComponent<RequirementUnit>();

            unit.Init(requirement.dishArr[i]);

            go.transform.SetParent(transform, false);

            float x = 10 + i / 2 * 45;

            float y = -10 - i % 2 * 45;

            (go.transform as RectTransform).anchoredPosition = new Vector2(x, y);
        }
    }
}
