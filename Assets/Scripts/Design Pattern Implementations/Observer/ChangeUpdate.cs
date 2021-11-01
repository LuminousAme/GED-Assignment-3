using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//based on observer pattern video : https://youtu.be/UWMmib1RYFE

public class ChangeUpdate : MonoBehaviour
{
    //The UI text indicating if level editing changes have been saved or not
    public Text UnsaveUI;
    //bool to track if all the changes are saved or not
    private static bool SavedState;

    private void Start()
    {
        UnsaveUI.text = "";
        SavedState = true;
    }

    //subscribes to delegate
    private void OnEnable()
    {
        SpawnDetect.ObjectChanged += UpdateSave;
        LevelEditorManager.savedAction += Saved;
    }

    //Unsibscribes to delegate
    private void OnDisable()
    {
        SpawnDetect.ObjectChanged -= UpdateSave;
        LevelEditorManager.savedAction -= Saved;
    }

    //change text to unsaved
    public void UpdateSave()
    {
        UnsaveUI.text = "Unsaved Changes";
        SavedState = false;
    }

    //change text to saved
    public void Saved()
    {
        UnsaveUI.text = "All Changes Saved";
        SavedState = true;
    }

    //function to get if the editted level has been saved or not
    public static bool GetIsSaved()
    {
        return SavedState;
    }
}
