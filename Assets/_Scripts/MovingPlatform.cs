using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

    public float speed;
    public float range;

    Vector3 pos;
    Vector3 startingPos;
    public Vector3 dir;

    

    void Start()
    {
        pos = transform.position;
        startingPos = transform.position;
        dir = Vector3.right;
    }

    void Update()
    {

        if (pos.x > startingPos.x + range)
        {
            dir = -Vector3.right;
        }
        if (pos.x < startingPos.x - range)
        {
            dir = Vector3.right;
        }

        pos = pos + dir * Time.deltaTime * speed;

        transform.position = pos;
    }

}
