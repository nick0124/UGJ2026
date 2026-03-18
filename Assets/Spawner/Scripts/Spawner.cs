using Unity.VisualScripting;
using UnityEngine;

public class Spawner
{
    public T CreateObject<T>(T obj, Vector3 position) where T : Object
    {
        T spawnObj = GameObject.Instantiate(obj, position, Quaternion.identity);

        return spawnObj;
    }
}
