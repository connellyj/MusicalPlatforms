using UnityEngine;
using System.Collections;

public class MusicNoteController : MonoBehaviour {

    public Color playedNoteColor;
    public AudioClip note;
    SpriteRenderer spriteRenderer;
    bool played;

    protected void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playedNoteColor.a = 255;
        played = false;
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

    public void playNote() {
        if(!played) {
            spriteRenderer.color = playedNoteColor;
            if(tag != "TrebleClef") AudioSource.PlayClipAtPoint(note, transform.position);
            played = true;
        }
    }
}