using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to implement behavior for all fightable/damageable units [this includes MC]
public class Combatant : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 5;

    public int health { get { return currentHealth; } }
    protected int currentHealth;

    public float timeInvincible = 2.0f;
    protected bool isInvincible;
    protected float invincibleTimer;

    protected Rigidbody2D rigidbody2d;
    protected float horizontal;
    protected float vertical;

    protected Animator animator;
    protected Vector2 lookDirection = new Vector2(1, 0);

    protected AudioSource audioSource;
    public AudioClip damagedAudio;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //updates not based on current framerate
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }


    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }


    public virtual void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
 
            PlaySound(damagedAudio);
        }
        else
        {

        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }


}
