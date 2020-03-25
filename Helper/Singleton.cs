using UnityEngine;

// singleton class to easily access data
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T Instance;

    public static T get
    {
        get
        {
            // if no instance is found, find reference to this singleton
            if(Instance == null)
            {
                Instance = FindObjectOfType<T>();
                if(Instance == null)
                {
                    Instance = new GameObject().AddComponent<T>();
                }
                // adds this singleton to the singleton reset list
                SingletonReset.Singletons.Add(Instance);
            }
            return Instance;
        }
    }

    // singleton needs to be reset on scene change, otherwise data could be corrupted
    public void Reset()
    {
        Instance = null;
    }
}