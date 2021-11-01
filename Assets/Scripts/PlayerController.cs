using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    //serialized field so we can modify it from the editor
    //the speed of the player's movement
    [SerializeField] private float moveSpeed = 10f;

    //layermask that tells us what the player can jump off of 
    [SerializeField] private LayerMask jumpMask;

    //controls for the jump
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float horiDistanceToPeak = 1f;
    [SerializeField] private float horiDistanceWhileFalling = 0.5f;
    private float jumpInitialVerticalVelo;
    private float gravityGoingUp;
    private float gravityGoingDown;
    private Vector3 currentGravity;

    //enemy collision force
    [SerializeField] private float enemyKnockbackModifier = 3.0f;
    private float enemyCollisionKnockback;
    private Vector3 knockBackDirection = Vector3.zero;
    private float currentKnockback = 0.0f;
    [SerializeField] private float knockBackTime = 1.0f;
    private float knockBackTimePassed = 0.0f;

    //character controller
    private CharacterController cc;
    [SerializeField] private float yvelo;
    [SerializeField] private bool grounded;
    private bool hitHead;
    private bool headAlreadyHit;
    private Vector3 moveVelo = Vector3.zero;

    //the player's horizontal input
    private float xinput = 0f;


    // Start is called before the first frame update
    void Start()
    {
        //grab a reference to the character controller
        cc = this.GetComponent<CharacterController>();

        //jump calculations based on the building a better jump GDC talk, source: https://youtu.be/hG9SzQxaCm8

        //use that along with the desired height, movement speed, and the disctance to peak to find the intial vertical velocity for the jump
        jumpInitialVerticalVelo = (2f * jumpHeight * moveSpeed) / horiDistanceToPeak;
        //calculate the gravity using the same variables (note two different gravities to allow for enhanced game feel)
        gravityGoingUp = (-2f * jumpHeight * (moveSpeed * moveSpeed) / (horiDistanceToPeak * horiDistanceToPeak));
        gravityGoingDown = (-2f * jumpHeight * (moveSpeed * moveSpeed) / (horiDistanceWhileFalling * horiDistanceWhileFalling));

        //set the default gravity to be the gravity going up 
        currentGravity = Vector3.down * gravityGoingUp;

        //start on the ground 
        grounded = true;
        hitHead = false;
        headAlreadyHit = false;

        //enemy knockback
        enemyCollisionKnockback = enemyKnockbackModifier * moveSpeed;
        knockBackTimePassed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = checkIsGrounded();
        hitHead = CheckHitHead();

        //grab the input from the player for movement
        xinput = Input.GetAxis("Horizontal");

        //if the player is on the ground, set the y velocity to be atleast 0
        if ((grounded && yvelo < 0) || (!grounded && yvelo < 0 && moveVelo.y == 0))
        {
            yvelo = 0;
            headAlreadyHit = false;
        }
        //if the player has pressed the space key this frame and can jump, then jump
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            yvelo += jumpInitialVerticalVelo;
        }
        //if the player has hit their head on a platform or enemy start sending them down
        if (!headAlreadyHit && hitHead)
        {
            yvelo = gravityGoingDown * Time.deltaTime;
            headAlreadyHit = true;
        }


        //set gravity based on player velocity
        if (yvelo > 0) currentGravity = Vector3.up * gravityGoingUp;
        else if (yvelo < 0) currentGravity = Vector3.up * gravityGoingDown;

        //calculate the current knockback
        currentKnockback = Mathf.Clamp(Mathf.Lerp(0.0f, enemyCollisionKnockback, knockBackTimePassed / knockBackTime), 0.0f, enemyCollisionKnockback);
        knockBackTimePassed -= Time.deltaTime;

        //apply gravity and move with the input and jump
        yvelo += currentGravity.y * Time.deltaTime;
        Vector3 hori = (new Vector3(0.0f, 0.0f, xinput * moveSpeed) + (knockBackDirection * currentKnockback));
        if (hori.magnitude < 0.0001) hori = Vector3.zero; //jitter correction
        cc.Move((hori + new Vector3(0.0f, yvelo, 0.0f)) * Time.deltaTime);

        moveVelo = cc.velocity;
    }

    //collision with enemy
    private void OnTriggerEnter(Collider collision)
    {
        //only process the collision if it's with an enemy
        if (collision.gameObject.layer == 7)
        {
            //get the direction of the collision and set it to the knockback direction
            Vector3 enemyPos = collision.transform.position;
            knockBackDirection = (transform.position - enemyPos);
            knockBackDirection.y = 0;
            knockBackDirection.Normalize();
            knockBackTimePassed = knockBackTime;

            //screenshake
            CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
            if(camFollow != null) camFollow.shakeScreen(0.25f, 0.2f);
            //subtract from the player's hp
            FindObjectOfType<GameManager>().ChangeHp(-0.2f);
        }
    }

    //check if the player is grounded using a raycast
    //based on code from the Code Monkey, Source: https://youtu.be/c3iEl5AwUF8 
    private bool checkIsGrounded()
    {
        //do the raycast 
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        RaycastHit hit;
        bool detect = Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f * transform.localScale.y, jumpMask);

        if (hit.collider != null && hit.collider.gameObject.layer == 7)
        {
            Destroy(hit.collider.gameObject);
            FindObjectOfType<GameManager>().DropEnemyNum();
        }

        //return if it hit anything
        return (hit.collider != null);
    }

    //check if the player has collided with something on their head using a raycast
    private bool CheckHitHead()
    {
        //do the raycast 
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        RaycastHit hit;
        bool detect = Physics.Raycast(transform.position, Vector3.up, out hit, 1.1f * transform.localScale.y, jumpMask);

        if (hit.collider != null && hit.collider.gameObject.layer == 7)
        {
            Destroy(hit.collider.gameObject);
            FindObjectOfType<GameManager>().DropEnemyNum();
        }

        //return if it hit anything
        return (hit.collider != null);
    }
}