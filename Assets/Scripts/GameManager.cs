using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    static GameManager instance;
    bool gameStarted;

    void Awake() {
        gameStarted = false;
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void startGame() {
        gameStarted = true;
    }

    public static void endGame() {
        instance.gameStarted = false;
        UIManager.endGame();
        SongManager.playSong();
    }

    public static bool isGameStarted() {
        return instance.gameStarted;
    }

    public void restart() {
        SceneManager.LoadScene(0);
    }
}
