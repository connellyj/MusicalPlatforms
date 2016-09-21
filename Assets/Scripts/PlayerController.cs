using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Transform groundCheck;
    bool jump = false;
    float moveForce = 150f;
    float maxSpeed = 3f;
    float jumpForce = 325f;
    Rigidbody2D rb2d;
    Renderer playerRenderer;
    Transform mostRecentNote;

    void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<Renderer>();
    }
    
    void Update() {
        if(GameManager.isGameStarted() && isPlayerOffscreen()) {
            GameManager.endGame();
            Destroy(gameObject);
        }
        RaycastHit2D noteHit = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Note"));
        if(noteHit) {
            if(mostRecentNote != noteHit.transform) {
                bindTransform(noteHit.transform);
                noteHit.transform.gameObject.GetComponent<MusicNoteController>().playNote();
                mostRecentNote = noteHit.transform;
            }
            if(Input.GetKeyDown(KeyCode.Space)) {
                jump = true;
            }
        } else {
            unbindTransform();
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

    void bindTransform(Transform objectToBindTo) {
        transform.parent = objectToBindTo;
    }

    void unbindTransform() {
        transform.parent = null;
    }
}