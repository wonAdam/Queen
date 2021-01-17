using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName="GenericData_SO", fileName="GenericData_SO", order=0)]
public class GenericData_SO : ScriptableObject
{
    // 이 부분을 수정할때는  
    // GenericData-to-add
    // 를 검색하여 수정하세요.
    public Will will;
    public Mana mana;

    public List<ChessPiece_Generic> ChessPieces = new List<ChessPiece_Generic>();
    public List<Skill_Generic> Skills = new List<Skill_Generic>();


    // 문자열 코드가 들어오면 코드에 맞는 기물를 리턴합니다.
    // 못찾았으면 null을 리턴합니다.
    public int GetPieceIdxByCode(string code){

        for(int i = 0 ; i < ChessPieces.Count; i++){
            if(ChessPieces[i].code == code){
                return i;
            }
        }

        return -1;
    }
    public int GetSkillIdxByCode(string code){

        for(int i = 0 ; i < Skills.Count; i++){
            if(Skills[i].code == code){
                return i;
            }
        }

        return -1;
    }
}
