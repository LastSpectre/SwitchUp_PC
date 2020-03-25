using System.Collections.Generic;
using UnityEngine;

public class SingletonReset : MonoBehaviour
{
    public static List<MonoBehaviour> Singletons = new List<MonoBehaviour>();

    // reset every value from all singletons | used at scene reset
    public static void Reset()
    {
        foreach (var item in Singletons)
        {
            if(item != null)
            {
                item.SendMessage("Reset");
            }
        }
        Singletons.Clear();
    }
}
