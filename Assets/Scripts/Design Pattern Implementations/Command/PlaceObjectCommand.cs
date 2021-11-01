using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjectCommand : SpawnFunctions
{
    Vector3 position;
    string type;
    uint id;

    public PlaceObjectCommand(Vector3 position, string type)
    {
        this.position = position;
        this.type = type;
    }

    public void Execute()
    {
        id = SpawnObject.PlaceObject(position, type);
    }

    public void Undo()
    {
        SpawnObject.RemoveObject(id);
    }
}
