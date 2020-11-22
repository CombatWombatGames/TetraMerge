using UnityEngine;

public class Singleton : MonoBehaviour
{
    private static Singleton instance = null;

    void Awake()
    {
        if (instance != null && instance != this) 
        {
            Destroy(gameObject);
        } 
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
