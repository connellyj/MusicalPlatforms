using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {

    public Button startButton;
    public Button exitButton;
    public Button freePlayButton;

    void Start() {
        startButton.onClick.AddListener(() => GameManager.startGame());
        exitButton.onClick.AddListener(() => GameManager.exitGame());
        freePlayButton.onClick.AddListener(() => GameManager.startFreePlay());
    }
}
