using UnityEngine;

public class GameManager : MonoBehaviour {

    static GameManager instance;
    bool gameStarted;

    void Awake() {
        gameStarted = false;
        instance = this;
        DontDestroyOnLoad(this);
    }

    public static void startGame() {
        instance.gameStarted = true;
    }

    public static void endGame() {
        instance.gameStarted = false;
        UIManager.endGame();
        SongManager.playSong();
    }

    public static bool isGameStarted() {
        return instance.gameStarted;
    }
}
