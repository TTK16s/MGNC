using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour {

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
            player.Perish();
        }
    }
}
