using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class StageDataMgr
{

    public static StageData LoadSingleStageData(int stageIdx){
        string jsonFileName = "stageData-" +  stageIdx.ToString();
        TextAsset jsonData = Resources.Load<TextAsset>(jsonFileName);
            
        Debug.Log("StageDataMgr: SINGLE STAGE_DATA LOAD COMPLETE");
        return JsonUtility.FromJson<StageData>(jsonData.ToString());
    }

    public static string GetPathFromSaveFile()
    {
        return Path.Combine(Application.persistentDataPath, "stageData-n.json");
    }

    public static void MakeSingleEmptyStageData(){
        StageData stageData = new StageData();
        string JsonData = JsonUtility.ToJson(stageData,true);
        string path = GetPathFromSaveFile();
        using (FileStream stream = File.Open(path, FileMode.Create))
        {

            byte[] byteData = Encoding.UTF8.GetBytes(JsonData);

            stream.Write(byteData, 0, byteData.Length);

            stream.Close();

            Debug.Log("GenericDataMgr: GENERIC_DATA (CACHE->PERSIS) COMPLETE \n" + path);
        }             
    }   


}
