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
    public GameObject pauseMenu;
    public Button nextLevelButton;

    static GameManager instance;

    bool gamePlaying;
    bool freePlay;

    float startTime;
    float timeDisplayed;

    int curLevel;

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
        curLevel = 0;
    }



    void onSceneLoad(Scene loadedScene, LoadSceneMode loadSceneMode) {
        if(loadedScene.name == "Main") {
            timeText = FindObjectOfType<Text>();
            startTime = Time.time;
            gamePlaying = true;
            SongManager.addStartTime(startTime);
            SongManager.playSongsDuringLevel();
        }
    }



    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape) && gamePlaying) pause();
        if(gamePlaying) updateTime();
    }



    void updateTime() {
        timeDisplayed = Mathf.Round(levelTime - (Time.time - startTime));
        if(timeDisplayed == 0) endGame(true);
        timeText.text = timeDisplayed.ToString();
    }



    public static void startGame() {
        SceneManager.LoadScene("Main");
    }



    public void unPauseGame() {
        gamePlaying = true;
        pauseMenu.SetActive(false);
    }



    public void pause() {
        gamePlaying = false;
        pauseMenu.SetActive(true);
    }



    public static void endGame(bool success) {
        instance.gamePlaying = false;
        if(success) {
            instance.successUI.SetActive(true);
            if(instance.curLevel >= instance.numLevels - 1) {
                instance.nextLevelButton.enabled = false;
            }
        } else instance.failUI.SetActive(true);
    }



    public static bool isGamePlaying() {
        return instance.gamePlaying;
    }



    public void restart() {
        SceneManager.LoadScene("StartScreen");
        deactivateUI();
    }



    public void replay() {
        SceneManager.LoadScene("Main");
        deactivateUI();
    }



    public static void exitGame() {
        Application.Quit();
    }



    public static void startFreePlay() {
        instance.freePlay = true;
        startGame();
    }



    public void proceedToNextLevel() {
        curLevel++;
        SceneManager.LoadScene("Main");
        deactivateUI();
    }



    static void deactivateUI() {
        instance.successUI.SetActive(false);
        instance.failUI.SetActive(false);
        instance.pauseMenu.SetActive(false);
    }



    public static int getNumLevels() {
        return instance.numLevels;
    }



    public static int getCurLevel() {
        return instance.curLevel;
    }
}