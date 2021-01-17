using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;


public class GenericDataMgr
{
    public static GenericData_SO genericData_SO = Resources.Load<GenericData_SO>("GenericData_SO");

    public static string GetPathFromSaveFile()
    {
        return Path.Combine(Application.persistentDataPath, "genericData.json");
    }   

    public static void Sync_Persis_To_Cache(){
        TextAsset jsonData = Resources.Load<TextAsset>("genericData");
        GenericData genericPersisData = JsonUtility.FromJson<GenericData>(jsonData.ToString());
            

        // GenericData-to-add
        // Generic Data에 추가되는 항목은 여기에도 추가하세요.            
        genericData_SO.ChessPieces.Clear();
        genericData_SO.Skills.Clear();
        genericData_SO.mana.upgrades.Clear();
        genericData_SO.will.upgrades.Clear();
        foreach(Will_Upgrade w in genericPersisData.will.upgrades){
            genericData_SO.will.upgrades.Add(w);
        }
        foreach(Mana_Upgrade m in genericPersisData.mana.upgrades){
            genericData_SO.mana.upgrades.Add(m);
        }
        foreach(ChessPiece_Generic c in genericPersisData.ChessPieces){
            genericData_SO.ChessPieces.Add(c);
        }
        foreach(Skill_Generic s in genericPersisData.Skills){
            genericData_SO.Skills.Add(s);
        }


        Debug.Log("GenericDataMgr: GENERIC_DATA (PERSIS->CACHE) COMPLETE");

    }

    /// <summary>
    /// 이제 더이상 안 쓸 함수입니다.
    /// 쓰지마세요!
    /// </summary>

    public static void Sync_Cache_To_Persis(){
        GenericData genericPersisData = new GenericData();



        // GenericData-to-add
        // Generic Data에 추가되는 항목은 여기에도 추가하세요.
        genericPersisData.ChessPieces.Clear();
        genericPersisData.Skills.Clear();
        genericPersisData.mana.upgrades.Clear();
        genericPersisData.will.upgrades.Clear();
        foreach(Will_Upgrade w in genericData_SO.will.upgrades){
            genericPersisData.will.upgrades.Add(w);
        }
        foreach(Mana_Upgrade m in genericData_SO.mana.upgrades){
            genericPersisData.mana.upgrades.Add(m);
        }        
        foreach(ChessPiece_Generic c in genericData_SO.ChessPieces){
            genericPersisData.ChessPieces.Add(c);
        }
        foreach(Skill_Generic s in GenericDataMgr.genericData_SO.Skills){
            genericPersisData.Skills.Add(s);
        }



        string JsonData = JsonUtility.ToJson(genericPersisData,true);
        string path = GetPathFromSaveFile();
        using (FileStream stream = File.Open(path, FileMode.Create))
        {

            byte[] byteData = Encoding.UTF8.GetBytes(JsonData);

            stream.Write(byteData, 0, byteData.Length);

            stream.Close();

            Debug.Log("GenericDataMgr: GENERIC_DATA (CACHE->PERSIS) COMPLETE \n" + path);
        }             
    }



    public static void ShowData(){

        Debug.Log("********************* GenericDataMgr SHOW DATA *********************");

        // Debug.Log("-------------Will-------------");
        // for(int i = 0 ; i < ){
        //     Debug.Log("Name : " + c.name_kor + 
        //     "\n Desciprtion : " + c.description + 
        //     "\n Health : " + c.health + 
        //     "\n Damage : " + c.damage + 
        //     "\n Cost : " + c.cost);

        // }
        // Debug.Log("-------------------------------------");

        // 이 부분을 수정할때는  
        // GenericData-to-add
        // 를 검색하여 수정하세요.
        Debug.Log("-------------ChessPieces-------------");
        foreach(ChessPiece_Generic c in genericData_SO.ChessPieces){
            Debug.Log("Name : " + c.name_kor + 
            "\n Desciprtion : " + c.description + 
            "\n Health : " + c.health + 
            "\n Damage : " + c.damage + 
            "\n Cost : " + c.cost);

        }
        Debug.Log("-------------------------------------");



        // 이 부분을 수정할때는  
        // GenericData-to-add
        // 를 검색하여 수정하세요.
        Debug.Log("-------------Skills-------------");
        foreach(Skill_Generic s in genericData_SO.Skills){
            Debug.Log("Name : " + s.name_kor + 
            "\n Desciprtion : " + s.description + 
            "\n Damage : " + s.damage + 
            "\n Cost : " + s.cost);
        }

        Debug.Log("---------------------------------");




        Debug.Log("********************* END *********************");

    }


}
