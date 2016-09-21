using UnityEngine;
using System.Collections;

public class MusicNoteController : MonoBehaviour {

    Color[] playedNoteColors;
    public AudioClip note;
    SpriteRenderer spriteRenderer;
    int timesPlayed;

    protected void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playedNoteColors = NoteManager.getPlayedColors();
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
        spriteRenderer.color = playedNoteColors[timesPlayed % playedNoteColors.Length];
        timesPlayed++;
        if(tag != "TrebleClef") {
            AudioSource.PlayClipAtPoint(note, transform.position);
            SongManager.addNote(note, Time.time);
        }
    }
}