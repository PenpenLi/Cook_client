using UnityEngine;
using UnityEngine.UI;
using Cook_lib;
using textureFactory;

public class RequirementUnit : MonoBehaviour
{
    [SerializeField]
    private Image bg;

    [SerializeField]
    private Image icon;

    private DishResultBase dishResultBase;

    public void Init(DishResultBase _dishResultBase)
    {
        dishResultBase = _dishResultBase;

        TextureFactory.Instance.GetTexture<Sprite>("Assets/Resource/texture/" + (dishResultBase.sds as ResultSDS).icon + ".png", GetSprite, true);

        RefreshIsOptimized();
    }

    private void GetSprite(Sprite _sp)
    {
        icon.sprite = _sp;
    }

    public void RefreshIsOptimized()
    {
        bg.color = dishResultBase.isOptimized ? Color.white : Color.black;
    }
}
