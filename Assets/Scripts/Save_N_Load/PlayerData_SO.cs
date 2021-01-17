using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerData_SO", fileName = "PlayerData_SO", order = 1)]
public class PlayerData_SO : ScriptableObject
{

    // 이 부분을 수정할때는  
    // PlayerData-to-add
    // 를 검색하여 수정하세요.
    public int stageProgress;
    public int gold;
    public int willItemCount = 0;
    public int manaItemCount = 0;
    public List<ChessPiece_Player> ChessPieces = new List<ChessPiece_Player>();
    public List<Skill_Player> Skills = new List<Skill_Player>();


    public int GetPieceIdxByCode(string code)
    {

        for (int i = 0; i < ChessPieces.Count; i++)
        {
            if (ChessPieces[i].code == code)
            {
                return i;
            }
        }

        return -1;
    }

    public int GetSkillIdxByCode(string code)
    {

        for (int i = 0; i < Skills.Count; i++)
        {
            if (Skills[i].code == code)
            {
                return i;
            }
        }

        return -1;
    }


}

