using UnityEngine;
using System.Collections;

public class DestroyablePlatform : MonoBehaviour {

    PlayerController player;
    
	void Start ()
    {
        player = Singletons.playerInstance;
	}
	
    public void SetTimer()
    {
        Invoke("Explode", 3);
    }

    void Explode()
    {
        Destroy(this.gameObject);
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        //Debug.Log("collision stay " + this);
        if (coll.gameObject.name == "Player")
        {
            //player = coll.gameObject.GetComponent<PlayerController>();
            player.bombSite = this;
            player.onBombSite = true;
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.name == "Player")
        {
            player.bombSite = null;
            player.onBombSite = false;
        }
    }

}
