using UnityEngine;
using System.Collections;

//PLAY AGAIN WITH PREVIOUS AUDIO IN BACKGROUND
//ADD AN OPTIONAL/UP BEAT
//ONLY PLAY NOTE ONCE, CHANGES TO BLACK AGAIN AFTER A SECOND OR 2
//CANVAS SLIDING SHOULD BE ABLE TO TAKE A TIME AND DESTROY THE CANVAS ONCE OFFSCREEN

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

    public static bool activateNote(GameObject note) {
        if (Physics2D.Linecast(instance.player.transform.position, instance.groundCheck.transform.position, 1 << LayerMask.NameToLayer("Note"))) {
            instance.player.transform.parent = note.transform;
            return true;
        }
        return false;
    }
}