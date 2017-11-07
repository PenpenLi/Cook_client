using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    private const float MAX_LENGTH = 360;

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

        float time = sds.prepareTime + sds.cookTime + sds.optimizedTime;
    }
}
