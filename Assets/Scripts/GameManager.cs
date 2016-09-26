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
    public int numToCollect;
    public GameObject successUI;
    public GameObject failUI;
    public GameObject pauseMenu;
    public GameObject freePlayUI;
    public Text winMessage;
    public Text failMessage;
    public Text timeText;
    public Button nextLevelButton;
    public Button pianoButton;
    public Button violinButton;
    public Button fluteButton;
    public Button randomButton;

    static GameManager instance;

    bool gamePlaying;
    bool freePlay;
    bool randomFreePlay;

    float startTime;
    float timeDisplayed;
    float timePaused;

    int curLevel;
    int numCollected;



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
        randomFreePlay = false;
        curLevel = 0;
        numCollected = 0;
        initFreePlayButtons();
    }



    // Gets called when a scene is finished loading
    // Initializes things for the level
    void onSceneLoad(Scene loadedScene, LoadSceneMode loadSceneMode) {
        if(loadedScene.name == "Main") {
            if(!freePlay) {
                initGame();
            }else {
                freePlayUI.SetActive(true);
            }
        }
    }



    // Initializes the buttons for the free play menu
    void initFreePlayButtons() {
        pianoButton.onClick.AddListener(() => {
            NoteManager.setInstrumentIndex(0);
            initGame();
        });
        violinButton.onClick.AddListener(() => {
            NoteManager.setInstrumentIndex(1);
            initGame();
        });
        fluteButton.onClick.AddListener(() => {
            NoteManager.setInstrumentIndex(2);
            initGame();
        });
        randomButton.onClick.AddListener(() => {
            randomFreePlay = true;
            initGame();
        });
    }



    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape) && gamePlaying) pause();
        if(gamePlaying && !freePlay) updateTime();
        if(gamePlaying) deactivateUI();
    }



    // Initializes  and starts the game
    void initGame() {
        initTime();
        initSong();
        gamePlaying = true;
    }



    // Initializes the song for the level
    void initSong() {
        SongManager.addStartTime(startTime);
        SongManager.playSongsDuringLevel();
    }



    // Initializes the time for the level
    void initTime() {
        startTime = Time.time;
        timeText.gameObject.SetActive(true);
    }



    // Updates the time text and ends the game if it's zero
    void updateTime() {
        timeDisplayed = Mathf.Round(levelTime - (Time.time - startTime));
        if(timeDisplayed == 0) endGame(true);
        timeText.text = timeDisplayed.ToString();
    }



    // Loads the game scene
    public static void startGame() {
        resetScene();
        SongManager.resetCurrentSong();
        SceneManager.LoadScene("Main");
    }



    // Unpauses the game by updating the time and hiding the pause menu
    public void unPauseGame() {
        timePaused = Time.time - timePaused;
        startTime += timePaused;
        pauseMenu.SetActive(false);
        gamePlaying = true;
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
            activateSuccessfulUI();
        } else {
            activateFailureUI();
        }
    }



    // Activates the failure UI
    static void activateFailureUI() {
        if(isFreePlay()) instance.failMessage.text = "Uh-oh, you fell!";
        else instance.failMessage.text = "Level failed!";
        instance.failUI.SetActive(true);
    }



    // Activates the successful UI
    static void activateSuccessfulUI() {
        if(instance.curLevel >= instance.numLevels - 1) {
            instance.nextLevelButton.gameObject.SetActive(false);
            instance.winMessage.text = "You win!";
        } else {
            instance.nextLevelButton.gameObject.SetActive(true);
            instance.winMessage.text = "Level Completed!";
        }
        instance.successUI.SetActive(true);
    }



    // Returns the game state
    public static bool isGamePlaying() {
        return instance.gamePlaying;
    }



    // Opens the start menu
    public void restart() {
        resetScene();
        freePlay = false;
        randomFreePlay = false;
        SongManager.resetAllSongs();
        SceneManager.LoadScene("StartScreen");
    }



    // Reloads the current level
    public void replay() {
        startGame();
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
        startGame();
    }



    // Deactivates all UI Menus
    static void deactivateUI() {
        instance.successUI.SetActive(false);
        instance.failUI.SetActive(false);
        instance.pauseMenu.SetActive(false);
        instance.freePlayUI.SetActive(false);
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



    // Returns whether or not the free play is random
    public static bool isRandomFreePlay() {
        return instance.randomFreePlay;
    }



    // Resets the UI and stops the music
    static void resetScene() {
        deactivateUI();
        SongManager.stopSongs();
        instance.timeText.gameObject.SetActive(false);
        instance.numCollected = 0;
    }



    // Adds one more to the collected score
    public static void addCollectable() {
        instance.numCollected++;
    }
}