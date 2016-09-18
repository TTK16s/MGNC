using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class Singletons : MonoBehaviour {

    public static Singletons instance;

    public static PlayerController playerInstance;
    public static Camera mainCameraInstance;

    //musicManager instance
    //levelManager instance
    //menuManager  instance

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
