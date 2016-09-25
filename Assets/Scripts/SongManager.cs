/**
 * Written by Julia Connelly, 9/28/2016
 * 
 * Holds, creates, and plays the song created during the game
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SongManager : MonoBehaviour {

    public float[] volume;

    List<AudioClip>[] songs;
    List<float>[] time;

    int songsPlaying;

    static SongManager instance;



    // Makes sure there is only one SongManager in each scene
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
        songsPlaying = 0;
    }



    // Records a note in the song for the current level
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
        if(songsPlaying == 0) for(int i = 0; i < GameManager.getCurLevel() + 1; i++) StartCoroutine(startSong(i, true));
    }



    // Plays the songs during the level
    public static void playSongsDuringLevel() {
        for(int i = 0; i < GameManager.getCurLevel(); i++) instance.StartCoroutine(instance.startSong(i, false));
    }



    // Adds the time before the first note is played to the song
    public static void addStartTime(float startTime) {
        int index = GameManager.getCurLevel();
        instance.time[index] = new List<float>();
        instance.time[index].Add(startTime);
    }



    // Gets the volume for the current instrument
    public static float getCurVolume() {
        return instance.volume[GameManager.getCurLevel()];
    }



    // Plays the song
    IEnumerator startSong(int songToStart, bool endOfLevel) {
        songsPlaying++;
        List<float> curSongTime = time[songToStart];
        if(songs[songToStart] != null) {
            for(int i = 0; i < songs[songToStart].Count; i ++) {
                // Essentially waits for the game to be unpaused
                while(!endOfLevel && !GameManager.isGamePlaying()) yield return null;

                // Provides the break between notes
                yield return new WaitForSeconds(Mathf.Abs(curSongTime[i] - curSongTime[i + 1]));

                // Plays the note
                AudioSource.PlayClipAtPoint(songs[songToStart][i], transform.position, volume[songToStart]);
            }
        }
        songsPlaying--;
    }



    // Stops playing the song
    public static void stopSongs() {
        instance.StopAllCoroutines();
        instance.songsPlaying = 0;
    }



    // Resets the current song
    public static void resetCurrentSong() {
        int index = GameManager.getCurLevel();
        instance.songs[index] = new List<AudioClip>();
        instance.time[index] = new List<float>();
    }



    // Resets all songs
    public static void resetAllSongs() {
        int numLevels = GameManager.getNumLevels();
        instance.songs = new List<AudioClip>[numLevels];
        instance.time = new List<float>[numLevels];
    }
}