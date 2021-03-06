using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class LevelSelectionSystem : MonoBehaviour
{
    private static List<string> AllLevels;
    private static bool isInit = false;
    [SerializeField] private Dropdown levelPicker;
    [SerializeField] private Text newLevelNameInput;
    private bool manualChange = false;

    // Start is called before the first frame update
    void Start()
    {
        //only initliaze the levels once
        if (!isInit)
        {
            //find all of the level files
            string dir = Application.persistentDataPath;
            string[] files = Directory.GetFiles(dir, "*.atxsl");

            //iterate over them and copy their names without surronding path and extension
            AllLevels = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                AllLevels.Add(Path.GetFileNameWithoutExtension(files[i]));
            }

            //handle if there are no levels stored
            if (AllLevels.Count == 0)
            {
                //create a new empty level called sample level
                LevelSerializationManager.CurrentFile = "SampleLevel";
                LevelSerializationManager.SerializeLevel();
                //set the dirty flag to false since the serialization will have it loaded in C++ memory
                LevelSerializationManager.isDirty = false;
            }
            else
            {
                LevelSerializationManager.CurrentFile = AllLevels[0];
                //set the dirty flag to true since it won't have been loaded yet
                LevelSerializationManager.isDirty = true;
            }

            //add all of the levels to the picker and set it to the first one
            levelPicker.AddOptions(AllLevels);
            levelPicker.value = 0;

            //set is init to true
            isInit = true;
        }
        //if it's already initalized set the dropdown to match the data
        else
        {
            levelPicker.ClearOptions();
            levelPicker.AddOptions(AllLevels);
            int index = AllLevels.IndexOf(LevelSerializationManager.CurrentFile);
            if (levelPicker.value != index)
            {
                manualChange = true;
                levelPicker.value = index;
            }
        }
    }

    // When the level the player is selecting changes
    public void SelectedLevelChanged(int index)
    {
        if(!manualChange)
        {
            //set the current filename in the serialization manager to match the one selected
            LevelSerializationManager.CurrentFile = AllLevels[index];
            //set the dirty flag to false since the file for this won't have been loaded
            LevelSerializationManager.isDirty = true;
        }

        manualChange = false;
    }

    //function for when the player hits the button to create a new level
    public void CreateNewLevelPressed()
    {
        string newLevelName = newLevelNameInput.text;
        //make sure there is acutally a name for the level
        if (newLevelName != "")
        {
            //check if it's already in the list
            bool isAlreadyInList = false;
            int index = -1;
            for (int i = 0; i < AllLevels.Count; i++)
            {
                if (newLevelName == AllLevels[i])
                {
                    isAlreadyInList = true;
                    index = i;
                    break;
                }
            }

            //if it isn't add it to the list of levels and too the dropdown
            if (!isAlreadyInList)
            {
                AllLevels.Add(newLevelName);
                levelPicker.AddOptions(new List<string> { newLevelName });
                index = AllLevels.Count - 1;
            }

            //make the dropdown select that one
            levelPicker.value = index;

            //set the current file in the serialzed manager to point at it and create (or recreate) the file
            LevelSerializationManager.CurrentFile = newLevelName;
            LevelSerializationManager.SerializeLevel();
        }
    }
}
