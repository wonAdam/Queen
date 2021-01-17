using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(AnimState))]
public class Health : MonoBehaviour
{
    public enum STATUS{
        GOOD, BAD, DEAD
    }
    [Header ("Set in Editor")]
    [SerializeField] public string code;
    [SerializeField] private SpriteRenderer Good_Sprite;
    [SerializeField] private SpriteRenderer Bad_Sprite;
    [SerializeField] private bool doesHealthStateChangeSprite = true;
    [SerializeField] Image healthBar;


    [Header ("Set in Runtime")]
    private ChessPiece_Generic piece = new ChessPiece_Generic();
    public STATUS status;
    public int maxHealth;
    public int currHealth;
    private AnimState animState;
    private Color show_color = new Color(1f, 1f, 1f, 1f);
    private Color none_color = new Color(1f, 1f, 1f, 0f);

    // 아군 업글 비숍들은 다른 색깔
    private Color blue_color = new Color(0.4386792f, 0.4895903f, 1f, 1f);
    private Color red_color = new Color(0.990566f, 0.2962537f, 0.2756764f, 1f);
    private Color green_color = new Color(1f, 0.8339025f, 0.3915094f, 1f);

    private void Start() {
        //// health bar
        //if (healthBar) healthBar.gameObject.SetActive(true);
        //else
        //{
        //    healthBar = transform.Find("PieceCanvas").Find("HealthBar").GetComponent<Image>();
        //    if(healthBar) healthBar.gameObject.SetActive(true);
        //}

        animState = GetComponent<AnimState>();

        int pieceIdx = GenericDataMgr.genericData_SO.GetPieceIdxByCode(code);
        piece = GenericDataMgr.genericData_SO.ChessPieces[pieceIdx];
        int upgrade = PlayerDataMgr.playerData_SO.ChessPieces[pieceIdx].upgrade;


        // Health
        int basic_health = piece.health;
        int up_health = piece.upgrades[upgrade].health;
        maxHealth = basic_health + up_health;

        currHealth = maxHealth;
        status = STATUS.GOOD;

        if (doesHealthStateChangeSprite)
        {
            SetColor(false);
        }
    }

    public void TakeDamage(int damage){
        currHealth = Mathf.Clamp(currHealth - damage, 0, maxHealth);
        UpdateHealthBar();

        if (currHealth <= 0){
            // 죽는 로직
            status = STATUS.DEAD;
            animState.ChangeAnimState(3);
            GetComponent<Collider2D>().enabled = false;


            // grid blinking cancel
            List<Grid> gridsToCancelBlinking = new List<Grid>() ;
            switch (code)
            {
                case "P0":
                    GridMgr.PawnMoveOnGrid(GetComponent<FriendlyPieceMover>().currGrid, out gridsToCancelBlinking);
                    break;

                case "K0":
                    GridMgr.KnightMoveOnGrid(GetComponent<FriendlyPieceMover>().currGrid, out gridsToCancelBlinking);
                    break;

                case "R0":
                    GridMgr.RookMoveOnGrid(GetComponent<FriendlyPieceMover>().currGrid, out gridsToCancelBlinking);
                    break;

                default:
                    break;
            
            }

            if(gridsToCancelBlinking.Count > 0)
            {
                foreach(var grid in gridsToCancelBlinking)
                {
                    grid.isBlinking = false;
                }
            }

        }

        else if(currHealth / (float)maxHealth <= 0.3f){
            // 30% 이하일 경우 기물의 모습이 변함.
            status = STATUS.BAD;
            if (doesHealthStateChangeSprite)
            {
                SetColor(true);
            }
            // Pawn Exception
            if(code == "P0"){
                FeatherDisplayHandle();
            }        
        }
    }

    public void Heal(int amount){
        currHealth = Mathf.Clamp(currHealth + amount, 0, maxHealth);
        UpdateHealthBar();

        if (currHealth / (float)maxHealth > 0.3f && status == STATUS.BAD){
            // 30% 이상일 경우 기물의 모습이 변함.
            status = STATUS.GOOD;
            if (doesHealthStateChangeSprite)
            {
                SetColor(false);
            }
            // Pawn Exception
            if(code == "P0"){
                FeatherDisplayHandle();
            }
        }
    }

    // Animation Event <Die>
    public void DestroySelf() {
        Destroy(gameObject);
    }

    public void FeatherDisplayHandle(){
        if(status == STATUS.GOOD){
            transform.Find("Body").Find("feather").GetComponent<SpriteRenderer>().color = show_color;
        }
        else{
            transform.Find("Body").Find("feather").GetComponent<SpriteRenderer>().color = none_color;
        }
    }


    private void SetColor(bool isDamaged){
        if(isDamaged){
            if(code == "B0B"){
                Good_Sprite.color = none_color;
                Bad_Sprite.color = blue_color;
            }
            else if(code == "B0R"){
                Good_Sprite.color = none_color;
                Bad_Sprite.color = red_color;
            }
            else if(code == "B0G"){
                Good_Sprite.color = none_color;
                Bad_Sprite.color = green_color;
            }
            else{
                Good_Sprite.color = none_color;
                Bad_Sprite.color = show_color;
            }
        }
        else{
            if(code == "B0B"){
                Good_Sprite.color = blue_color;
                Bad_Sprite.color = none_color;
            }
            else if(code == "B0R"){
                Good_Sprite.color = red_color;
                Bad_Sprite.color = none_color;
            }
            else if(code == "B0G"){
                Good_Sprite.color = green_color;
                Bad_Sprite.color = none_color;
            }
            else{
                Good_Sprite.color = show_color;
                Bad_Sprite.color = none_color;
            }
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = currHealth / (float)maxHealth;
    }
}
