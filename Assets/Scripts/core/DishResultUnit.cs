using UnityEngine;
using UnityEngine.UI;
using Cook_lib;
using textureFactory;

public class DishResultUnit : MonoBehaviour
{
    [SerializeField]
    private Image bg;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Image timeIcon;

    private DishResult dishResult;

    private float exceedTime;

    public void Init(DishResult _dishResult)
    {
        dishResult = _dishResult;

        exceedTime = dishResult.sds.GetExceedTime() * CookConst.TICK_NUM_PER_SECOND;

        TextureFactory.Instance.GetTexture<Sprite>((dishResult.sds as DishSDS).icon, GetSprite, true);

        RefreshTime();

        RefreshIsOptimized();
    }

    private void GetSprite(Sprite _sp)
    {
        icon.sprite = _sp;
    }

    public void RefreshTime()
    {
        timeIcon.fillAmount = dishResult.time / exceedTime;
    }

    public void RefreshIsOptimized()
    {
        bg.color = dishResult.isOptimized ? Color.white : Color.black;
    }
}
