using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : CreateableObjects
{
    public override string ObjectTypeName => "player";

    public override Transform Spawn(Vector3 position)
    {
        //create the player
        Transform player = Object.Instantiate(Resources.Load("Prefab/Player Prefab", typeof(GameObject)) as GameObject, position, Quaternion.identity).transform;

        //and return it
        return player;
    }
}
