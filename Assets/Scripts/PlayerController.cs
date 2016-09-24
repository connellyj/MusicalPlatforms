/**
 * Written by Julia Connelly, 9/28/2016
 * 
 * Controls the players movement and interaction
 */

using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public Transform groundCheck;

    bool jump;
    bool babyJump;
    bool seenByCamera;
    bool facingRight;

    float moveForce = 150f;
    float maxSpeed = 3f;
    float jumpForce = 325f;
    float babyJumpForce = 105f;
    
    Rigidbody2D playerRigidbody;
    SpriteRenderer playerRenderer;
    Transform mostRecentNote;



    void Start() {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
        jump = false;
        babyJump = false;
        seenByCamera = false;
    }
    


    void Update() {
        if(!GameManager.isGamePlaying()) playerRigidbody.Sleep();
        else {
            if(isPlayerOffscreen()) GameManager.endGame(false);
            else {
                RaycastHit2D noteHit = checkIfOnNote();
                if(noteHit) {
                    if(mostRecentNote != noteHit.transform) {
                        activateNote(noteHit.transform);
                    }
                    setJumpStatus();
                } else {
                    unbindTransform();
                }
                mostRecentNote = noteHit.transform;
            }
        }
    }



    void FixedUpdate() {
        if(GameManager.isGamePlaying()) {
            // Gets horizontal input
            float h = Input.GetAxis("Horizontal");

            // If the input is low, stop the players horizontal movement
            if (h < 0.001f && h > -0.001f) {
                playerRigidbody.velocity = new Vector2(0, playerRigidbody.velocity.y);
            }

            // If the desired speed is less than the max speed, add horizontal force to the player
            if (h * playerRigidbody.velocity.x < maxSpeed) {
                playerRigidbody.AddForce(Vector2.right * h * moveForce);
            }

            // If the player is moving faster than max speed, sets its speed to max speed
            if (Mathf.Abs(playerRigidbody.velocity.x) > maxSpeed) {
                playerRigidbody.velocity = new Vector2(Mathf.Sign(playerRigidbody.velocity.x) * maxSpeed, playerRigidbody.velocity.y);
            }

            // Normal jump
            if (jump) {
                playerRigidbody.AddForce(new Vector2(0f, jumpForce));
                jump = false;
            }

            // Small jump
            if(babyJump) {
                playerRigidbody.AddForce(new Vector2(0f, babyJumpForce));
                babyJump = false;
            }

            if(h > 0 && !facingRight) {
                flip();
            }

            if(h < 0 && facingRight) {
                flip();
            }
        }
    }



    void flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }



    // Returns a RayCastHit2D of the note the player is on (also acts as a boolean)
    RaycastHit2D checkIfOnNote() {
        return Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Note"));
    }



    // Plays the notes audio and binds the players transform to it
    void activateNote(Transform note) {
        bindTransform(note);
        note.gameObject.GetComponent<MusicNoteController>().playNote();
    }



    // Checks for jump input
    void setJumpStatus() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            babyJump = true;
        }
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            jump = true;
        }
    }



    // Checks if the player is offscreen
    bool isPlayerOffscreen() {
        if(playerRenderer.isVisible) {
            if(!seenByCamera) seenByCamera = true;
            return false;
        } else if(!seenByCamera) return false;
        return true;
    }



    // Makes the player a child of the object (used to make player move with platforms)
    void bindTransform(Transform objectToBindTo) {
        transform.parent = objectToBindTo;
    }



    // Removes any parent transforms
    void unbindTransform() {
        transform.parent = null;
    }
}