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

    public void startGame() {
        UIManager.startGame();
        gamePlaying = true;
    }

    public void unPauseGame() {
        gamePlaying = true;
    }

    public static void pause() {
        instance.gamePlaying = false;
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
