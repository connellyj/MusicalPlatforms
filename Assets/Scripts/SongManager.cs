/**
 * Written by Julia Connelly, 9/28/2016
 * 
 * Holds, creates, and plays the song created during the game
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SongManager : MonoBehaviour {

    List<AudioClip>[] songs;
    List<float>[] time;

    static SongManager instance;



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
        int numLevels = GameManager.getNumLevels();
        songs = new List<AudioClip>[numLevels];
        time = new List<float>[numLevels];
    }



    // Records a note in the song
    public static void addNote(AudioClip clip, float timePlayed) {
        if(GameManager.isGamePlaying()) {
            int index = GameManager.getCurLevel();
            if(instance.songs[index] == null) {
                instance.songs[index] = new List<AudioClip>();
            }
            if(instance.time[index] == null) {
                instance.time[index] = new List<float>();
            }
            instance.songs[index].Add(clip);
            instance.time[index].Add(timePlayed);
        }
    }



    // Starts playing the song
    public void playSong() {
        for(int i = 0; i < GameManager.getCurLevel() + 1; i ++) StartCoroutine(startSong(i, true));
    }



    public static void playSongsDuringLevel() {
        for(int i = 0; i < GameManager.getCurLevel(); i++) instance.StartCoroutine(instance.startSong(i, false));
    }



    // Plays the song
    IEnumerator startSong(int songToStart, bool endOfLevel) {
        List<float> curSongTime = time[songToStart];
        curSongTime.Add(0f);
        for(int i = 0; i < songs[songToStart].Count; i ++) {
            while(!endOfLevel && !GameManager.isGamePlaying()) yield return null;
            AudioSource.PlayClipAtPoint(songs[songToStart][i], transform.position);
            yield return new WaitForSeconds(Mathf.Abs(curSongTime[i] - curSongTime[i + 1]));
        }
        yield return null;
    }
}
