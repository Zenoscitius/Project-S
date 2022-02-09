using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class to implement behavior for all fightable/damageable units [this includes MC]
public class Combatant : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    protected int currentHealth;
    protected UIHealthBar UIHealthBar;
    public Image healthBarMask;    //TODO: visual healthbar

    public float timeInvincible = 2.0f;
    protected bool isInvincible;
    protected float invincibleTimer;

    protected Rigidbody2D rigidbody2d;
    protected Vector2 move = new Vector2(0, 0);
    protected Vector2 lookDirection = new Vector2(1, 0);

    protected Animator animator;
    protected AudioSource audioSource;

    public AudioClip damagedAudio;
    public ParticleSystem damagedEffect;

    public AudioClip healedAudio;
    public ParticleSystem healedEffect;

    // Start is called before the first frame update; does not run in child class instances without direct call
    protected virtual void Start()
    {
        //Debug.Log("Comb Start fxn");
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;

        UIHealthBar = gameObject.AddComponent<UIHealthBar>() as UIHealthBar;  new UIHealthBar(healthBarMask, false);
    }

    //https://learn.unity.com/tutorial/coroutines?uv=2019.3&projectId=5c88f2c1edbc2a001f873ea5
    //coroutines-->more efficient updating when usable

    // Update is called once per frame
    protected virtual void Update()
    {
        //check iframes
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
    }

    //updates not based on current framerate
    protected virtual void FixedUpdate()
    {
        //update their position
        //Debug.Log("What is rigidbody2d here", rigidbody2d);
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * move.x * Time.fixedDeltaTime;
        position.y = position.y + speed * move.y * Time.fixedDeltaTime;

        rigidbody2d.MovePosition(position);
    }

    //play targeted sound
    //https://gamedevbeginner.com/10-unity-audio-tips-that-you-wont-find-in-the-tutorials/
    public void PlaySound(AudioClip clip)
    {
        //PlayOneShot can also take a float [0,1] for volume of just this clip
        if (clip != null) audioSource.PlayOneShot(clip);
    }

    //play targeted animation
    public void PlayAnimation(string animationName)
    {
        this.animator.SetTrigger(animationName);
    }

    //public void PlayParticles(ParticleSystem particles)
    //{
    //    particles.Play();
    //}


    //update health 
    public virtual void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible) return; //no dmg

            //take damage and gain iFrames
            isInvincible = true;
            invincibleTimer = timeInvincible;

            PlayAnimation("Hit");//hit animation
            damagedEffect.Play();//play visualfx
            PlaySound(damagedAudio);//damaged sound
        }
        else
        {
            healedEffect.Play();//play visualfx
            PlaySound(healedAudio);//healed sound
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.SetVisibility(currentHealth != maxHealth);
        UIHealthBar.SetValue(currentHealth / (float)maxHealth);
    }
}
