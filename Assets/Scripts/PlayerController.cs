using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Transform groundCheck;
    bool jump = false;
    bool grounded = false;
    float moveForce = 150f;
    float maxSpeed = 3f;
    float jumpForce = 325f;
    Rigidbody2D rb2d;
    Renderer playerRenderer;

    void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<Renderer>();
    }
    
    void Update() {
        if(GameManager.isGameStarted()) {
            if (isPlayerOffscreen()) GameManager.endGame();
            grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Note"));
            if (Input.GetKeyDown(KeyCode.Space) && grounded) jump = true;
        }
    }

    void FixedUpdate() {
        if(GameManager.isGameStarted()) {
            float h = Input.GetAxis("Horizontal");

            if (h < 0.001f && h > -0.001f) {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }

            if (h * rb2d.velocity.x < maxSpeed) {
                rb2d.AddForce(Vector2.right * h * moveForce);
            }

            if (Mathf.Abs(rb2d.velocity.x) > maxSpeed) {
                rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
            }

            if (jump) {
                rb2d.AddForce(new Vector2(0f, jumpForce));
                jump = false;
            }
        }
    }

    bool isPlayerOffscreen() {
        if (playerRenderer.isVisible) return false;
        return true;
    }
}