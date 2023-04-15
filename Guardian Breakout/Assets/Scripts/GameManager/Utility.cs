using UnityEngine;

public static class Utility
{
    public static GameObject FindObjectByTagIncludingInactive(string tag)
    {
        GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in objects)
        {
            if (obj.CompareTag(tag))
            {
                return obj;
            }
        }
        return null;
    }
}
