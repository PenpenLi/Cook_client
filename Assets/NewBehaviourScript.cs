using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cook_lib;

public class NewBehaviourScript : MonoBehaviour {

    public Dish dish;

    private DishData dd;

	// Use this for initialization
	void Start () {

        ResourceLoader.Load(LoadOver);
	}

    private void LoadOver()
    {
        Debug.Log("loadover");

        Dish.InitData();

        dd = new DishData();

        dd.sds = StaticData.GetData<DishSDS>(1);

        dish.Init(dd);

        Time.fixedDeltaTime = 1.0f / CookConst.TICK_NUM_PER_SECOND;
    }

    void FixedUpdate()
    {
        dd.state = DishState.PREPAREING;

        dd.time++;

        dish.Refresh();    
    }

    // Update is called once per frame
    void Update () {
		
	}
}
