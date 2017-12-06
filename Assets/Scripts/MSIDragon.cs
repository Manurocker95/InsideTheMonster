using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSIDragon : MonoBehaviour
{
    private static MSIDragon instance;
    public static MSIDragon Instance { get { return instance; } }
    
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float jumpSpeed = 10;
    [SerializeField] private float holdJumpFallSpeed = 2.5f;
    [SerializeField] private float fallSpeed = 7;
    [SerializeField] private float groundedDistance = 0.78f;
    [SerializeField] private float maxJumpTime = 0.7f;
    [SerializeField] private float flySpeed = 10;
    [SerializeField] private TextAsset tutorialText;
    [SerializeField] private GameObject darkEffect;
    [SerializeField] private MSILightEffectScript lightEffect;
    [SerializeField] private MSILadder ladderRef;
    public bool debug = true;


    Rigidbody2D body;
    Animator animController;
    SpriteRenderer spriteRender;
    float currentJumpTime;
    bool isJumping = false;
    bool canJump = false;
    bool dontKeepJumping = false;
    bool canMove = true;
    bool inContactWithLadder = false;
    bool onLadder = false;
    float flyTimeLeft = 0;
    float waitingTime = 0;
    MSIParseXML tutorials;
    Collider2D thisCollider;

    public bool isInContactWithLadder { get { return inContactWithLadder; } }
    public bool isOnLadder { get { return onLadder; } }
    // Use this for initialization
    void Start ()
    {
        instance = this;

        body = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();
        animController = GetComponent<Animator>();
        thisCollider = GetComponent<Collider2D>();
        tutorials = new MSIParseXML(tutorialText);
        if (!debug)
        {
            darkEffect.SetActive(true);
            lightEffect.gameObject.SetActive(false);
            canMove = false;
            Invoke("ShowFirstDialog", 2f);
        }
        else
        {
            darkEffect.SetActive(false);
            lightEffect.gameObject.SetActive(false);
            canMove = true;
        }
    }

    void ShowFirstDialog()
    {
        MSIDialogManager.Instance.AddInitialSubtitles(tutorials.getDescriptionByTriggerName("First_oscuro"), tutorials.getDescriptionByTriggerName("Second_oscuro"));
    }

    public void StartGame()
    {
        canMove = true;
    }

    public void animateLightEffect()
    {
        darkEffect.SetActive(false);
        lightEffect.gameObject.SetActive(true);
        lightEffect.animateInitialEffect();
    }

    #region Update

    // Update is called once per frame
    void Update ()
    {
        if (waitingTime > 0)
            waitingTime -= Time.deltaTime;
         
        float moveX = 0;

        if(canMove)
        {
            if(Input.GetKey(KeyCode.A))
            {
                moveX = -movementSpeed;
            }
            else if(Input.GetKey(KeyCode.D))
            {
                moveX = movementSpeed;
            }
        }

        float jumpMovement = doJump();

        if(moveX > 0)
        {
            spriteRender.flipX = true;
        }
        else if(moveX < 0)
        {
            spriteRender.flipX = false;
        }

        if(flyTimeLeft > 0)
        {
            canJump = false;
            jumpMovement = flySpeed;
            flyTimeLeft -= Time.deltaTime;
        }
        else if(!thisCollider.enabled)
        {
            thisCollider.enabled = true;
        }

        if(!canJump)
        {
            animController.SetBool("Jump", true);
            animController.SetBool("Idle", false);
            animController.SetBool("Walk", false);
        }
        else if (moveX != 0)
        {
            animController.SetBool("Walk", true);
            animController.SetBool("Idle", false);
            animController.SetBool("Jump", false);

        }
        else
        {
            animController.SetBool("Idle", true);
            animController.SetBool("Jump", false);
            animController.SetBool("Walk", false);
        }

        body.velocity = new Vector2(moveX, jumpMovement);
	}

    #endregion

    #region jump
    float doJump()
    {
        float jumpMove = 0;
        bool jumpPressed = false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);

        if (hit.distance <= groundedDistance)
        {
            canJump = true;
            currentJumpTime = 0;
            Debug.Log(hit.collider.name);
            if(hit.collider.gameObject.name.Equals("HandStair"))
            {
                onLadder = true;
            }
            else
            {
                onLadder = false;
            }
        }
        else
        {
            canJump = false;
        }
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (canJump && !dontKeepJumping)
            {
                isJumping = true;
                canJump = false;
            }
        }

        if(Input.GetKey(KeyCode.Space))
        {
            jumpPressed = true;
            if (isJumping)
            {
                currentJumpTime += Time.deltaTime;

                if(currentJumpTime >= maxJumpTime)
                {
                    isJumping = false; 
                }
            }  
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            currentJumpTime = maxJumpTime;
        }

        if(isJumping)
        {
            jumpMove = jumpSpeed;
        }
        else if(jumpPressed)
        {
            jumpMove -= holdJumpFallSpeed;
        }
        else
        {
            jumpMove = -fallSpeed;
        }

        return jumpMove;
    }
    #endregion
    public void addWaitingTime()
    {
        waitingTime = 0.5f;
    }

    public void GetLaunched(float flyTime)
    {
        flyTimeLeft = flyTime;
        ladderRef.RotateToLaunch();
        thisCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (waitingTime <= 0 && other.tag.Equals("Item"))
        {
            MSIInventoryItem item = other.GetComponent<MSIInventoryItem>();
            if(item != null)
            {
                item.HandlePlayerCollision();
            }
            else
            {
                Debug.LogError("MSIDragon OnTriggerEnter2D error " + other.name + " doesnt include an MSIInventoryItem script");
            }
        }
        else if(other.tag.Equals("TutorialTrigger"))
        {
            string description = tutorials.getDescriptionByTriggerName(other.name);
            MSIDialogManager.Instance.AddSubtitles(description);
        }
        else if(other.tag.Equals("LadderHolder"))
        {
            inContactWithLadder = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("LadderHolder"))
        {
            inContactWithLadder = false;
        }
    }
}
