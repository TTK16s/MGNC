using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    PlayerController player;
    Rigidbody2D rb;
    BoxCollider2D bc2D;
    DestroyablePlatform dp = null;

    void Start()
    {
        player = Singletons.playerInstance;
    }

    public void LightTheFuse(DestroyablePlatform bs)
    {
        dp = bs;
        rb = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();
        bc2D.isTrigger = false;
        rb.isKinematic = false;
        bs.SetTimer();
        Invoke("Explode", 3);
        //Debug.Log("fuse set");
    }

    void Explode()
    {
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("trigger " + this);
        if (other.gameObject.name == "Player" && dp == null)
        {
            if (!player.hasBomb)
            {
                player.PickUpABomb();
                Destroy(this.gameObject);
            }
        }
    }


}


