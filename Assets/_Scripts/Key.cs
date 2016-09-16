using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

    Player player;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("trigger " + this);
        if (other.gameObject.name == "Player")
        {
            player = other.gameObject.GetComponent<Player>();   //replace this with singleton pattern reference to player
            player.PickUpAKey();    //pass the key color as parameter?
        }
        Destroy(this.gameObject);
    }
}
