using UnityEngine;
using Cook_lib;
using UnityEngine.UI;
using textureFactory;
using gameObjectFactory;

public class Dish : MonoBehaviour
{
    public const float MAX_LENGTH = 360;

    [SerializeField]
    private RectTransform container;

    [SerializeField]
    private DishResultBt dishResultBt;

    [SerializeField]
    public SeatUnit dishWorkerBt;

    [SerializeField]
    private DishUnit prepare;

    [SerializeField]
    private DishUnit cook;

    [SerializeField]
    private DishUnit optimize;

    [SerializeField]
    private Image resultIcon;

    [SerializeField]
    private GameObject resultGo;

    [SerializeField]
    private RectTransform resultContainer;

    [SerializeField]
    private RectTransform workerContainer;

    [SerializeField]
    private Graphic[] controlGraphic;

    private DishData dishData;

    private int index;

    private DishResultUnit resultUnit;

    private DishClientCore core;

    public void Init(DishClientCore _core, DishData _dishData, int _index, bool _canControl)
    {
        dishData = _dishData;

        index = _index;

        core = _core;

        dishResultBt.Init(core);

        dishWorkerBt.Init(core, index, _canControl);

        if (!_canControl)
        {
            for (int i = 0; i < controlGraphic.Length; i++)
            {
                controlGraphic[i].raycastTarget = false;
            }
        }

        float time = dishData.sds.GetPrepareTime() + dishData.sds.GetCookTime() + dishData.sds.GetOptimizeTime();

        container.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, time / DishClientCore.MAX_TIME * MAX_LENGTH);

        prepare.Init(dishData.sds.GetPrepareTime());

        float prepareLength = dishData.sds.GetPrepareTime() / DishClientCore.MAX_TIME * MAX_LENGTH;

        (prepare.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, prepareLength);

        float cookLength = 0;

        if (dishData.sds.GetCookTime() > 0)
        {
            if (!cook.gameObject.activeSelf)
            {
                cook.gameObject.SetActive(true);
            }

            cook.Init(dishData.sds.GetCookTime());

            cookLength = dishData.sds.GetCookTime() / DishClientCore.MAX_TIME * MAX_LENGTH;

            (cook.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cookLength);

            (cook.transform as RectTransform).anchoredPosition = new Vector2(prepareLength, 0);
        }
        else
        {
            if (cook.gameObject.activeSelf)
            {
                cook.gameObject.SetActive(false);
            }
        }

        optimize.Init(dishData.sds.GetOptimizeTime());

        float optimizeLength = dishData.sds.GetOptimizeTime() / DishClientCore.MAX_TIME * MAX_LENGTH;

        (optimize.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, optimizeLength);

        (optimize.transform as RectTransform).anchoredPosition = new Vector2(prepareLength + cookLength, 0);

        TextureFactory.Instance.GetTexture<Sprite>("Assets/Resource/texture/" + (dishData.sds as DishSDS).icon + ".png", GetSprite, true);

        if (dishData.result != null)
        {
            DishResultAppear();
        }
    }

    private void GetSprite(Sprite _sp)
    {
        resultIcon.sprite = _sp;
    }

    public void Refresh()
    {
        switch (dishData.state)
        {
            case DishState.NULL:

                prepare.SetTime(0);

                if (dishData.sds.GetCookTime() > 0)
                {
                    cook.SetTime(0);
                }

                optimize.SetTime(0);

                break;

            case DishState.PREPAREING:

                prepare.SetTime(dishData.time);

                if (dishData.sds.GetCookTime() > 0)
                {
                    cook.SetTime(0);
                }

                optimize.SetTime(0);

                break;

            case DishState.COOKING:

                prepare.SetTime(0);

                cook.SetTime(dishData.time);

                optimize.SetTime(0);

                break;

            default:

                prepare.SetTime(0);

                if (dishData.sds.GetCookTime() > 0)
                {
                    cook.SetTime(0);
                }

                optimize.SetTime(dishData.time);

                break;
        }

        if (resultUnit != null)
        {
            if (dishData.result == null)
            {
                DishResultDisappear();
            }
            else
            {
                resultUnit.RefreshTime();

                resultUnit.RefreshIsOptimized();
            }
        }
        else
        {
            if (dishData.result != null)
            {
                DishResultAppear();
            }
        }
    }

    public void DishResultAppear()
    {
        GameObject go = GameObjectFactory.Instance.GetGameObject("Assets/Resource/prefab/dishResult.prefab", null);

        go.transform.SetParent(resultContainer, false);

        resultUnit = go.GetComponent<DishResultUnit>();

        resultUnit.Init(dishData.result);

        resultGo.SetActive(false);
    }

    private void DishResultDisappear()
    {
        resultUnit.StopTween();

        Destroy(resultUnit.gameObject);

        resultUnit = null;

        resultGo.SetActive(true);

        core.ResultDisappear(dishResultBt);
    }

    public void SetWorker(WorkerUnit _workerUnit)
    {
        dishWorkerBt.SetWorker(_workerUnit);
    }
}
