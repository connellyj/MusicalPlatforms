/**
 * Written by Julia Connelly, 9/28/2016
 * 
 * Adds functionality to the start screen buttons
 */

using UnityEngine;
using UnityEngine.UI;

public class StartScreenButtons : MonoBehaviour {

    public Button startButton;
    public Button exitButton;
    public Button freePlayButton;

    void Start() {
        startButton.onClick.AddListener(() => GameManager.startNormalGame());
        exitButton.onClick.AddListener(() => GameManager.exitGame());
        freePlayButton.onClick.AddListener(() => GameManager.startFreePlayMode());
    }
}
