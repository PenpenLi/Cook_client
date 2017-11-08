using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using textureFactory;

public class DishResultContainer : MonoBehaviour
{
    [SerializeField]
    private Image bg;

    private DishResultUnit result;

    public void Init(bool _b)
    {
        string path = _b ? "Assets/Resource/texture/a.png" : "Assets/Resource/texture/b.png";

        TextureFactory.Instance.GetTexture<Sprite>(path, GetSprite, true);
    }

    private void GetSprite(Sprite _sp)
    {
        bg.sprite = _sp;
    }

    public void SetResult(DishResultUnit _result)
    {
        result = _result;
    }

    public void Refresh()
    {
        if (result != null)
        {
            result.RefreshTime();
        }
    }
}
