using UnityEngine;
using System.Collections;

public class DestroyablePlatform : Item {

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
        player.ProcessItem(this);
        gameObject.SetActive(false);
    }
    
    public override void Reset()
    {
        gameObject.SetActive(true);
    }

}
