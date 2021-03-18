using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCController : Combatant
{
    //COMMENTED MEMEBERS comes from parent class
    //public float speed = 3.0f;

    //public int maxHealth = 5;

    //public int health { get { return currentHealth; } }
    //protected int currentHealth;

    //public float timeInvincible = 2.0f;
    //protected bool isInvincible;
    //protected float invincibleTimer;

    //protected Rigidbody2D rigidbody2d;
    //protected float horizontal;
    //protected float vertical;

    //protected Animator animator;
    //protected Vector2 lookDirection = new Vector2(1, 0);

    //protected AudioSource audioSource;
    //public AudioClip damagedAudio;

    public GameObject projectilePrefab;
    public ParticleSystem heroDamagedEffect;
    public ParticleSystem heroHealedEffect;
    public AudioClip cogThrowAudio;

    //https://learn.unity.com/tutorial/main-character-and-first-script?uv=2020.1&projectId=5c6166dbedbc2a0021b1bc7c#5cda962cedbc2a08f692f813

    protected override void Start()
    {
        //Debug.Log("MC Start fxn");
        base.Start();
        //animator = GetComponent<Animator>();

        //currentHealth = maxHealth;
        //rigidbody2d = GetComponent<Rigidbody2D>();
        //audioSource = GetComponent<AudioSource>();
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;
    }

    // Update is called once per frame
    protected override void Update()
    {

        //base.Update();
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);




        if (false)//(Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        //Debug.Log(horizontal);
        //Debug.Log(vertical);

        // Vector2 position = transform.position;
        // position.y = position.y + 3f * vertical * Time.deltaTime;
        // position.x = position.x + 3f * horizontal * Time.deltaTime;
        // transform.position = position;

        // If you want to make it work across devices, you can use Input.GetButtonDown with an axis name, like you did for movement, and define which button that axis corresponds to in the Input settings (Edit > Project Settings > Input). For an example, take a look at the Axes > Fire1.
        if (Input.GetKeyDown(KeyCode.C))
        //if (Input.GetButtonDown('Fire1'))
        {
            Launch();
        }
    }


    public override void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
            heroDamagedEffect.Play();
            PlaySound(damagedAudio);
        }
        else
        {
            heroHealedEffect.Play();
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        PlaySound(cogThrowAudio);
    }

}
