using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    static List<Transform> obj;
    static uint currentId;

    public static uint PlaceObject(Vector3 position, string type)
    {
        if (obj == null)
        {
            obj = new List<Transform>();
            currentId = 0;
        }
        if (position.y < 0) position.y = 0; //don't allow the player to place objects below the ground
        Transform newObject = ObjectFactory.MakeObject(type).Spawn(position);
        obj.Add(newObject);
        currentId++;
        newObject.GetComponent<ObjectIDContainer>().ID = currentId;

        return currentId;
    }

    public static void RemoveObject(uint id)
    {
        for(int i = 0; i < obj.Count; i++)
        {
            if (obj[i].GetComponent<ObjectIDContainer>().ID == id)
            {
                GameObject.Destroy(obj[i].gameObject);
                obj.RemoveAt(i);
                break;
            }
        }
    }
}
