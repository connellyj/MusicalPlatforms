/**
 * Written by Julia Connelly, 9/28/2016
 * 
 * Controls overall game state
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public float levelTime;
    public int numLevels;
    public GameObject successUI;
    public GameObject failUI;
    public Button nextLevelButton;

    static GameManager instance;

    bool gamePlaying;
    bool freePlay;

    float startTime;
    float timeDisplayed;

    Text timeText;



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
        SceneManager.sceneLoaded += onSceneLoad;
        gamePlaying = false;
        freePlay = false;
    }



    void onSceneLoad(Scene loadedScene, LoadSceneMode loadSceneMode) {
        if(loadedScene.name == "Main") {
            timeText = FindObjectOfType<Text>();
            startTime = Time.time;
            gamePlaying = true;
        }
    }



    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) pause();
        if(gamePlaying) updateTime();
    }



    void updateTime() {
        timeDisplayed = Mathf.Round(levelTime - (Time.time - startTime));
        if(timeDisplayed == 0) endGame(true);
        timeText.text = timeDisplayed.ToString();
    }



    public void startGame() {
        SceneManager.LoadScene("Main");
    }



    public void unPauseGame() {
        gamePlaying = true;
    }



    public static void pause() {
        instance.gamePlaying = false;
    }



    public static void endGame(bool success) {
        instance.gamePlaying = false;
        if(success) {
            instance.successUI.SetActive(true);
            if(NoteManager.getInstrumentIndex() >= instance.numLevels - 1) {
                instance.nextLevelButton.enabled = false;
            }
        } else instance.failUI.SetActive(true);
    }



    public static bool isGamePlaying() {
        return instance.gamePlaying;
    }



    public void restart() {
        SceneManager.LoadScene(0);
        deactivateUI();
    }



    public void replay() {
        SceneManager.LoadScene("Main");
        deactivateUI();
    }



    public void exitGame() {
        Application.Quit();
    }



    public void startFreePlay() {
        freePlay = true;
        startGame();
    }



    public void proceedToNextLevel() {
        NoteManager.proceedToNextLevel();
        SceneManager.LoadScene("Main");
        deactivateUI();
    }



    static void deactivateUI() {
        instance.successUI.SetActive(false);
        instance.failUI.SetActive(false);
    }
}
