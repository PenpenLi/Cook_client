using UnityEngine;

public class WorkerUnit : MonoBehaviour
{
    public SeatUnit container;

    public int index { private set; get; }

    public void Init(int _index)
    {
        index = _index;
    }
}
