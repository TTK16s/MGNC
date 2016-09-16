using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour {

    Player player;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trigger " + this);
        if (other.gameObject.name == "Player")
        {
            player = other.gameObject.GetComponent<Player>();   //replace this with singleton pattern reference to player
            player.Perish();
        }
    }
}
