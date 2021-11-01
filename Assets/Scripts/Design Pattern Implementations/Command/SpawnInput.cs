using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInput : MonoBehaviour
{
    Camera maincam;
    Plane gamePlane = new Plane(Vector3.right, 0);
    float distanceAlongRay = 0;
    private static bool CanSpawn;

    // Start is called before the first frame update
    void Awake()
    {
        maincam = Camera.main;
        CanSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(CanSpawn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = maincam.ScreenPointToRay(Input.mousePosition);
                if (gamePlane.Raycast(ray, out distanceAlongRay))
                {

                    SpawnFunctions command = new PlaceObjectCommand(ray.GetPoint(distanceAlongRay), "platform");
                    SpawnScript.AddCommand(command);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = maincam.ScreenPointToRay(Input.mousePosition);
                if (gamePlane.Raycast(ray, out distanceAlongRay))
                {
                    SpawnFunctions command = new PlaceObjectCommand(ray.GetPoint(distanceAlongRay), "enemy");
                    SpawnScript.AddCommand(command);
                }
            }
        }
    }

    public static void SetCanSpawn(bool can)
    {
        CanSpawn = can;
    }
}