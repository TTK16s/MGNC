using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class Level
{
    public List<Item> items = new List<Item>();


    public void Reset()
    {
        foreach(Item i in items)
        {
            i.Reset();
            
        }
        items.Clear();
    }
}
public class Inventory
{
    public List<Item> items = new List<Item>();

    public bool CheckFor(string neededItem)
    {
        bool itemFound = false;

        foreach(Item i in items)
        {
            if (i.typeString == neededItem && i.itemUsed == false)
            {
                itemFound = true;
            }
        }
        return itemFound;
    }

    public void Reset()
    {
        Debug.Log("inventory reset");
        items.Clear();
    }
}

public class PlayerController : MonoBehaviour {

    public LayerMask groundMask;
    public LayerMask itemMask;

    public LayerMask doorMask;
    public LayerMask explodingPlatformMask;

    public enum State       { IDLE, RUN, JUMP, FALL, PUSHING }

    public enum Direction   { RIGHT, LEFT }

    public State playerState = State.IDLE;
    public Direction playerDir = Direction.RIGHT;

    public GameObject bombPrefab;   //probably works
    Bomb bomb;

    Level currentLevel = new Level();
    Inventory inventory = new Inventory();

    Rigidbody2D rb;
    SpriteRenderer playerRenderer;
    Animator playerAnimator;

    MovingPlatform movingPlatform;

    RaycastHit2D wallInfo;
    RaycastHit2D leftGroundInfo;
    RaycastHit2D rightGroundInfo;

    Vector3 startingPosition;
    Vector3 direction;
    Vector3 horizontalMovement;

    float xVelocity;

    public float runSpeed;
    public float jumpForce;

    public bool hasJumped = false;

    public bool standingOnMovingPlatform = false;

    public bool blueKey;
    public bool yellowKey;
    public bool redKey;

    public float jumpTime;
    public float jumpTimer;
    
