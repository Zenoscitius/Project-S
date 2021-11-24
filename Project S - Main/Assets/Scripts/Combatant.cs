﻿using System.Collections;
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
    }

    //https://learn.unity.com/tutorial/coroutines?uv=2019.3&projectId=5c88f2c1edbc2a001f873ea5
    //coroutines-->more efficient updating when usable

    // Update is called once per frame
    protected virtual void Update()
    {

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
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayParticles(ParticleSystem particles)
    {

    }

    //update health 
    public virtual void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible) return; //no dmg

            //take damage and gain iFrames
            isInvincible = true;
            invincibleTimer = timeInvincible;

            animator.SetTrigger("Hit");//hit animation
            damagedEffect.Play();//play visualfx
            PlaySound(damagedAudio);//damaged sound
        }
        else
        {
            healedEffect.Play();//play visualfx
            PlaySound(healedAudio);//healed sound
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }


}
