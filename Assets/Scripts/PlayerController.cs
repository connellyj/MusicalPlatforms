/**
 * Written by Julia Connelly, 9/28/2016
 * 
 * Controls the players movement and interaction
 */

using UnityEngine;

public class PlayerController : MonoBehaviour {

    // A child of the player object that's below the player
    public Transform groundCheck;

    // Track whether or not the jump button(s) were clicked
    bool jump = false;
    bool babyJump = false;

    // Force limits and values
    float moveForce = 150f;
    float maxSpeed = 3f;
    float jumpForce = 325f;
    float babyJumpForce = 105f;

    Rigidbody2D playerRigidbody;
    Renderer playerRenderer;
    Transform mostRecentNote;

    void Awake() {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<Renderer>();
    }
    
    void Update() {
        if(!GameManager.isGamePlaying()) {
            playerRigidbody.Sleep();
        }
        if(GameManager.isGamePlaying()) {
            if(isPlayerOffscreen()) GameManager.endGame();
            else {
                RaycastHit2D noteHit = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Note"));
                if(noteHit) {
                    if(mostRecentNote != noteHit.transform) {
                        bindTransform(noteHit.transform);
                        noteHit.transform.gameObject.GetComponent<MusicNoteController>().playNote();
                    }
                    if(Input.GetKeyDown(KeyCode.Space)) {
                        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) babyJump = true;
                        else jump = true;
                    }
                } else {
                    unbindTransform();
                }
                mostRecentNote = noteHit.transform;
            }
        }
    }

    void FixedUpdate() {
        if(GameManager.isGamePlaying()) {
            float h = Input.GetAxis("Horizontal");

            if (h < 0.001f && h > -0.001f) {
                playerRigidbody.velocity = new Vector2(0, playerRigidbody.velocity.y);
            }

            if (h * playerRigidbody.velocity.x < maxSpeed) {
                playerRigidbody.AddForce(Vector2.right * h * moveForce);
            }

            if (Mathf.Abs(playerRigidbody.velocity.x) > maxSpeed) {
                playerRigidbody.velocity = new Vector2(Mathf.Sign(playerRigidbody.velocity.x) * maxSpeed, playerRigidbody.velocity.y);
            }

            if (jump) {
                playerRigidbody.AddForce(new Vector2(0f, jumpForce));
                jump = false;
            }

            if(babyJump) {
                playerRigidbody.AddForce(new Vector2(0f, babyJumpForce));
                babyJump = false;
            }
        }

    }

    bool isPlayerOffscreen() {
        if (playerRenderer.isVisible) return false;
        return true;
    }

    void bindTransform(Transform objectToBindTo) {
        transform.parent = objectToBindTo;
    }

    void unbindTransform() {
        transform.parent = null;
    }
}