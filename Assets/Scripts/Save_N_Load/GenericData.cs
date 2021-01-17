using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
정적 데이터. 
개발이 끝나고 나면 플레이중에는 안바뀔 데이터입니다.
예를들면, 특정 기물의 업그레이드 안했을때 이름, 체력, 코스트, 데미지 등등.
특정 스킬의 기본 데미지, 코스트 등등
특정 스테이지의 노멀웨이브시간, 빅웨이브시간, 적군스폰디테일
*/



[System.Serializable]
public class GenericData
{
    // 이 부분을 수정할때는  
    // GenericData-to-add
    // 를 검색하여 수정하세요.

    public Will will = new Will();
    public Mana mana = new Mana();
    public List<ChessPiece_Generic> ChessPieces = new List<ChessPiece_Generic>();
    public List<Skill_Generic> Skills = new List<Skill_Generic>();

}


[System.Serializable]
public class ChessPiece_Generic{
    // 이 부분을 수정할때는  
    // GenericData-to-add
    // 를 검색하여 수정하세요.
    public GameObject prefab;
    public string code;
    public string name_kor;
    public string name_eng;
    public string description;
    public int health;
    public int damage;
    public float speed;
    public float attackCoolTime;
    public float skillEffectiveness;
    public int range;
    public float placingCoolTime;
    public int cost;
    public float moveCoolTime;

    // 적군일경우 처치시 골드드랍 min/max
    // 아군일 경우 max를 0으로 설정하세요.
    public int goldMin;
    public int goldMax;
    public List<Chess_Upgrade> upgrades = new List<Chess_Upgrade>();

    public ChessPiece_Generic(){
        name_kor = "";
        name_eng = "";
        health = 0;
        damage = 0;
        speed = 0f;
        attackCoolTime = 0f;
        skillEffectiveness = 0f;
        range = 0;
        placingCoolTime = 0f;
        cost = 0;
        goldMin = 0;
        goldMax = 0;
        moveCoolTime = 0f;
        upgrades.Add(new Chess_Upgrade());
        upgrades.Add(new Chess_Upgrade());
        upgrades.Add(new Chess_Upgrade());
        upgrades.Add(new Chess_Upgrade());
        upgrades.Add(new Chess_Upgrade());
        upgrades.Add(new Chess_Upgrade());
    }
}

[System.Serializable]
public class Chess_Upgrade{
    public int cost;
    public int health;
    public int damage;
    public float skillEffectiveness;
}

[System.Serializable]
public class Skill_Generic{
    // 이 부분을 수정할때는  
    // GenericData-to-add
    // 를 검색하여 수정하세요.
    public string code;
    public string name_kor;
    public string name_eng;
    public string description;
    public int damage;
    public float coolTime;
    public float duration;
    public int cost;
    public GameObject prefab;
    public GameObject previewPrefab;
    public Sprite thumbnail_noMana;
    public Sprite thumbnail_yesMana;
    public List<Skill_Upgrade> upgrades = new List<Skill_Upgrade>();

    public Skill_Generic(){
        name_kor = "";
        name_eng = "";
        description = "";
        damage = 0;
        coolTime = 0f;
        duration = 0f;
        cost = 0;
        upgrades.Add(new Skill_Upgrade());
        upgrades.Add(new Skill_Upgrade());
        upgrades.Add(new Skill_Upgrade());
        upgrades.Add(new Skill_Upgrade());
        upgrades.Add(new Skill_Upgrade());
    }
}

[System.Serializable]
public class Skill_Upgrade{
    public int cost;
    public int damage;
    public float coolTime;
    public float duration;
}


[System.Serializable]
public class Will{
    public List<Will_Upgrade> upgrades = new List<Will_Upgrade>();

}

[System.Serializable]
public class Will_Upgrade{
    public int cost;
    public float regenPerSec;
}

[System.Serializable]
public class Mana{
    public List<Mana_Upgrade> upgrades = new List<Mana_Upgrade>();

}

[System.Serializable]
public class Mana_Upgrade{
    public int cost;
    public float regenPerSec;
}
