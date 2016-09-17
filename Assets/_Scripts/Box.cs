using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour {

    public LayerMask boxMask;

    MovingPlatform movingPlatform;
    RaycastHit2D groundInfo;
    


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        groundInfo = Physics2D.Raycast(transform.position, Vector3.down, 1f, boxMask);

        if (groundInfo.collider != null)
        {
            if (groundInfo.collider.gameObject.layer == 10)
            {
                movingPlatform = groundInfo.collider.gameObject.GetComponent<MovingPlatform>();
                transform.Translate(movingPlatform.dir * Time.deltaTime * movingPlatform.speed);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "Box 2")
        {
            Debug.Log("you win");
            coll.gameObject.GetComponent<Box>().Explode();
            Explode();
        }
        
    }

    public void Explode()
    {
        Destroy(this.gameObject);
    }
}
