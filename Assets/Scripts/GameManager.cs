/**
 * Written by Julia Connelly, 9/28/2016
 * 
 * Controls overall game state
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    static GameManager instance;

    bool gamePlaying;



    void Awake() {
        gamePlaying = false;
        instance = this;
        DontDestroyOnLoad(gameObject);
    }



    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) pause();
    }



    public void startGame() {
        UIManager.startGame();
        gamePlaying = true;
    }



    public void unPauseGame() {
        gamePlaying = true;
        UIManager.unPause();
    }



    public static void pause() {
        instance.gamePlaying = false;
        UIManager.pause();
    }



    public static void endGame() {
        instance.gamePlaying = false;
        UIManager.endGame();
        SongManager.playSong();
    }



    public static bool isGamePlaying() {
        return instance.gamePlaying;
    }



    public void restart() {
        SceneManager.LoadScene(0);
    }
}
