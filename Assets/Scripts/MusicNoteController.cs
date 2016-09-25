/**
 * Written by Julia Connelly, 9/28/2016
 * 
 * Moves individual notes and handles their audio
 */

using UnityEngine;
using System.Collections;

public class MusicNoteController : MonoBehaviour {

    public AudioClip[] notes;

    int timesPlayed;
    int instrumentIndex;

    Color[] playedNoteColors;
    SpriteRenderer spriteRenderer;



    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playedNoteColors = NoteManager.getPlayedColors();
        instrumentIndex = NoteManager.getInstrumentIndex();
        StartCoroutine(moveNote());
    }



    // Moves the note and destroys it once offscreen
    IEnumerator moveNote() {
        float offscreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 2;
        while(true) {
            if(GameManager.isGamePlaying()) {
                transform.position += Vector3.left * NoteManager.getNoteSpeed();
                if(transform.position.x < offscreen) {
                    Destroy(gameObject);
                }
            }
            yield return new WaitForSeconds(1 / 40);
        }
    }



    // Plays the audio, saves the note in the song, and changes the note color
    public void playNote() {
        spriteRenderer.color = playedNoteColors[timesPlayed % playedNoteColors.Length];
        timesPlayed++;
        if(tag != "TrebleClef") {
            AudioSource.PlayClipAtPoint(notes[instrumentIndex], transform.position, SongManager.getCurVolume());
            SongManager.addNote(notes[instrumentIndex], Time.time);
        }
    }



    public void setInstrumentIndex(int index) {
        instrumentIndex = index;
    }
}