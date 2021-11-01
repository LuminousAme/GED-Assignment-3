using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;

//factory implementation based on the one presented in the following video: https://youtu.be/FGVkio4bnPQ

public abstract class CreateableObjects
{
    //string to name the type of object, will be overloaded by subclasses so the factory can run generically
    public abstract string ObjectTypeName { get; }

    //method the subclasses will overload to spawn objects 
    public abstract Transform Spawn(Vector3 position);
}

public static class ObjectFactory {
    private static Dictionary<string, Type> objectTypesLookUp = null;
    
    private static void InitFactory()
    {
        //if the object types dictonary is not null than it the factory has already been initlized and the function can just exit
        if (objectTypesLookUp != null)
            return;

        //otherwise we need to set it up

        //so gather all of the types that are derived from object in the project
        var allObjectTypes = Assembly.GetAssembly((typeof(CreateableObjects))).GetTypes().Where(thisType => !thisType.IsAbstract && thisType.IsSubclassOf(typeof(CreateableObjects)));

        //create the dictonary
        objectTypesLookUp = new Dictionary<string, Type>();

        //put each type into the dictonary
        foreach (var type in allObjectTypes)
        {
            //make a temporary copy of the object so we can extract it's type name, it'll be discarded from memory as soon as we leave scope anyways
            var tempObj = (CreateableObjects)Activator.CreateInstance(type);
            //add it to the look up table
            objectTypesLookUp.Add(tempObj.ObjectTypeName, type);
        }
    }

    public static CreateableObjects MakeObject(string ObjectType)
    {
        //lazily initilize the factory, only when it's first being used (return statement in the function ensures it only inits once)
        InitFactory();

        //check if the type exists
        if(objectTypesLookUp.ContainsKey(ObjectType))
        {
            //if it does grab it, make a new copy of it, and return that copy
            Type type = objectTypesLookUp[ObjectType];
            //using Activator.CreateInstance() instead of new because of the generic type class
            var NewObject = (CreateableObjects)Activator.CreateInstance(type);
            return NewObject;
        }

        return null;
    }
}
