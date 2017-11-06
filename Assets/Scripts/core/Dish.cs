using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour {

    [SerializeField]
    private DishUnit[] units;

    private DishSDS sds;

    public void Init(DishSDS _sds)
    {
        sds = _sds;
    }
}
