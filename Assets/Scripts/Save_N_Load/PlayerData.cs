using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
동적 데이터. 
플레이하면서 계속 바뀌어야할 데이터입니다.
예를들면, 플레이어의 골드, 스테이지 클리어상태, 
플레이어 보유 스킬, 기물 업그레이드 정도 등등
*/


[System.Serializable]
public class PlayerData
{
    // 이 부분을 수정할때는  
    // PlayerData-to-add
    // 를 검색하여 수정하세요.
    public int stageProgress;
    public int gold;
    public int willItemCount;
    public int manaItemCount;
    public List<ChessPiece_Player> ChessPieces = new List<ChessPiece_Player>();
    public List<Skill_Player> Skills = new List<Skill_Player>();

    public PlayerData()
    {
        // to-do
        // 첫 플레이시 어떻게 초기화할건지
        stageProgress = 0;
        gold = 0;
        willItemCount = 0;
        manaItemCount = 0;
    }


}

[System.Serializable]
public class ChessPiece_Player
{
    public string code;
    public string name;
    public int upgrade;
    public bool isPurchased;

    public ChessPiece_Player(string _code, string _name, int _upgrade, bool _isPurchased)
    {

        // to-do
        // 첫 플레이시 어떻게 초기화할건지

        code = _code;
        name = _name;
        upgrade = _upgrade;
        isPurchased = _isPurchased;
    }

}

[System.Serializable]
public class Skill_Player
{
    public string code;
    public string name;
    public int upgrade;
    public bool isPurchased;

    public Skill_Player(string _code, string _name, int _upgrade, bool _isPurchased)
    {

        // to-do
        // 첫 플레이시 어떻게 초기화할건지
        code = _code;
        name = _name;
        upgrade = _upgrade;
        isPurchased = _isPurchased;
    }

}