    void Awake()
    {
        if (Singletons.playerInstance != null)
        {
            Destroy(Singletons.playerInstance.gameObject);
        }
        Singletons.playerInstance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();

        startingPosition = transform.position;
        direction = Vector3.right;  //ota talteen alku suunta resettejä varten?

        GameObject superHandle = Instantiate(bombPrefab, new Vector3(0, 0, -20), Quaternion.identity) as GameObject;
        //bomb = Instantiate(bombPrefab, new Vector3(0, 0, -20), Quaternion.identity) as GameObject;
        bomb = superHandle.GetComponent<Bomb>();
        bomb.SetUpABomb();
        //bomb.GetComponent<Bomb>().SetUpABomb();

    }
    
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "player X: " + xVelocity.ToString("#.00"));
        GUI.Label(new Rect(10, 30, 100, 20), "player y: " + rb.velocity.y.ToString("#.00"));
    }
    

    void Update()
    {
        //mitä vitun paskaa

        wallInfo = RayCastWall(transform.position);
        leftGroundInfo = RayCastGround(transform.position);
        rightGroundInfo = RayCastGround(transform.position);

        if (leftGroundInfo.collider != null || rightGroundInfo.collider != null)
        {
            if (leftGroundInfo.collider.gameObject.layer == 10 || rightGroundInfo.collider.gameObject.layer == 10)  // 10 = MovingPlatform Layer
            {
                standingOnMovingPlatform = true;
            }
            else
            {
                standingOnMovingPlatform = false;
            }
        }
        else
        {
            standingOnMovingPlatform = false;
        }

        if (standingOnMovingPlatform)
        {
            if (movingPlatform == null)
            {
                movingPlatform = RayCastGround(transform.position).collider.gameObject.GetComponent<MovingPlatform>();
            }

            transform.Translate(movingPlatform.dir * Time.deltaTime * movingPlatform.speed);

        }
        else
        {
            movingPlatform = null;
        }
        //oikeesti

        xVelocity = Input.GetAxis("Horizontal");
        playerAnimator.SetFloat("X", Mathf.Abs(xVelocity));

        if (xVelocity > 0 && wallInfo.collider == null)  //voi tiivistää yhteen iffiin josta saadaan vaan mathf.abs ja katsotaan suuntaa sitten erikseen
        {
            playerState = State.RUN;
            playerAnimator.SetBool("Pushing", false);
            GetDirection(xVelocity);
        }
        else if (xVelocity < 0 && wallInfo.collider == null)
        {
            playerState = State.RUN;
            playerAnimator.SetBool("Pushing", false);
            GetDirection(xVelocity);
        }

        switch (playerState)
        {
            case State.IDLE:
                playerAnimator.SetBool("Pushing", false);
                break;
            case State.RUN:
                //playerAnimator.SetBool("Pushing", false);
                break;
            case State.JUMP:
                playerAnimator.SetBool("Pushing", false);
                //playerAnimator.SetTrigger("JumpTrigger");
                break;
            case State.FALL:
                playerAnimator.SetBool("Pushing", false);

                break;
            case State.PUSHING:
                playerAnimator.SetBool("Pushing", true);
                break;
            default:
                break;
        }
        switch (playerDir)
        {
            case Direction.RIGHT:
                playerRenderer.flipX = false;
                direction = Vector3.right;
                break;
            case Direction.LEFT:
                playerRenderer.flipX = true;
                direction = Vector3.left;
                break;
        }

        jumpTimer = jumpTimer + Time.deltaTime *1.1f;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (!hasJumped)
            {
                //Debug.Log("lol");
                jumpTimer = 0f;
                hasJumped = true;
            }
            Jump();
        }

        if (rb.velocity.y < 0)
        {
            playerAnimator.SetTrigger("FallTrigger");
            Invoke("ResetFallTrigger", 0.1f);
        }
        
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Action();
        }

        horizontalMovement.x = Input.GetAxis("Horizontal") *Time.deltaTime * runSpeed;
        transform.Translate(horizontalMovement);
    }

    void GetDirection(float x)
    {
        if (x > 0)
        {
            playerDir = Direction.RIGHT;
        }
        else if (x < 0)
        {
            playerDir = Direction.LEFT;
        }
    }

    void Jump()
    {
        playerAnimator.SetTrigger("JumpTrigger");
        Invoke("ResetJumpTrigger", 0.1f);
        if (jumpTimer < jumpTime)
        {
            rb.velocity = (new Vector2(0, jumpForce));
        }
    }

    void ResetJumpTrigger()
    {
        playerAnimator.ResetTrigger("JumpTrigger");
    }
    void ResetLandingTrigger()
    {
        playerAnimator.ResetTrigger("LandingTrigger");
    }
    void ResetFallTrigger()
    {
        playerAnimator.ResetTrigger("FallTrigger");
    }

    void Action()
    {
        Door nearbyDoor;
        DestroyablePlatform bombSite;

        if (Physics2D.OverlapCircle(transform.position, 0.7f, explodingPlatformMask) != null)
        {
            bombSite = Physics2D.OverlapCircle(transform.position, 1f, itemMask).GetComponent<DestroyablePlatform>();
            if (inventory.CheckFor("Bomb"))
            {
                bomb.LightTheFuse(bombSite, transform.position);
            }
            else
            {
                Debug.Log("no bomb found"); //play some error noise here 
            }
        }
        else if (Physics2D.OverlapCircle(transform.position, 0.7f, doorMask) != null)
        {
            nearbyDoor = Physics2D.OverlapCircle(transform.position, 0.7f, itemMask).GetComponent<Door>();
            //if right kind of key  -> if the key fits -> mark the key as used
            if (inventory.CheckFor("Key"))   //pass key color as parameter?
            {
                nearbyDoor.Open();
            }
            else
            {
                Debug.Log("No key Found");  //play some error noise here
            }
        }
    }

    public RaycastHit2D RayCastGround(Vector3 point)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(point, Vector3.down, 0.55f, groundMask);
        Debug.DrawLine(point, (point + new Vector3(0,-0.5f,0)), Color.green);
        if (hitInfo.transform != null)
        {
            if (hitInfo.transform.gameObject.layer == 9|| hitInfo.transform.gameObject.layer == 10)
            {
                hasJumped = false;
                playerAnimator.SetTrigger("LandingTrigger");
                Invoke("ResetLandingTrigger", 0.1f);
            }
        }
        return hitInfo;
    }
    public RaycastHit2D RayCastWall(Vector3 point)
    {
        Debug.DrawLine(point, (point + new Vector3(-0.25f, 0, 0)), Color.red);
        RaycastHit2D hitInfo = Physics2D.Raycast(point, direction, 0.25f, groundMask);
        if (hitInfo.transform !=null)
        {
            Debug.Log("lol");
            playerState = State.PUSHING;
        }
        else
        {
            playerState = State.IDLE;
        }
        return hitInfo;
    }

    public void ProcessItem(Item i)
    {
        currentLevel.items.Add(i);
        if (i.itemName == "Key")
        {
            inventory.items.Add(i);
        }
        if (i.itemName == "Bomb")
        {
            inventory.items.Add(i);
        }
    }

    public void Perish()
    {
        currentLevel.Reset();
        inventory.Reset();
        Reset();
    }

    void Reset()
    {
        transform.position = startingPosition;
        blueKey = false;
        yellowKey = false;
        redKey = false;
        Debug.Log("player reseted");
    }


}
