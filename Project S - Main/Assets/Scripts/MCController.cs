using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public PlayerInput playerInput;

    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;

    private Vector2 move = new Vector2(0,0);


    //Current Control Scheme
    private string currentControlScheme;

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
        //horizontal = Input.GetAxis("Horizontal");
        //vertical = Input.GetAxis("Vertical");

        //get our look vector
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        Vector2 position = transform.position;
        position.y = position.y + 3f * move.y * Time.deltaTime;
        position.x = position.x + 3f * move.x * Time.deltaTime;
        transform.position = position;

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
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
        //PlaySound(cogThrowAudio);
    }




    //INPUT SYSTEM ACTION METHODS --------------

    //This is called from PlayerInput; when a joystick or arrow keys has been pushed.
    //It stores the input Vector as a Vector3 to then be used by the smoothing function.


    public void OnMovement(InputAction.CallbackContext value)
    {
        //Debug.Log("Attempting to move");
        Vector2 inputMovement = value.ReadValue<Vector2>();
        move.x = inputMovement.x;
        move.y = inputMovement.y; //rawInputMovement

    }

    //This is called from PlayerInput, when a button has been pushed, that corresponds with the 'Light Attack' action
    public void OnLightAttack(InputAction.CallbackContext value)
    {
        
        if (value.started)
        {
            Debug.Log("Light Attack!");
            //playerAnimationBehaviour.PlayAttackAnimation();
        }
        //Launch();
    }


    //This is called from PlayerInput, when a button has been pushed, that corresponds with the 'Light Attack' action
    public void OnHeavyAttack(InputAction.CallbackContext value)
    {
        
        if (value.started)
        {
            Debug.Log("Heavy Attack!");
            //playerAnimationBehaviour.PlayAttackAnimation();
        }
        //Launch();
    }

    //This is called from PlayerInput, when a button has been pushed, that corresponds with the 'Ranged Attack' action
    public void OnRangedAttack(InputAction.CallbackContext value)
    {
        if (value.started) { //only do it when we initially press the button 
            Debug.Log("Ranged Attack!");
            Launch();
        }
    }

    //This is called from PlayerInput, when a button has been pushed, that corresponds with the 'Escape Menu' action-->moved to escapemenuscipts
    //public void OnOpenEscapeMenu(InputAction.CallbackContext value)
    //{
    //    if (value.started)
    //    { //only do it when we initially press the button 
    //        //PauseGame();
    //        Debug.Log("Open the Escape menu!");
    //    }
    //}


    //This is called from Player Input, when a button has been pushed, that correspons with the 'TogglePause' action
    public void OnInteract(InputAction.CallbackContext value)
    {
        //check if we are talking to someone
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


    //This is called from Player Input, when a button has been pushed, that correspons with the 'TogglePause' action
    public void OnTogglePause(InputAction.CallbackContext value)
    {
        //if (value.started)
        //{
        //    GameManager.Instance.TogglePauseState(this);
        //}
    }



}
