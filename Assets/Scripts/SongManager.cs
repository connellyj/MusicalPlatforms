/**
 * Written by Julia Connelly, 9/28/2016
 * 
 * Holds, creates, and plays the song created during the game
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SongManager : MonoBehaviour {

    Queue<AudioClip> song;
    Queue<float> time;

    static SongManager instance;



    void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }



    void Start() {
        song = new Queue<AudioClip>();
        time = new Queue<float>();
    }



    // Records a note in the song
    public static void addNote(AudioClip clip, float timePlayed) {
        if(GameManager.isGamePlaying()) {
            instance.song.Enqueue(clip);
            instance.time.Enqueue(timePlayed);
        }
    }



    // Starts playing the song
    public static void playSong() {
        instance.StartCoroutine(instance.startSong());
    }



    // Plays the song
    IEnumerator startSong() {
        time.Enqueue(0f);
        foreach(AudioClip clip in instance.song) {
            AudioSource.PlayClipAtPoint(clip, transform.position);
            yield return new WaitForSeconds(Mathf.Abs(time.Dequeue() - time.Peek()));
        }
        yield return null;
    }
}
