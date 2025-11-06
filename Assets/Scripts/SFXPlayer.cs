using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public static SFXPlayer instance;

    /*public AudioSource audioSourceSFX;
    public AudioSource audioSourceMusic;
    public AudioSource audioSourceSFX_Unscaled;

    [Header("Skills")]
    public AudioClip lightningSkill;
    public AudioClip swordSkill;
    public AudioClip healSkill;

    [Header("Player")]
    public AudioClip playerWalk;
    public AudioClip playerTakeDamage;
    public AudioClip spear;
    public AudioClip spearDamage;
    public AudioClip playerDeath;

    [Header("Enemy")]
    public AudioClip zombieBite;
    public AudioClip spiderBite;
    public AudioClip zombieDeath;
    public AudioClip spiderDeath;

    [Header("SFX")]
    public AudioClip backgroundMusic;
    public AudioClip buttonClick;
    public AudioClip chestOpening;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public void PlaySoundUnscaled(AudioClip clip)
    {
        if (clip != null)
        {
            audioSourceSFX_Unscaled.ignoreListenerPause = true;
            audioSourceSFX_Unscaled.PlayOneShot(clip);
        }
    }*/
}