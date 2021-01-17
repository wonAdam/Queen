using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectedState : PointerModeState
{
    LayerMask gridLayer;
    string selectedSkillCode;
    GameObject selectedSkill;
    Queen queen;
    ManaBar manaBar;
    List<Toggle> skillToggles = new List<Toggle>();
    public List<GameObject> previewSkillPrefabs;
    int cost;
    int damage;
    int skillIdx;

    public SkillSelectedState(LayerMask _gridLayer,
                                string _selectedSkillCode,
                                GameObject _selectedSkill,
                                Queen _queen,
                                ManaBar _manaBar,
                                List<Toggle> _skillToggles)
    {
        gridLayer = _gridLayer;
        selectedSkillCode = _selectedSkillCode;
        selectedSkill = _selectedSkill;
        queen = _queen;
        manaBar = _manaBar;

        // 스킬인텍스, 필요 마나량, 데미지 캐시
        skillIdx = GenericDataMgr.genericData_SO.GetSkillIdxByCode(selectedSkillCode);
        cost = GenericDataMgr.genericData_SO.Skills[skillIdx].cost;
        damage = GenericDataMgr.genericData_SO.Skills[skillIdx].damage;

        previewSkillPrefabs = new List<GameObject>();

        skillToggles.Clear();
        foreach (var t in _skillToggles)
        {
            skillToggles.Add(t);
        }

    }

    public override void Enter()
    {
        GameObject.FindObjectOfType<PointerMgr>().PointerMode = PointerMgr.Mode.SkillSelected;
    }

    public override bool Process()
    {
        // 마나가 충분한지계속 체크
        return manaBar.IsEnoughMana(cost);
    }

    public override bool Click()
    {
        // 클릭 위치에 grid가 맞으면 선택되어있는 스킬을 구사합니다.
        RaycastHit2D hit2D = Physics2D.Raycast(
            Camera.main.ScreenPointToRay(Input.mousePosition).origin,
            Camera.main.ScreenPointToRay(Input.mousePosition).direction,
            Mathf.Infinity,
            gridLayer);

        if (hit2D.collider != null)
        {
            if (hit2D.collider.GetComponent<Grid>() != null && manaBar.IsEnoughMana(cost))
            {
                OnClickGrid_UseSkill(hit2D);

                nextState = PointerMgr.Mode.None;
                return false;
            }
            // grid엔 맞았지만 마나가 부족한 경우
            else if (hit2D.collider.GetComponent<Grid>() != null && !manaBar.IsEnoughMana(cost))
            {
                return true;
            }

        }

        hit2D = Physics2D.Raycast(
            Camera.main.ScreenPointToRay(Input.mousePosition).origin,
            Camera.main.ScreenPointToRay(Input.mousePosition).direction,
            Mathf.Infinity,
            LayerMask.GetMask("Friendly"));

        // 아군 기물이 맞음
        if (hit2D.collider != null)
        {
            if (hit2D.collider.transform.GetComponent<MoveCoolTime>().moveReady)
            {
                nextState = PointerMgr.Mode.FriendlyPieceMove;
                mode_selectedPiece = hit2D.collider.gameObject;
                return false;
            }
        }

        return true;

    }

    public override void Exit()
    {
        foreach (var t in skillToggles)
        {
            t.isOn = false;
        }
    }

    public override void SetSkillPreview(List<GameObject> _previewSkillPrefabs)
    {
        previewSkillPrefabs.Clear();
        foreach (var p in _previewSkillPrefabs)
        {
            previewSkillPrefabs.Add(p);
        }
    }

    private void OnClickGrid_UseSkill(RaycastHit2D hit2D)
    {
        Grid clickedGrid = hit2D.transform.GetComponent<Grid>();
        Skill skill = selectedSkill.transform.GetComponent<Skill>();

        /* Visualize */
        // 퀸 잔상 이펙트
        queen.TriggerSkillEffect();

        /* Mana Cost */
        // cost만큼 마나를 소비합니다.
        manaBar.UseMana(cost);

        // preview가 있다면 우선 전부 파괴합니다.
        if (previewSkillPrefabs[0] != null)
        {
            foreach (var preview in previewSkillPrefabs)
            {
                if (preview != null) GameObject.Destroy(preview);
                else break;
            }
        }


        SkillVisualization(skill, clickedGrid);


        // 해당 grid를 skill에 보내 범위를 받습니다.
        List<Grid> gridsInRange = skill.GetRange(clickedGrid);
        // damage와 effect를 줄 health들을 만듭니다.
        HashSet<Health> healths;
        GetHealthToDamage(skill, gridsInRange, out healths);


        /* Damage Process */
        // damage와 skill effect를 줍니다.
        foreach (var health in healths)
        {
            health.TakeDamage(damage);
            skill.InvokeSkillEffect(health.gameObject);
            SFXMgr.Instance.SetSFXbyIndex(9 + skillIdx);
            SFXMgr.Instance.PlaySFX();
            // 효과음
        }
    }

    private void SkillVisualization(Skill skill, Grid clickedGrid)
    {
        // prefab을 소환할 Vector2를 받습니다.
        List<Vector2> positionsToInstantiate = skill.GetPrefabPosition(clickedGrid);

        // 스킬 prefab을 소환하고
        foreach (var pos in positionsToInstantiate)
        {

            GameObject go = GameObject.Instantiate(selectedSkill,
                new Vector3(pos.x, pos.y, 0f),
                Quaternion.identity);
        }
    }


    private void GetHealthToDamage(Skill skill, List<Grid> gridsInRange, out HashSet<Health> healths)
    {
        healths = new HashSet<Health>();

        foreach (var grid in gridsInRange)
        {
            // damage를 줄 health들을 담습니다.
            List<Collider2D> colliders;
            grid.GetCollidersOnThisGrid(skill.damageLayer, out colliders);
            if (colliders != null)
            {
                foreach (var collider in colliders)
                {
                    if (collider.transform.GetComponent<Health>())
                    {
                        healths.Add(collider.transform.GetComponent<Health>());
                    }

                }
            }

        }
    }

    
}
