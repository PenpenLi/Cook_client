using UnityEngine;
using UnityEngine.UI;
using superTween;
using Cook_lib;

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

    private int tweenID = -1;

    private bool hasFirstTime = false;

    private bool isActive = false;

    private float texOffsetY;

    public void Init(float _time)
    {
        time = _time * CookConst.TICK_NUM_PER_SECOND;
    }

    void Awake()
    {
        mat = Instantiate(matResource);

        texOffsetY = mat.GetTextureOffset("_MaskTex").y;

        Clear();
    }

    void Start()
    {
        icon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, icon.rectTransform.rect.height);
    }

    public void SetTime(float _time)
    {
        StopTween();

        if (_time == 0)
        {
            if (isActive)
            {
                isActive = false;

                bg.material = null;

                mat.SetTextureOffset("_MaskTex", new Vector2(1, texOffsetY));

                animator.gameObject.SetActive(false);
            }
        }
        else
        {
            if (!isActive)
            {
                isActive = true;

                bg.material = mat;

                animator.gameObject.SetActive(true);

                transform.SetAsLastSibling();
            }

            if (hasFirstTime)
            {
                tweenID = SuperTween.Instance.To(mat.GetTextureOffset("_MaskTex").x, 1 - _time / time, Dish.TICK_SPAN, TweenTo, TweenOver);
            }
            else
            {
                mat.SetTextureOffset("_MaskTex", new Vector2(1 - _time / time, texOffsetY));
            }
        }

        if (!hasFirstTime)
        {
            hasFirstTime = true;
        }
    }

    private void StopTween()
    {
        if (tweenID != -1)
        {
            SuperTween.Instance.Remove(tweenID);

            tweenID = -1;
        }
    }

    private void Clear()
    {
        StopTween();

        hasFirstTime = false;

        isActive = false;

        animator.gameObject.SetActive(false);

        bg.material = null;

        mat.SetTextureOffset("_MaskTex", new Vector2(1, texOffsetY));
    }

    private void TweenTo(float _v)
    {
        mat.SetTextureOffset("_MaskTex", new Vector2(_v, texOffsetY));
    }

    private void TweenOver()
    {
        tweenID = -1;
    }
}
