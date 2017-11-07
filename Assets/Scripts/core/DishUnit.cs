using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DishUnit : MonoBehaviour
{
    [SerializeField]
    private Image bg;

    [SerializeField]
    private Image icon;

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

    void Start()
    {
        icon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, icon.rectTransform.rect.height);
    }

    void Update()
    {

    }
}
