using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

    public float speed;
    public float range;

    public bool isElevator = false;

    Vector3 pos;
    Vector3 startingPos;

    public Vector3 dir;

    

    void Start()
    {
        pos = transform.position;
        startingPos = transform.position;

        if (!isElevator)
        {
            dir = Vector3.right;
        }
        else
        {
            dir = Vector3.up;
        }
    }

    void Update()
    {
        if (!isElevator)
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
        else
        {
            if (pos.y > startingPos.y + range)
            {
                dir = -Vector3.up;
            }
            if (pos.y < startingPos.y - range)
            {
                dir = Vector3.up;
            }

            pos = pos + dir * Time.deltaTime * speed;

            transform.position = pos;
        }
    }

}
