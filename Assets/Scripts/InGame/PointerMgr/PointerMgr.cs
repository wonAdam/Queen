using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointerMgr : MonoBehaviour
{
    public enum Mode
    {
        SkillSelected, PieceSelected, BishopSelected, FriendlyPieceMove, None
    }

    [Header ("Set in Editor")]
    [SerializeField] LayerMask gridLayer;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] LayerMask UILayer;


    [Header("Set in Runtime")]
    //[HideInInspector]
    //[SerializeField] public bool isPieceSelected = false;
    //[SerializeField] public bool isSkillSelected = false;
    //[SerializeField] public bool isBishopSelected = false;

    public FBishopUI openedFBishopUI = null;
    
    public Mode PointerMode = Mode.None;
    public PointerModeState pointerModeState;
    public List<Toggle> pieceToggles;
    public List<Toggle> skillToggles;

    public GameObject selectedPiece;
    public GameObject selectedSkill;
    public GameObject selectedSkillPreview;
    public GameObject selectedMovingPiece;
    public string selectedPieceCode;
    public string selectedSkillCode;
    public bool isMouseDown = false;
    public bool isDragged = false;
    private Grid lastClickedGrid = null;
    public GameObject[] previewSkillPrefabs;
    public WillBar willBar; // Use를 호출하기위함
    public ManaBar manaBar; // Use를 호출하기위함
    private Queen queen; // Skill사용 애니메이션을 트리거하기 위함

    private void Start()
    {
        willBar = FindObjectOfType<WillBar>();
        manaBar = FindObjectOfType<ManaBar>();
        queen = FindObjectOfType<Queen>();
        previewSkillPrefabs = new GameObject[FindObjectsOfType<Grid>().Length];

        // piece selection toggle 들을 초기화
        PieceSelectionToggle[] tmpArr1 = FindObjectsOfType<PieceSelectionToggle>();
        foreach (PieceSelectionToggle t in tmpArr1)
        {
            pieceToggles.Add(t.GetComponent<Toggle>());
        }

        // skill selection toggle 들을 초기화
        SkillSelectionToggle[] tmpArr2 = FindObjectsOfType<SkillSelectionToggle>();
        foreach (SkillSelectionToggle t in tmpArr2)
        {
            skillToggles.Add(t.GetComponent<Toggle>());
        }

        pointerModeState = new NoneState();
        pointerModeState.Enter();
    }
    

    private void Update()
    {

        // 입력
        // 드래그로 카메라 이동과 일반 클릭을 구분하기 위함입니다.
        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
            isDragged = false;
        }
        if (isDragged && PointerMode != Mode.SkillSelected)
        {
            isMouseDown = false;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isMouseDown)
            {
                MouseClicked();
                isMouseDown = false;
            }
            isDragged = false;
        }


        // 누르는 상태이고 스킬사용모드라면 스킬의 preview를 grid에 snap하여 보여줍니다.
        ProcessSkillPreview();

        // 현재 pointerModeState를 빠져가야 할 상황이 오면
        if (!pointerModeState.Process())
        {
            // NoneState로 전환
            StateChangeTo(new NoneState());
            return;
        }
    }

    

    

    // 일반 클릭일때 호출되는 함수입니다.
    // PointerMode에 따라 다르게 기능합니다.
    private void MouseClicked() {

        if(!pointerModeState.Click()) // 현재모드의 모드를 바꿔야하는 클릭위치
        {
            if (pointerModeState.nextState == Mode.BishopSelected)
            {
                // 선택된 아군비숍
                GameObject mode_selectedPiece = pointerModeState.mode_selectedPiece;
                StateChangeTo(new BishopSelectedState(mode_selectedPiece));
                return;
            }
            else if (pointerModeState.nextState == Mode.FriendlyPieceMove)
            {
                // 선택된 아군기물
                GameObject mode_selectedPiece = pointerModeState.mode_selectedPiece;
                StateChangeTo(new FriendlyPieceMoveState(mode_selectedPiece, gridLayer));
                return;
            }
            else if(pointerModeState.nextState == Mode.PieceSelected)
            {
                // 선택된 아군기물
                GameObject mode_selectedPiece = pointerModeState.mode_selectedPiece;
                string mode_selectedCode = pointerModeState.mode_selectedCode;

                StateChangeTo(new PieceSelectedState(gridLayer, UILayer, willBar, pieceToggles, mode_selectedPiece, mode_selectedCode));
                return;
            }
            else
            {
                StateChangeTo(new NoneState());
                return;
            }

        }

        
    }



    // 누르는 상태이고 스킬사용모드라면 스킬의 preview를 grid에 snap하여 보여줍니다.
    private void ProcessSkillPreview()
    {
        if (isMouseDown &&
            PointerMode == Mode.SkillSelected)
        {
            // 이미 preview하던게 있고 pointer의 위치가 바뀐거라면
            if (lastClickedGrid != GetGridUnderPointer() && previewSkillPrefabs[0] != null)
            {
                // 하던 preview는 파괴하고
                foreach (var prefab in previewSkillPrefabs)
                {
                    if (prefab) Destroy(prefab);
                    else break;
                }
                lastClickedGrid = GetGridUnderPointer();
                ShowSkillPreview();
            }
            // pointer의 위치는 그대로고 preview하던게 사라진거면
            else if (lastClickedGrid == GetGridUnderPointer() && previewSkillPrefabs[0] == null)
            {
                ShowSkillPreview();
            }
            // pointer의 위치는 다르고 preview하던게 사라짐.
            else if (previewSkillPrefabs[0] == null)
            {
                lastClickedGrid = GetGridUnderPointer();
                ShowSkillPreview();
            }

        }
    }

    private void ShowSkillPreview()
    {
        // 마우스위치에 해당하는 grid를 확인
        Grid gridUnderPointer = GetGridUnderPointer();

        // grid에 맞았으면
        if (gridUnderPointer != null)
        {
            // 해당 grid를 skill에 보내 범위를 받습니다.
            List<Vector2> positionsToInstantiate = selectedSkill.GetComponent<Skill>().GetPrefabPosition(gridUnderPointer);

            // 범위에 스킬prefab을 배치합니다
            List<GameObject> previews = new List<GameObject>();
            for (int i = 0; i < positionsToInstantiate.Count; i++)
            {
                GameObject go = Instantiate(selectedSkillPreview,
                                new Vector3(positionsToInstantiate[i].x, positionsToInstantiate[i].y, 0f),
                                Quaternion.identity);
                previewSkillPrefabs[i] = go;
                previews.Add(go);
            }
            pointerModeState.SetSkillPreview(previews);
        }

    }

    // Called by this.ProcessSkillPreview & this.ShowSkillPreview
    private Grid GetGridUnderPointer()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(
                Camera.main.ScreenPointToRay(Input.mousePosition).origin,
                Camera.main.ScreenPointToRay(Input.mousePosition).direction,
                Mathf.Infinity,
                gridLayer);

        if (hit2D.collider != null && hit2D.collider.GetComponent<Grid>() != null)
        {
            return hit2D.collider.GetComponent<Grid>();
        }
        else
        {
            return null;
        }
    }


    // PieceSelectionToggle에 의해 호출됩니다. 
    public void OnValueChange_PieceSelectionToggle(bool isOn, GameObject piece, string code)
    {
        if (isOn)
        {
            StateChangeTo(new PieceSelectedState(gridLayer, UILayer, willBar, pieceToggles, piece, code));
        }
        else
        {
            StateChangeTo(new NoneState());
        }

    }

    // SkillSelectionToggle에 의해 호출됩니다. 
    public void OnValueChange_SkillSelectionToggle(bool isOn, GameObject skill, GameObject preview, string code)
    {
        if (isOn)
        {
            StateChangeTo(new SkillSelectedState(gridLayer, code, skill, queen, manaBar, skillToggles));

            selectedSkill = skill;
            selectedSkillCode = code;
            selectedSkillPreview = preview;
        }
        else
        {
            StateChangeTo(new NoneState());
        }
    }

    private void StateChangeTo(PointerModeState nextState)
    {
        pointerModeState.Exit();
        pointerModeState = nextState;
        nextState.Enter();
    }

}
