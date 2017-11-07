using System.IO;
public class DishSDS_c {
    public static void Init(DishSDS _csv, BinaryReader _br){
        _csv.isUniversal = _br.ReadBoolean();
        _csv.ID = _br.ReadInt32();
        _csv.maxNum = _br.ReadInt32();
        _csv.money = _br.ReadInt32();
        _csv.moneyOptimized = _br.ReadInt32();
        _csv.cookTime = _br.ReadSingle();
        _csv.exceedTime = _br.ReadSingle();
        _csv.optimizeDecreaseValue = _br.ReadSingle();
        _csv.optimizeTime = _br.ReadSingle();
        _csv.prepareDecreaseValue = _br.ReadSingle();
        _csv.prepareTime = _br.ReadSingle();
    }
}
