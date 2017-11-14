using UnityEngine;
using UnityEngine.UI;
using Cook_lib;
using textureFactory;
using superTween;

public class DishResultUnit : DragUnit
{
    [SerializeField]
    private Image bg;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Image timeIcon;

    private DishResult dishResult;

    private float exceedTime;

    private int tweenID = -1;

    public void Init(DishClientCore _core, DishResult _dishResult)
    {
        base.Init(_core);

        dishResult = _dishResult;

        exceedTime = dishResult.sds.GetExceedTime() * CookConst.TICK_NUM_PER_SECOND;

        TextureFactory.Instance.GetTexture<Sprite>("Assets/Resource/texture/" + (dishResult.sds as DishSDS).icon + ".png", GetSprite, true);

        timeIcon.fillAmount = dishResult.time / exceedTime;

        RefreshIsOptimized();
    }

    private void GetSprite(Sprite _sp)
    {
        icon.sprite = _sp;
    }

    public void RefreshTime()
    {
        StopTween();

        tweenID = SuperTween.Instance.To(timeIcon.fillAmount, dishResult.time / exceedTime, DishClientCore.TICK_SPAN, To, Over);

        timeIcon.fillAmount = dishResult.time / exceedTime;
    }

    private void To(float _v)
    {
        timeIcon.fillAmount = _v;
    }

    private void Over()
    {
        tweenID = -1;
    }

    public void StopTween()
    {
        if (tweenID != -1)
        {
            SuperTween.Instance.Remove(tweenID);

            tweenID = -1;
        }
    }

    public void RefreshIsOptimized()
    {
        bg.color = dishResult.isOptimized ? Color.white : Color.black;
    }
}
