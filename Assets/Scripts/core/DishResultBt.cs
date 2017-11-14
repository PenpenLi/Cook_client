public class DishResultBt : ControlUnit
{
    public Dish dish { private set; get; }

    public void Init(DishClientCore _core, Dish _dish)
    {
        Init(_core);

        dish = _dish;
    }
}
