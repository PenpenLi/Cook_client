using System.IO;
public class ResultSDS_c {
    public static void Init(ResultSDS _csv, BinaryReader _br){
        _csv.isUniversal = _br.ReadBoolean();
        _csv.ID = _br.ReadInt32();
        _csv.maxNum = _br.ReadInt32();
        _csv.money = _br.ReadInt32();
        _csv.moneyOptimized = _br.ReadInt32();
        _csv.exceedTime = _br.ReadSingle();
        _csv.icon = _br.ReadString();
    }
}
