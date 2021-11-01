using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

public class LevelEditorManager : MonoBehaviour
{
    static bool PopUpEnabled;
    [SerializeField] private Transform HUD;

    void Awake()
    {
        //load the level in for editing
        LevelSerializationManager.LoadAndSpawnLevel();
        PopUpEnabled = false;
    }

    public static event Action savedAction;

    public void SavePressed()
    {
        LevelSerializationManager.SerializeLevel();
        //send out event signal for the observer later
        savedAction.Invoke();
    }

    private void MainMenuPressed()
    {
        //expand on this with observer later
        SceneManager.LoadScene("MainMenu");
    }

    //runs every frame
    private void Update()
    {
        if(!PopUpEnabled)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SavePressed();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (ChangeUpdate.GetIsSaved())
                    MainMenuPressed();
                else
                {
                    //make the action
                    Action goToMainMenuAction = () =>
                    {
                        this.MainMenuPressed();
                    };

                    //create the popup
                    GameObject NewPopUp = Instantiate(Resources.Load("Prefab/PopUp") as GameObject);
                    NewPopUp.GetComponent<PopUp>().InitPopUp(HUD, goToMainMenuAction);
                }
            }
        }
    }

    public static void SetPopUpEnabled(bool enabled)
    {
        PopUpEnabled = enabled;
    }
}
