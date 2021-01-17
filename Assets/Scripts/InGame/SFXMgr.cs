using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXMgr : MonoBehaviour
{

    private static SFXMgr instance;
    [SerializeField] AudioClip[] FriendlyAttackSound;
    [SerializeField] AudioClip[] EnemyAttackSound;
    [SerializeField] AudioClip PieceAppearingSound;
    [SerializeField] AudioClip[] SkillSound;
    [SerializeField] AudioClip BuyItemSound;
    [SerializeField] AudioClip[] HitSound;
    [SerializeField] AudioClip StageClearSound;
    [SerializeField] AudioClip StageFailedSound;
    AudioSource sound;

    public static SFXMgr Instance
    {
        get
        {
            return instance;
        }
        set
        {
            Instance = value;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        instance = this;
        sound = GetComponent<AudioSource>();

        DontDestroyOnLoad(gameObject);
    }

    public void SetVolume(float v)
    {
        sound.volume = v * 0.8f;
    }

    public void PlaySFX()
    {
        sound.Play();
    }

    public void StopSFX()
    {
        sound.Stop();
    }
    public void SetSFXbyIndex(int idx)
    {
        switch (idx)
        {
            case 0:
                sound.clip = FriendlyAttackSound[0]; //pawn
                break;
            case 1:
                sound.clip = FriendlyAttackSound[1]; //rook
                break;
            case 2:
                sound.clip = FriendlyAttackSound[2]; //fknight
                break;
            case 3:
                sound.clip = FriendlyAttackSound[3]; //fbishop
                break;
            case 4:
                sound.clip = EnemyAttackSound[0]; //zergling
                break;
            case 5:
                sound.clip = EnemyAttackSound[1]; //orc
                break;
            case 6:
                sound.clip = EnemyAttackSound[2]; //eknight
                break;
            case 7:
                sound.clip = EnemyAttackSound[3]; //ebishop
                break;
            case 8:
                sound.clip = PieceAppearingSound;
                break;
            case 9:
                sound.clip = SkillSound[0]; //fire
                break;
            case 10:
                sound.clip = SkillSound[1]; //ice
                break;
            case 11:
                sound.clip = SkillSound[2]; //thunder
                break;
            case 12:
                sound.clip = BuyItemSound;
                break;
            case 13:
                sound.clip = HitSound[0];
                break;
            case 14:
                sound.clip = HitSound[1];
                break;
            case 15:
                sound.clip = HitSound[2];
                break;
            case 16:
                sound.clip = StageClearSound;
                break;
            case 17:
                sound.clip = StageFailedSound;
                break;
            default:
                break;
        }
    }
}
