using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    Player player;

    public void Open()
    {
        this.gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        //Debug.Log("trigger enter " + this);
        if (coll.gameObject.name == "Player")
        {
            player = coll.gameObject.GetComponent<Player>();
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
