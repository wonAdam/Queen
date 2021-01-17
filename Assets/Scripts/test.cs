using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//debug
public class test : MonoBehaviour
{ 
    // PlayerData를 불러오고 저장하는 예시입니다.
    public void PlayerData_SaveNLoadExample(){
        // PlayerData_SO를 바꾼후
        PlayerDataMgr.playerData_SO.ChessPieces[0].name = "원동현";

        // 꼭 Sync를 해주세요!
        PlayerDataMgr.Sync_Cache_To_Persis();
    }

    // GenericData를 불러오는 예시입니다.
    // GenericData는 기획적으로만 바꿀수 있기때문에 Cache의 값을 바꾸지마세요! Get만 하세요!
    public void GenericData_LoadExample(){
        Debug.Log(GenericDataMgr.genericData_SO.ChessPieces[0].name_kor);
    }

    // 개발중에만 사용할 함수입니다.
    public void GenericData_C_To_P(){
        GenericDataMgr.Sync_Cache_To_Persis();
    }

    // 개발중에만 사용할 함수입니다.
    public void GenericData_P_To_C(){
        GenericDataMgr.Sync_Persis_To_Cache();
    }

    // 개발중에만 사용할 함수입니다.
    public void Init_PlayerData(){
        PlayerDataMgr.Init_PlayerData();
    }

    // 개발중에만 사용할 함수입니다.
    public void PlayerData_C_To_P(){
        PlayerDataMgr.Sync_Cache_To_Persis();
    }
    // 개발중에만 사용할 함수입니다.
    public void PlayerData_P_To_C(){
        PlayerDataMgr.Sync_Persis_To_Cache();
    }

    public void MakeStageData(){
        StageDataMgr.MakeSingleEmptyStageData();
    }

}
