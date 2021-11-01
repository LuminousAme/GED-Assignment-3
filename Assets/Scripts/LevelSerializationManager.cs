using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;

public static class LevelSerializationManager
{
    ///DLL INTERFACE///

    //make the struct to pass vector3 between each other
    public struct V3
    {
        public float x, y, z;
    }

    //Import the functions from the plugin
    [DllImport("SimpleLevelSerializer")]
    private static extern void LoadLevel(string fileName); //loads the level data in C++ memory

    [DllImport("SimpleLevelSerializer")]
    private static extern void SaveLevel(string fileName);//saves the level data from C++ memory into a file

    [DllImport("SimpleLevelSerializer")]
    private static extern void ClearData(); //clears the C++ memory data

    [DllImport("SimpleLevelSerializer")]
    private static extern int GetNumOfEnemies(); //get the number of enemies in the C++ memory data

    [DllImport("SimpleLevelSerializer")]
    private static extern V3 GetEnemyPosFromIndex(int index); //get an enemy at a specific index in the C++ memeory data

    [DllImport("SimpleLevelSerializer")]
    private static extern void AddEnemyToLevel(float x, float y, float z); //add an enemy to the C++ memory data

    [DllImport("SimpleLevelSerializer")]
    private static extern int GetNumOfPlatforms(); //get the nmber of platforms in the C++ memory data

    [DllImport("SimpleLevelSerializer")]
    private static extern V3 GetPlatformPosFromIndex(int index); //get a platform at a specific index in the C++ memory data

    [DllImport("SimpleLevelSerializer")]
    private static extern void AddPlatformToLevel(float x, float y, float z); //add a platfrom to the C++ memory data

    ///THIS SCRIPT TO THE REST OF PROJECT INTERFACE///
    public static string CurrentFile = null;

    public static void SerializeLevel()
    {
        //check we have a filename to save to (it doesn't matter if the file exists, the dll will create it if it doesn't, but we do need a name+path
        if (CurrentFile != null)
        {
            //clear all of the data on the C++ side, we're ovveriding it anyways
            ClearData();

            //find all of the objects with an id (these will be enemies and platforms)
            var serializableObjects = GameObject.FindObjectsOfType<ObjectIDContainer>();

            //iterate over each of them adding them to the C++ memory
            foreach (var obj in serializableObjects)
            {
                //get it's position
                Vector3 pos = obj.transform.position;

                //figure out if they are an enemy for not
                if (obj.GetComponentInChildren<EnemyController>() != null) AddEnemyToLevel(pos.x, pos.y, pos.z); //if it is, add it as an enemy
                else AddPlatformToLevel(pos.x, pos.y, pos.z); //otherwise it must be a platform, so save it as a platform
            }

            //now that all of the objects are in C++ memory, just tell the dll to serialize it to the selected filename
            SaveLevel(Application.persistentDataPath + "/" + CurrentFile + ".atxsl"); //.atxsl - Atlas X Serialized Level

            //clear all of the data on the C++ side, we don't need it there anymore
            ClearData();
        }
        else Debug.LogError("No File Name present");
    }

    public static void LoadAndSpawnLevel()
    {
        //clear the data on the C++, we're about to read it all in from a file anyways
        ClearData();

        //check we have a filename to read from
        if (CurrentFile != null)
        {
            //load in the data from the file
            LoadLevel(Application.persistentDataPath + "/" + CurrentFile + ".atxsl"); //.atxsl - Atlas X Serialized Level

            //get the data for all of the platforms and send them over to the factory
            int numOfPlatforms = GetNumOfPlatforms();
            for (int i = 0; i < numOfPlatforms; i++)
            {
                //get the position and convert it to a unity vector3
                V3 pos = GetPlatformPosFromIndex(i);
                Vector3 position = new Vector3(pos.x, pos.y, pos.z);
                //and use the factory to spawn a platform there
                ObjectFactory.MakeObject("platform").Spawn(position);
            }

            //get the data for all of the enemies and send them over to the factory
            int numOfEnemies = GetNumOfEnemies();
            for (int i = 0; i < numOfEnemies; i++)
            {
                //get the position and convert it to a unity vector3
                V3 pos = GetEnemyPosFromIndex(i);
                Vector3 position = new Vector3(pos.x, pos.y, pos.z);
                //and use the factory to spawn an enemy there
                ObjectFactory.MakeObject("enemy").Spawn(position);
            }

            //clear all of the data on the C++ side, as the level is now loaded in unity we don't need it in C++ anymore
            ClearData();
        }
        else Debug.LogError("No File Name present");
    }
}