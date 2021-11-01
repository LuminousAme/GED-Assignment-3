using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int EnemyNum;
    private int TotalEnemyNum;
    [SerializeField] Text enemyTracker;
    [SerializeField] Slider hpTracker;
    private float playerHP;

    // awake is called when the level is first started
    void Awake()
    {
        //load the level with all of the enemies and platforms
        LevelSerializationManager.LoadAndSpawnLevel();

        //create the player
        Transform newPlayer = ObjectFactory.MakeObject("player").Spawn(new Vector3(0.0f, 0.0f, -10.0f));
        //tell the camera to follow them
        CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
        if(camFollow != null) camFollow.playerTrans = newPlayer;

        //find the number of enemies
        EnemyController[] enemies = Object.FindObjectsOfType<EnemyController>();
        TotalEnemyNum = enemies.Length;
        EnemyNum = TotalEnemyNum;
        enemyTracker.text = "Enemies: " + EnemyNum.ToString() + "/" + TotalEnemyNum.ToString();

        //set the player's starting hp 
        playerHP = 1.0f;
        hpTracker.value = playerHP;
    }

    // Update is called once per frame
    void Update()
    {
        //if there're no more enemies
        if(EnemyNum <= 0)
        {
            //play the win scene
            SceneManager.LoadScene("YouWin");
        }

        //if the player is out of hp
        if (playerHP <= 0.0f)
        {
            //play the game over scene
            SceneManager.LoadScene("GameOver");
        }
    }

    //decreases the number of enemies in the scene and updates the ui
    public void DropEnemyNum()
    {
        EnemyNum--;
        enemyTracker.text = "Enemies: " + EnemyNum.ToString() + "/" + TotalEnemyNum.ToString();
    }

    //sets the health of the player
    public void ChangeHp(float hp)
    {
        playerHP += hp;
        hpTracker.value = playerHP;
    }
}
