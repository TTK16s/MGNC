using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

    Transform target;

    Vector3 horizontal = new Vector3 (0,0, -10);
    Vector3 velocity = Vector3.zero;
    float smoothTime = 0.3f;

    void Awake()
    {
        if (Singletons.mainCameraInstance != null)
        {
            Destroy(Singletons.mainCameraInstance.gameObject);
        }
        Singletons.mainCameraInstance = this;
    }

	// Use this for initialization
	void Start ()
    {
        target = Singletons.playerInstance.gameObject.transform;
	}
	
	void LateUpdate ()
    {
        horizontal.x = target.position.x;
        //transform.position = horizontal;

        transform.position = Vector3.SmoothDamp(transform.position, horizontal, ref velocity, smoothTime);
	}
}
