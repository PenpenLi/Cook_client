using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

    public Dish dish;

	// Use this for initialization
	void Start () {

        ResourceLoader.Load(LoadOver);
	}

    private void LoadOver()
    {
        Debug.Log("loadover");

        Dish.InitData();

        dish.Init(StaticData.GetData<DishSDS>(1));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
