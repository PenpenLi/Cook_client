using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cook_lib;

public class DishResultUnit : MonoBehaviour
{
    [SerializeField]
    private Image bg;

    [SerializeField]
    private Image icon;

    private DishResult dishResult;

    public void Init(DishResult _dishResult)
    {
        dishResult = _dishResult;


    }
}
