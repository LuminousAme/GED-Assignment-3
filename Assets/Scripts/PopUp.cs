using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PopUp : MonoBehaviour
{
    //based on the following pop up tutorial https://youtu.be/Bm62aXuVX4I

    [SerializeField] private Button firstButton;
    [SerializeField] private Button secondButton;

    //will execute when the pop is created
    private void OnEnable()
    {
        SpawnInput.SetCanSpawn(false);
        LevelEditorManager.SetPopUpEnabled(true);
    }

    //will execute when the pop is deleted
    private void OnDisable()
    {
        SpawnInput.SetCanSpawn(true);
        LevelEditorManager.SetPopUpEnabled(false);
    }

    public void InitPopUp(Transform canvas, Action button1Action, Action button2Action = null)
    {
        //attaches it to the canvas anhd fix the scale
        transform.SetParent(canvas);
        transform.localScale = Vector3.one;
        GetComponent<RectTransform>().offsetMin = Vector2.zero;
        GetComponent<RectTransform>().offsetMax = Vector2.zero;

        //use a lambda to define what happens for the first and second button
        firstButton.onClick.AddListener(() => {
            button1Action();
            Destroy(this.gameObject);
        }
        );

        //if the action for the second button is null just have it destroy the pop up
        if (button2Action == null)
        {
            secondButton.onClick.AddListener(() =>
            {
                Destroy(this.gameObject);
            });
        }
        //otherwise use a lambda to connect it to another action
        else
        {
            secondButton.onClick.AddListener(() =>
            {
                button2Action();
                Destroy(this.gameObject);
            });
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
