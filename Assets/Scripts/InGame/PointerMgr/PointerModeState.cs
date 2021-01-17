using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PointerModeState
{
    public PointerMgr.Mode nextState;
    public GameObject mode_selectedPiece;
    public string mode_selectedCode;

    /// <summary>
    /// 처음 State를 입장하였을 때 호출하세요.
    /// </summary>
    public abstract void Enter();
    /// <summary>
    /// false를 리턴하면 Mode를 바꿔야합니다. 
    /// true를 리턴하면 클릭이 있을 때까지 계속 Process를 호출합니다.
    /// </summary>
    /// <returns></returns>
    public abstract bool Process();
    /// <summary>
    /// 클릭이 있을 시 해당 Mode의 Click()함수를 호출하세요.
    /// </summary>
    /// <returns></returns>
    public abstract bool Click();
    /// <summary>
    /// Mode를 빠져나와 다른Mode로 전환해야할 시 Exit을 호출하고 다음 Mode의 Enter를 호출하세요.
    /// </summary>
    public abstract void Exit();

    public virtual void SetSkillPreview(List<GameObject> _previewSkillPrefabs)
    {

    }
}
