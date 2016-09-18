using UnityEngine;
using System.Collections;

public class MusicNoteController : MonoBehaviour {

    public Color playedNoteColor;
    public AudioClip note;
    SpriteRenderer spriteRenderer;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playedNoteColor.a = 255;
        StartCoroutine(moveNote());
    }

    IEnumerator moveNote() {
        while(true) {
            if(GameManager.isGameStarted()) {
                transform.position += Vector3.left * NoteManager.getNoteSpeed();
                if (transform.position.x < Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 2) Destroy(gameObject);
            }
            yield return new WaitForSeconds(1 / 40);
        }
    }

    void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.tag == "Player") {
            if (tag == "TrebleClef") {
                NoteManager.activateNote(gameObject);
                GameManager.startGame();
            }
            else if(NoteManager.activateNote(gameObject)) {
                spriteRenderer.color = playedNoteColor;
                AudioSource.PlayClipAtPoint(note, transform.position);
            }
        }
    }

    void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.tag == "Player") other.transform.parent = null;
    }
}