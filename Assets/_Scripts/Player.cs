using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public GameObject bombPrefab;
    GameObject litBomb;

    Rigidbody2D rb;
    BoxCollider2D playerColl;

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
    
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;

        Physics2D.IgnoreLayerCollision(8, 9);
	}

    void Update()
    {
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
        hasBomb = false;
        hasKey = false;
        Debug.Log("player reseted");
    }

}
