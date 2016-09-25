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
    public GameObject freePlayUI;
    public Text winMessage;
    public Button nextLevelButton;
    public Button pianoButton;
    public Button violinButton;
    public Button fluteButton;

    static GameManager instance;

    bool gamePlaying;
    bool freePlay;

    float startTime;
    float timeDisplayed;
    float timePaused;

    int curLevel;

    Text timeText;



    // Makes sure there is only one GameManager in each scene
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



    // Gets called when a scene is finished loading
    // Initializes things for the level
    void onSceneLoad(Scene loadedScene, LoadSceneMode loadSceneMode) {
        if(loadedScene.name == "Main") {
            if(!freePlay) {
                initTime();
                gamePlaying = true;
            }else {
                freePlayUI.SetActive(true);
                pianoButton.onClick.AddListener(() => {
                    NoteManager.setInstrumentIndex(0);
                    freePlayUI.SetActive(false);
                    gamePlaying = true;
                });
                violinButton.onClick.AddListener(() => {
                    NoteManager.setInstrumentIndex(1);
                    freePlayUI.SetActive(false);
                    gamePlaying = true;
                });
                fluteButton.onClick.AddListener(() => {
                    NoteManager.setInstrumentIndex(2);
                    freePlayUI.SetActive(false);
                    gamePlaying = true;
                });
            }
            initSong();
        }
    }



    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape) && gamePlaying) pause();
        if(gamePlaying && !freePlay) updateTime();
    }



    // Initializes the song for the level
    void initSong() {
        SongManager.addStartTime(startTime);
        SongManager.playSongsDuringLevel();
    }



    // Initializes the time for the level
    void initTime() {
        timeText = FindObjectOfType<Text>();
        startTime = Time.time;
    }



    // Updates the time text and ends the game if it's zero
    void updateTime() {
        timeDisplayed = Mathf.Round(levelTime - (Time.time - startTime));
        if(timeDisplayed == 0) endGame(true);
        timeText.text = timeDisplayed.ToString();
    }



    // Loads the game scene
    public static void startGame() {
        SceneManager.LoadScene("Main");
    }



    // Unpauses the game by updating the time and hiding the pause menu
    public void unPauseGame() {
        gamePlaying = true;
        timePaused = Time.time - timePaused;
        startTime += timePaused;
        pauseMenu.SetActive(false);
    }



    // Brings up the pause menu
    public void pause() {
        gamePlaying = false;
        timePaused = Time.time;
        pauseMenu.SetActive(true);
    }



    // Activates the relevant UI (failure or success) at the end of a level
    public static void endGame(bool success) {
        instance.gamePlaying = false;
        if(success) {
            instance.successUI.SetActive(true);
            if(instance.curLevel >= instance.numLevels - 1) {
                instance.nextLevelButton.gameObject.SetActive(false);
                instance.winMessage.text = "You win!";
            }else {
                instance.nextLevelButton.gameObject.SetActive(true);
            }
        } else instance.failUI.SetActive(true);
    }



    // Returns the game state
    public static bool isGamePlaying() {
        return instance.gamePlaying;
    }



    // Opens the start menu
    public void restart() {
        SceneManager.LoadScene("StartScreen");
        SongManager.stopSongs();
        deactivateUI();
    }



    // Reloads the current level
    public void replay() {
        SceneManager.LoadScene("Main");
        SongManager.stopSongs();
        deactivateUI();
    }



    // Quits the game
    public static void exitGame() {
        Application.Quit();
    }



    // Starts the game as free play
    public static void startFreePlay() {
        instance.freePlay = true;
        startGame();
    }



    // Moves to the next level
    public void proceedToNextLevel() {
        curLevel++;
        replay();
    }



    // Deactivates all UI Menus
    static void deactivateUI() {
        instance.successUI.SetActive(false);
        instance.failUI.SetActive(false);
        instance.pauseMenu.SetActive(false);
    }



    // Returns the number of levels
    public static int getNumLevels() {
        return instance.numLevels;
    }



    // Returns the current level
    public static int getCurLevel() {
        return instance.curLevel;
    }



    // Returns whether or not currently in free play
    public static bool isFreePlay() {
        return instance.freePlay;
    }
}