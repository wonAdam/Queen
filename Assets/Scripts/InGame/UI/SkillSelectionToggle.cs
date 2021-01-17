using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectionToggle : MonoBehaviour
{
    [Header("Set in Editor")]
    [SerializeField] Image background_selected;
    [SerializeField] Image background_unselected;


    [Header("Set in Runtime")]
    /*
     code
     */
    // 출시단계에서는 선택한 스킬들에 따라 런타임에서 세팅해줄거임
    // 개발단계에서는 직접세팅하며 디버깅
    public string code;
    public GameObject skill;
    public GameObject preview;
    public Skill_Generic Skill_Data = null;
    private PointerMgr pointerMgr;
    private ManaBar manaBar;
    private Toggle toggle;
    private bool hasSkill = false;
    private bool preState = false;
    private bool curState = false; // true : will 충분, false : will 부족


    private void Start() {
        pointerMgr = FindObjectOfType<PointerMgr>();
        manaBar = FindObjectOfType<ManaBar>();
        toggle = GetComponent<Toggle>();

        if (code != "")
        {
            int idx = GenericDataMgr.genericData_SO.GetSkillIdxByCode(code);
            if(idx != -1) // 해당 코드를 가진 스킬이 있음
            {
                Skill_Data = GenericDataMgr.genericData_SO.Skills[idx];
                skill = Skill_Data.prefab;
                preview = Skill_Data.previewPrefab;
                background_selected.sprite = Skill_Data.thumbnail_noMana;
                background_unselected.sprite = Skill_Data.thumbnail_noMana;
                hasSkill = true;
            }
        }
    }

    private void Update()
    {
        if (!hasSkill) return;

        preState = curState;
        curState = manaBar.IsEnoughMana(Skill_Data.cost);
        if (preState != curState)
        {
            if (curState)
            {
                //마나가 충분해지면 
                background_selected.sprite = Skill_Data.thumbnail_yesMana;
                background_unselected.sprite = Skill_Data.thumbnail_yesMana;
            }
            else
            {
                //마나가 불충분해지면
                background_selected.sprite = Skill_Data.thumbnail_noMana;
                background_unselected.sprite = Skill_Data.thumbnail_noMana;
                toggle.isOn = false;
            }
        }
    }

    public void OnValueChange_SkillSelectionToggle(Toggle change){
        if (!hasSkill)
        {
            change.isOn = false;
            return;
        }

        if (change.isOn)
        {
            if (!curState)
            {
                change.isOn = false;
            }
        }

        pointerMgr.OnValueChange_SkillSelectionToggle(change.isOn, skill, preview, code);
    }
}
