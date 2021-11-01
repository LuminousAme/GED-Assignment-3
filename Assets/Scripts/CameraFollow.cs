using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    //serialized field so we can modify it from the editor
    //the offset from the player's position
    [SerializeField] private Vector3 offetFromPlayer;

    //the range away from that offset on each axis is acceptable, after this the camera begins moving to return to the acceptable range
    [SerializeField] private Vector3 acceptableRange;

    //the player's transform to check their position
    public Transform playerTrans;

    //internal position
    private Vector3 thisPos;
    //offset from the internal position for screenshake
    private Vector3 thisOffset;

    //screenshot variables
    private float timeToShake;
    private float shakeStrenght;

    // Start is called before the first frame update
    void Start()
    {
        //start the internal position off at the transform
        thisPos = transform.position;
        //start the screenshake offset at zero
        thisOffset = Vector3.zero;
        //start off with no screenshake
        timeToShake = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //calculate the difference between the player and camera's position
        Vector3 diff = thisPos - playerTrans.position;

        //make a vector for how much the camera should correct
        Vector3 correction = Vector3.zero;

        //check each axis
        if (Mathf.Abs(diff.x) > acceptableRange.x)
        {
            correction.x = diff.x;
            correction.x += (diff.x < 0) ? acceptableRange.x : -1 * acceptableRange.x;        
        }

        if (Mathf.Abs(diff.y) > acceptableRange.y)
        {
            correction.y = diff.y;
            correction.y += (diff.y < 0) ? acceptableRange.y : -1 * acceptableRange.y;
        }

        if (Mathf.Abs(diff.z) > acceptableRange.z)
        {
            correction.z = diff.z;
            correction.z += (diff.z < 0) ? acceptableRange.z : -1 * acceptableRange.z;
        }

        //update the internal position
        thisPos -= correction;

        thisOffset = Vector3.zero;

        //Handle screenshake
        if (timeToShake > 0.0f)
        {
            timeToShake -= Time.deltaTime;
            float randX = Random.Range(-1f, 1f);
            float randy = Random.Range(-1f, 1f);

            thisOffset = new Vector3(0f, randy, randX) * shakeStrenght;
        }

        //update acutal position
        this.transform.position = thisPos + thisOffset;
    }

    public void shakeScreen(float duration, float strenght)
    {
        timeToShake = duration;
        shakeStrenght = strenght;
    }
}