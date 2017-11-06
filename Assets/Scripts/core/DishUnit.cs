using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DishUnit : MonoBehaviour {

    [SerializeField]
    private Image bg;

    [SerializeField]
    private Material matResource;

    [SerializeField]
    private Animator animator;

    private Material mat;

    private float time;

	public void Init(float _time)
    {
        time = _time;
    }
}
