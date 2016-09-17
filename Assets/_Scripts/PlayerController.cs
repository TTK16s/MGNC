using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour {
    
    public GameObject bombPrefab;
    GameObject litBomb;

    MovingPlatform movingPlatform;

    Rigidbody2D rb;

    public LayerMask playerMask;

    RaycastHit2D leftGroundInfo;
    RaycastHit2D rightGroundInfo;

    Vector3 position;       // make the input/player movement to work on using separate vector3 that we use to set transform.position
                            // and utilize raycast to determine if we are grounded, not grounded etc: 

    Vector3 startingPosition;

    public float speed;
    public float jumpForce;

    public bool hasBomb = false;
    public bool hasKey = false;

    public bool nearDoor = false;
    public bool onBombSite = false;

    public Door nearbyDoor = null;
    public DestroyablePlatform bombSite = null;

    public bool standingOnMovingPlatform = false;

    void Awake()
    {
        if (Singletons.playerInstance != null)
        {
            Destroy(Singletons.playerInstance);
        }

        Singletons.playerInstance = this;
    }

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;

        Physics2D.IgnoreLayerCollision(8, 20);  //ignore collision with bombs. 8 = player, 20 = bomb
    }

    void Update()
    {
        leftGroundInfo = RaycastGround(transform.position);
        rightGroundInfo = RaycastGround(transform.position);

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
                movingPlatform = RaycastGround(transform.position).collider.gameObject.GetComponent<MovingPlatform>();
            }

            transform.Translate(movingPlatform.dir * Time.deltaTime * movingPlatform.speed);

        }
        else
        {
            movingPlatform = null;
        }

        //rework all this to support crossplatform input manager and to work by using raycasts
        #region input
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-Vector3.right * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb.velocity = new Vector3(0, jumpForce, 0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (nearbyDoor != null && hasKey)
                OpenDoor(nearbyDoor);

            if (onBombSite && hasBomb)
                PlaceABomb(bombSite);

            //do actions specific to place we stand in
            // DropABomb(); or  OpenADoor();
        }
        #endregion


    }

    public RaycastHit2D RaycastGround(Vector3 point)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(point, Vector3.down, 1f, playerMask);
        return hitInfo;
    }

    public void PickUpABomb()   //allow player to carry more than one bomb at a time?
    {
        hasBomb = true;
    }

    public void PickUpAKey()    //pass the key color as parameter?
    {
        hasKey = true;
    }

    void performAction()
    {
        //use this to call DropABomB and OpenADoor Methods
    }

    public void PlaceABomb(DestroyablePlatform bs)
    {
        //when at a bombsite, drops bomb on the site
        litBomb = Instantiate(bombPrefab, transform.position, Quaternion.identity) as GameObject;
        litBomb.GetComponent<Bomb>().LightTheFuse(bs);
        hasBomb = false;
    }

    public void OpenDoor(Door door)
    {
        door.Open();
        hasKey = false;
    }

    public void Perish()
    {
        //reset player to start
        //reset the level? keys, enemies, bombs etc
        Reset();
    }

    void Reset()
    {
        transform.position = startingPosition;
        hasBomb = false;    //replace these
        hasKey = false;     //with "reset inventory"
        Debug.Log("player reseted");
    }

}
