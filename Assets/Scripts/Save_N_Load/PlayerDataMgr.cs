using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// PlayerData를 관리하는 스크립트입니다.
// (2) DataSyn Functions : Persis -> Cache / Cache -> Persis
public class PlayerDataMgr
{
    public static PlayerData_SO playerData_SO = Resources.Load<PlayerData_SO>("PlayerData_SO");

    #region PUBLIC METHODS
    // 첫 플레이시, 플레이어 데이터를 첫플레이에 맞게 초기화합니다.
    public static void Init_PlayerData(){

        // 이 부분을 수정할때는  
        // PlayerData-to-add
        // 를 검색하여 수정하세요.   

        // 이 부분을 수정할때는  
        // GenericData-to-add
        // 를 검색하여 수정하세요.     

        PlayerData data = new PlayerData();

        // GenericData와 같은 부분은 복사합니다.
        foreach(ChessPiece_Generic c in GenericDataMgr.genericData_SO.ChessPieces){
            // 아군 적군 기물 전부 복사합니다.
            // todo 나중에 아군 기물만으로 수정합니다.
            data.ChessPieces.Add(new ChessPiece_Player(c.code, c.name_kor, 0, false));
            
        }
        foreach(Skill_Generic s in GenericDataMgr.genericData_SO.Skills){
            data.Skills.Add(new Skill_Player(s.code, s.name_kor, 0, false));
        }



        string JsonData = JsonUtility.ToJson(data,true);

        string path = GetPathFromSaveFile();
        using (FileStream stream = File.Open(path, FileMode.Create))
        {

            byte[] byteData = Encoding.UTF8.GetBytes(JsonData);

            stream.Write(byteData, 0, byteData.Length);

            stream.Close();

            Sync_Persis_To_Cache();
            Debug.Log("PlayerDataMgr: INIT COMPLETE - " + path);
        }
    }

    public static void Sync_Persis_To_Cache(){
        PlayerData playerPersisData;
        string path = GetPathFromSaveFile();
        using (FileStream stream = File.Open(path, FileMode.Open))
        {

            byte[] byteData = new byte[stream.Length];

            stream.Read(byteData, 0, byteData.Length);

            stream.Close();

            string JsonData = Encoding.UTF8.GetString(byteData);

            playerPersisData = JsonUtility.FromJson<PlayerData>(JsonData);

        } 

        // PlayerData-to-add
        // Player Data에 추가되는 항목은 여기에도 추가하세요.        
        playerData_SO.ChessPieces.Clear();
        playerData_SO.Skills.Clear();

        playerData_SO.gold = playerPersisData.gold;
        playerData_SO.stageProgress = playerPersisData.stageProgress;
        playerData_SO.willItemCount = playerPersisData.willItemCount;
        playerData_SO.manaItemCount = playerPersisData.manaItemCount;
        foreach (ChessPiece_Player c in playerPersisData.ChessPieces){
            playerData_SO.ChessPieces.Add(c);
        }
        foreach(Skill_Player s in playerPersisData.Skills){
            playerData_SO.Skills.Add(s);
        }


        Debug.Log("PlayerDataMgr: PLAYER_DATA (PERSIS->CACHE) COMPLETE \n " + path);
    }

    public static void Sync_Cache_To_Persis(){
        PlayerData playerPersisData = new PlayerData();


        // PlayerData-to-add
        // Player Data에 추가되는 항목은 여기에도 추가하세요.        
        playerPersisData.ChessPieces.Clear();
        playerPersisData.Skills.Clear();

        playerPersisData.gold = playerData_SO.gold;
        playerPersisData.stageProgress = playerData_SO.stageProgress;
        playerPersisData.willItemCount = playerData_SO.willItemCount;
        playerPersisData.manaItemCount = playerData_SO.manaItemCount;
        foreach (ChessPiece_Player c in playerData_SO.ChessPieces){
            playerPersisData.ChessPieces.Add(c);
        }
        foreach(Skill_Player s in playerData_SO.Skills){
            playerPersisData.Skills.Add(s);
        }


        string JsonData = JsonUtility.ToJson(playerPersisData,true);
        string path = GetPathFromSaveFile();
        using (FileStream stream = File.Open(path, FileMode.Create))
        {

            byte[] byteData = Encoding.UTF8.GetBytes(JsonData);

            stream.Write(byteData, 0, byteData.Length);

            stream.Close();

            Debug.Log("PlayerDataMgr: PLAYER_DATA (CACHE->PERSIS) COMPLETE \n " + path);
        }    

    }

    // 플레이어데이터가 있는지 없는지. => 보통 Init_PlayerData() 하기전에 검사용
    public static bool isPlayerDataExist(){
        if(File.Exists(GetPathFromSaveFile())){
            return true;
        }
        else{
            return false;
        }
    }
          
    public static void ShowData(){


        Debug.Log("############################## PlayerDataMgr SHOW DATA ##############################");

        // 이 부분을 수정할때는  
        // PlayerData-to-add
        // 를 검색하여 수정하세요.
        Debug.Log("*********************ChessPieces*********************");
        foreach(ChessPiece_Player c in playerData_SO.ChessPieces){
            Debug.Log("Name : " + c.name + 
            "\n Desciprtion : " + c.upgrade + 
            "\n Health : " + c.isPurchased);

        }
        Debug.Log("***************************************************************");

        // 이 부분을 수정할때는  
        // PlayerData-to-add
        // 를 검색하여 수정하세요.
        Debug.Log("*********************Skills*********************");
        foreach(Skill_Player s in playerData_SO.Skills){
            Debug.Log("Name : " + s.name + 
            "\n Desciprtion : " + s.upgrade + 
            "\n Damage : " + s.isPurchased);
        }
        Debug.Log("***************************************************************");

        Debug.Log("############################## END ##############################");


    }

    #endregion
    

    #region PRIVATE METHODS

    // Helper Function
    private static string GetPathFromSaveFile()
    {
        return Path.Combine(Application.persistentDataPath, "PlayerData.json");
    }    

    #endregion
}
