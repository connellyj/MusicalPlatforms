/**
 * Written by Julia Connelly, 9/28/2016
 * 
 * Spawns notes ands stores information relevant to all notes
 */

using UnityEngine;
using System.Collections;

public class NoteManager : MonoBehaviour {

    public float noteSpeed;
    public float distanceBetweenNotes;

    public GameObject[] musicNotes;
    public Color[] playedNoteColors;

    static NoteManager instance;



    // Make sure there is only one NoteManager in each scene
    void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if(instance != this) {
            Destroy(gameObject);
            return;
        }
    }



    void Start() {
        StartCoroutine(spawnNotes());
    }



    // Creates notes randomly
    IEnumerator spawnNotes() {
        System.Random intGenerator = new System.Random();
        int randomNoteIndex = 0;
        while (true) {
            if (GameManager.isGamePlaying()) {
                randomNoteIndex = intGenerator.Next(0, musicNotes.Length - 1);
                while (randomNoteIndex < musicNotes.Length) {
                    Instantiate(musicNotes[randomNoteIndex]);
                    randomNoteIndex += 3;
                }
            }
            yield return new WaitForSeconds(distanceBetweenNotes);
        }
    }



    // Note speed for all the notes
    public static float getNoteSpeed() {
        return instance.noteSpeed;
    }



    // Played colors for all the notes
    public static Color[] getPlayedColors() {
        return instance.playedNoteColors;
    }
}