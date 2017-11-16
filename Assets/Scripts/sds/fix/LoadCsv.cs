using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;
public class LoadCsv {
    public static Dictionary<Type,IDictionary> Init(byte[] _bytes) {
        MemoryStream ms = new MemoryStream(_bytes);
        BinaryReader br = new BinaryReader(ms);
        Dictionary<Type,IDictionary> dic = new Dictionary<Type,IDictionary>();
        Dictionary<int,DishSDS> DishSDSDic = new Dictionary<int,DishSDS>();
        int lengthDishSDS = br.ReadInt32();
        for(int i = 0 ; i < lengthDishSDS ; i++){
            DishSDS unit = new DishSDS();
            DishSDS_c.Init(unit,br);
            unit.Fix();
            DishSDSDic.Add(unit.ID,unit);
        }
        dic.Add(typeof(DishSDS),DishSDSDic);
        Dictionary<int,ResultSDS> ResultSDSDic = new Dictionary<int,ResultSDS>();
        int lengthResultSDS = br.ReadInt32();
        for(int i = 0 ; i < lengthResultSDS ; i++){
            ResultSDS unit = new ResultSDS();
            ResultSDS_c.Init(unit,br);
            unit.Fix();
            ResultSDSDic.Add(unit.ID,unit);
        }
        dic.Add(typeof(ResultSDS),ResultSDSDic);
        br.Close();
        ms.Close();
        ms.Dispose();
        return dic;
    }
}
