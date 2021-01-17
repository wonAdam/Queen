using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimState : MonoBehaviour
{

    // Animator에 설정된 AnimState Parameter와 같아야합니다.
    public enum ANIM_STATE{
        IDLE, // 0
        WALK, // 1
        ATTACK, // 2
        DEAD // 3
    }

    // 모든 기물들의 초기 애니메이션 state는 Idle입니다.
    public ANIM_STATE ANIMSTATE = ANIM_STATE.IDLE;
    // public ANIM_STATE ANIMSTATE{ get{ return AnimationState; } }

    private Attacker attacker;

    private Animator animator;

    private void Start() {
        ANIMSTATE = ANIM_STATE.IDLE;
        animator = GetComponent<Animator>();
        attacker = GetComponent<Attacker>();
    }

    // ATTACK->IDLE은 Animation Event로 처리
    // 나머지 State Transition은 Attcker와 Health에서 호출
    public void ChangeAnimState(int state){
        if(ANIMSTATE == (ANIM_STATE)state) // 무한 트리거 방지. 
            return;

        if((ANIM_STATE)state == ANIM_STATE.DEAD){
            attacker.enabled = false;
        }

        ANIMSTATE = (ANIM_STATE)state;

        animator.SetInteger("AnimState", state);
        animator.SetTrigger("AnimTrigger");


        if(state == (int)ANIM_STATE.DEAD)
        {
            Destroy(gameObject, 3f);
        }
        
        
    }


}
