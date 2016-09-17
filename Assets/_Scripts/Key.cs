using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

    PlayerController player;
    
    void Start()
    {
        player = Singletons.playerInstance;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("trigger " + this);
        if (other.gameObject.name == "Player")
        {
            player.PickUpAKey();    //pass the key color as parameter?
        }
        Destroy(this.gameObject);
    }
}
