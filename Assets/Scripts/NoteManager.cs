using UnityEngine;
using System.Collections;

public class NoteManager : MonoBehaviour {

    static NoteManager instance;
    public float noteSpeed;
    public float distanceBetweenNotes;
    public GameObject[] musicNotes;
    public GameObject player;
    public GameObject groundCheck;

    void Awake() {
        instance = this;
    }

    void Start() {
        StartCoroutine(spawnNotes());
    }

    IEnumerator spawnNotes() {
        System.Random intGenerator = new System.Random();
        int randomNoteIndex = 0;
        while (true) {
            if (GameManager.isGameStarted()) {
                randomNoteIndex = intGenerator.Next(0, musicNotes.Length - 1);
                while (randomNoteIndex < musicNotes.Length) {
                    Instantiate(musicNotes[randomNoteIndex]);
                    randomNoteIndex += 3;
                }
            }
            yield return new WaitForSeconds(distanceBetweenNotes);
        }
    }

    public static float getNoteSpeed() {
        return instance.noteSpeed;
    }
}