public class WorkerUnit : DragUnit
{
    public SeatUnit container;

    public int index { private set; get; }

    public void Init(DishClientCore _core, int _index)
    {
        base.Init(_core);

        index = _index;
    }
}
