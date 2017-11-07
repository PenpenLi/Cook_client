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

    private DishResult dishResult;

    private float exceedTime;

    private bool isOptimized;

    public void Init(DishResult _dishResult)
    {
        dishResult = _dishResult;

        exceedTime = dishResult.sds.GetExceedTime() * CookConst.TICK_NUM_PER_SECOND;

        isOptimized = _dishResult.isOptimized;

        bg.color = isOptimized ? Color.white : Color.black;

        TextureFactory.Instance.GetTexture<Sprite>((dishResult.sds as DishSDS).icon, GetSprite, true);
    }

    private void GetSprite(Sprite _sp)
    {
        icon.sprite = _sp;
    }

    public void Refresh()
    {
        bg.fillAmount = dishResult.time / exceedTime;

        if (dishResult.isOptimized && !isOptimized)
        {
            bg.color = Color.white;
        }
    }
}
