using Cook_lib;

public class DishSDS : CsvBase, IDishSDS
{
    public float prepareTime;
    public float decreaseValue;
    public float cookTime;
    public float exceedTime;
    public float optimizedTime;
    public bool isUniversal;
    public int maxNum;
    public int money;
    public int moneyOptimized;

    public int GetID()
    {
        return ID;
    }

    public float GetPrepareTime()
    {
        return prepareTime;
    }

    public float GetDecreaseValue()
    {
        return decreaseValue;
    }

    public float GetCookTime()
    {
        return cookTime;
    }

    public float GetExceedTime()
    {
        return exceedTime;
    }

    public float GetOptimizeTime()
    {
        return optimizedTime;
    }

    public bool GetIsUniversal()
    {
        return isUniversal;
    }
    public int GetMaxNum()
    {
        return maxNum;
    }

    public int GetMoney()
    {
        return money;
    }

    public int GetMoneyOptimized()
    {
        return moneyOptimized;
    }
}
