using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
    
    Player player;
    Rigidbody2D rb;
    BoxCollider2D bc;
    DestroyablePlatform dp = null;

    public void LightTheFuse(DestroyablePlatform bs)
    {
        dp = bs;
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        bc.isTrigger = false;
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
            player = other.gameObject.GetComponent<Player>();   //replace this with singleton pattern reference to player
            if (!player.hasBomb)
            {
                player.PickUpABomb();
                Destroy(this.gameObject);
            }
        }
    }


}


