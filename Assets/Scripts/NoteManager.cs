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

    int instrumentIndex;

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
        bool forward;
        int randomNoteIndex;
        while (true) {
            if (GameManager.isGamePlaying()) {
                randomNoteIndex = intGenerator.Next(0, musicNotes.Length - 1);
                forward = randomNoteIndex < musicNotes.Length / 2; 
                while (randomNoteIndex < musicNotes.Length && randomNoteIndex >= 0) {
                    Instantiate(musicNotes[randomNoteIndex]);
                    if(forward) randomNoteIndex += 3;
                    else randomNoteIndex -= 3;
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



    // Sets the instrument to be used
    public static void setInstrumentIndex(int index) {
        instance.instrumentIndex = index;
    }



    // Gets the instrument to be used
    public static int getInstrumentIndex() {
        if(!GameManager.isFreePlay()) return GameManager.getCurLevel();
        else return instance.instrumentIndex;
    }
}