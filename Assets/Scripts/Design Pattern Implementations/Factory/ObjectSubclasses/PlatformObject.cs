using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformObject : CreateableObjects
{
    public override string ObjectTypeName => "platform";

    public override Transform Spawn(Vector3 position)
    {
        //create the new platform
        Transform newPlatform = Object.Instantiate(Resources.Load("Prefab/Platform Prefab", typeof(GameObject)) as GameObject, position, Quaternion.identity).transform;

        //and return it
        return newPlatform;
    }
}