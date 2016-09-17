using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    PlayerController player;

    void Start()
    {
        player = Singletons.playerInstance;
    }

    public void Open()
    {
        this.gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        //Debug.Log("trigger enter " + this);
        if (coll.gameObject.name == "Player")
        {
            player.nearbyDoor = this;
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        //Debug.Log("trigger enter " + this);
        if (coll.gameObject.name == "Player")
        {
            player.nearbyDoor = null;
        }
    }


}
