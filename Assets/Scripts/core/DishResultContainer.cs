using UnityEngine;
using UnityEngine.UI;
using textureFactory;
using Cook_lib;

public class DishResultContainer : MonoBehaviour
{
    [SerializeField]
    private Image bg;

    private DishClientCore core;

    private int index;

    private DishResultUnit result;

    public void Init(DishClientCore _core, int _index, bool _canControl)
    {
        core = _core;

        index = _index;

        bool b = CookConst.RESULT_STATE[index];

        string path = b ? "Assets/Resource/texture/a.png" : "Assets/Resource/texture/b.png";

        TextureFactory.Instance.GetTexture<Sprite>(path, GetSprite, true);

        if (!_canControl)
        {
            bg.raycastTarget = false;
        }
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

    public void Clear()
    {
        if (result != null)
        {
            Destroy(result.gameObject);

            result = null;
        }
    }
}
