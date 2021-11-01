using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    //the start and end of their path
    public Vector3 pathStart, pathEnd;
    //the speed at which they travel it in units/second
    [SerializeField] private float travelSpeed = 5f;

    //some variables to acutally do interpolation, calculated based on the serialized info
    private float totalTravelTime;
    private float currentTraveledTime;
    private float direction;

    //rigidbody
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //start the direction as being positive (start to end)
        direction = 1f;

        //use the dll to generate some random positions for the enemy to start and end at
        //pathStart = MidtermPluginManager.generateEnemyPosition(new Vector2(0f, 0f), new Vector2(0f, 25f), new Vector2(-5f, 30f), pathStart);
        //pathEnd = MidtermPluginManager.generateEnemyPosition(new Vector2(0f, 0f), new Vector2(0f, 25f), new Vector2(-5f, 30f), pathEnd);

        //get the distance between the start and end of the path
        float distance = Mathf.Abs(Vector3.Distance(pathStart, pathEnd));
        //find the total time needed to travel that distance
        totalTravelTime = distance / travelSpeed;

        //set the current time travelled to zero
        currentTraveledTime = 0f;

        //grab a reference to the rigidbody
        rb = this.GetComponent<Rigidbody>();
    }

    //Fixed update is called every physics step
    private void FixedUpdate()
    {
        //add to the time that has passed while travelling
        currentTraveledTime += Time.fixedDeltaTime * direction;

        //if it has reached the end on either side, reverse it's direction
        if (currentTraveledTime < 0.0f || currentTraveledTime > totalTravelTime) direction *= -1;

        //create an interpolation parameter from the times
        float t = Mathf.Clamp(currentTraveledTime / totalTravelTime, 0.0f, 1.0f);

        //use a lerp to calculate the position
        rb.MovePosition(Vector3.Lerp(pathStart, pathEnd, t));
    }
}