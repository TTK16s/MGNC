using UnityEngine;
using System.Collections;

public class Bomb : Item {

    PlayerController player;
    public Rigidbody2D rb;
    public CircleCollider2D col;
    DestroyablePlatform dp = null;

    void Start()
    {
        typeString = "Bomb";
        itemName = gameObject.name;
        player = Singletons.playerInstance;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            player.ProcessItem(this);
            gameObject.SetActive(false);
        }
    }

    public void SetUpABomb()
    {
        // all your base are belong to us
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();

        gameObject.SetActive(false);
    }

    public void LightTheFuse(DestroyablePlatform bs, Vector3 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
        dp = bs;

        col.isTrigger = false;
        rb.isKinematic = false;
        bs.SetTimer();
        Invoke("Explode", 3);
    }

    void Explode()
    {
        gameObject.SetActive(false);
    }
    
    public override void Reset()
    {
        gameObject.SetActive(true);
        itemUsed = false;
    }
}


