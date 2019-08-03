using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Reflection;

public static class CSVLoader 
{
    public static string filePath = Application.streamingAssetsPath + "/cardcsv.csv";
    /// <summary>
    /// LoadCsvFile(string) 返回字典的表格读取函数
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static Dictionary<string,Dictionary<string,string>> LoadCsvFile(string filePath)
    {
        Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();
        //根据路径读取文件并储存到字符串表中
        string[] fileData = File.ReadAllLines(filePath);
        //保存第一行的Key字段
        string[] keys = fileData[0].Split(',');
        for(int i = 1; i < fileData.Length; i++)
        {
            //储存当前行的数据
            string[] line = fileData[i].Split(',');
            //把当前行第一个数据ID单独拿出来,以string形式注册到字典中,作为id
            string ID = line[0];
            result[ID] = new Dictionary<string, string>();
            for(int j = 0; j < line.Length; j++)
            {
                //对于当前行，key-value
                result[ID][keys[j]] = line[j];
            }
        }
        return result;
    }
    //泛型+反射+属性加载CSV文件
    public static Dictionary<int,T_CsvData> LoadCsvData<T_CsvData>(string csvFilePath)
    {
        Dictionary<int, T_CsvData> dic = new Dictionary<int, T_CsvData>();

        Dictionary<string, Dictionary<string, string>> csvdatas = LoadCsvFile(filePath);
        foreach(string id in csvdatas.Keys)
        {
            Dictionary<string, string> datas = csvdatas[id];

            PropertyInfo[] props = typeof(T_CsvData).GetProperties();
            T_CsvData obj = Activator.CreateInstance<T_CsvData>();
            foreach(PropertyInfo pi in props)
            {
                pi.SetValue(obj, Convert.ChangeType(datas[pi.Name], pi.PropertyType),null);
            }
            
            dic[Convert.ToInt32(id)] = obj;
        }
        return dic;
    }

    public static CardKind StringToEnum(string st)
    {
        CardKind result=CardKind.PlayerCard;
        result = (CardKind)Enum.Parse(typeof(CardKind), st);
        //switch (st)
        //{
        //    case "PlayerCard":
        //        result = CardKind.PlayerCard;
        //        break;
        //    case "StateCard":
        //        result = CardKind.PlayerCard;
        //        break;
        //    case "CurseCard":
        //        result = CardKind.PlayerCard;
        //        break;
        //}
        return result;
    }
}
