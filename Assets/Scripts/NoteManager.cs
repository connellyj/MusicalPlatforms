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
    public GameObject[] collectables;

    int instrumentIndex;

    bool randomNote;

    readonly static float noteSpeedIncrement = 0.01f;
    readonly static float noteDistanceIncrement = 0.2f;

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
                    createNote(randomNoteIndex);
                    if(forward) randomNoteIndex += 3;
                    else randomNoteIndex -= 3;
                }
            }
            yield return new WaitForSeconds(distanceBetweenNotes);
        }
    }



    // Spawns a note and sometimes puts a collectable on top of it
    void createNote(int randomNoteIndex) {
        GameObject curNote = Instantiate(musicNotes[randomNoteIndex]);
        if(!GameManager.isFreePlay() && Random.value < 0.6) {
            GameObject curCollectable = Instantiate(collectables[getInstrumentIndex()], curNote.transform) as GameObject;
            curCollectable.transform.position = curNote.transform.position + Vector3.up * 1.5f;
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
        else {
            if(instance.instrumentIndex == GameManager.getNumLevels()) return Random.Range(0, GameManager.getNumLevels());
            else return instance.instrumentIndex;
        }
    }



    // Returns the current collectable
    public static GameObject getCurrentCollectable() {
        return instance.collectables[GameManager.getCurLevel()];
    }



    // Moves on to the next level
    public static void proceedToNextLevel() {
        instance.noteSpeed += noteSpeedIncrement;
        instance.distanceBetweenNotes -= noteDistanceIncrement;
    }



    // Resets to the state of the first level
    public static void resetLevel() {
        instance.noteSpeed -= noteSpeedIncrement * GameManager.getCurLevel();
        instance.distanceBetweenNotes += noteDistanceIncrement * GameManager.getCurLevel();
    }
}