using UnityEngine;
using UnityEngine.UI;
using textureFactory;
using Cook_lib;

public class DishResultContainer : ControlUnit
{
    [SerializeField]
    private Image bg;

    public int index { private set; get; }

    public DishResultUnit result;

    private DishResult[] dishResultArr;

    public void Init(DishClientCore _core, int _index, bool _canControl)
    {
        Init(_core);

        index = _index;

        bool b = CookConst.RESULT_STATE[index];

        string path = b ? "Assets/Resource/texture/a.png" : "Assets/Resource/texture/b.png";

        TextureFactory.Instance.GetTexture<Sprite>(path, GetSprite, true);

        if (!_canControl)
        {
            bg.raycastTarget = false;
        }
    }

    public void SetData(DishResult[] _dishResultArr)
    {
        dishResultArr = _dishResultArr;
    }

    private void GetSprite(Sprite _sp)
    {
        bg.sprite = _sp;
    }

    public void SetResult(DishResultUnit _result)
    {
        result = _result;

        if (result != null)
        {
            result.transform.SetParent(transform, false);

            (result.transform as RectTransform).anchoredPosition = Vector2.zero;
        }
    }

    public void Refresh()
    {
        if (result != null)
        {
            if (dishResultArr[index] == null)
            {
                Disappear();
            }
            else
            {
                result.RefreshTime();
            }
        }
    }

    private void Disappear()
    {
        Clear();

        core.ResultDisappear(this);
    }

    public void Clear()
    {
        if (result != null)
        {
            result.StopTween();

            Destroy(result.gameObject);

            result = null;
        }
    }
}
