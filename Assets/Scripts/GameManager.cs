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
    public GameObject successUIPrefab;
    public GameObject failUIPrefab;
    public GameObject pauseMenuPrefab;
    public GameObject freePlayUIPrefab;
    public GameObject timeTextPrefab;
    public GameObject numCollectedTextPrefab;

    Text timeText;
    Text numCollectedText;

    readonly static int numCollectIncrement = 5;

    static GameManager instance;

    bool gamePlaying;
    bool freePlay;

    float startTime;
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
        startTime = 0f;
        timePaused = 0f;
        curLevel = 0;
        numCollected = 0;
    }



    void Update() {
        if(gamePlaying) {
            if(Input.GetKeyDown(KeyCode.Escape)) pause();
            if(!freePlay) updateTime();
        }
    }



    // Updates the time text and ends the game if it's zero
    void updateTime() {
        float timeDisplayed = Mathf.Round(levelTime - (Time.time - startTime));
        if(timeDisplayed == 0) {
            if(numCollected >= numToCollect) endGame(true);
            else endGame(false);
        }
        timeText.text = timeDisplayed.ToString();
    }



    // Loads the game scene
    public static void startNormalGame() {
        SceneManager.LoadScene("Main");
    }



    // Starts the game as free play
    public static void startFreePlayMode() {
        instance.freePlay = true;
        Instantiate(instance.freePlayUIPrefab);
    }



    // Called by Unity when a scene is loaded
    void onSceneLoad(Scene sceneLoaded, LoadSceneMode loadSceneMode) {
        if(sceneLoaded.name == "Main") startGame();
    }



    // Initializes  and starts the game
    void startGame() {
        startTime = Time.time;
        numCollected = 0;
        if(!freePlay) initUI();
        initSong();
        gamePlaying = true;
    }



    // Initializes any required UI components
    void initUI() {
        GameObject tmpCanvas = Instantiate(numCollectedTextPrefab) as GameObject;
        numCollectedText = tmpCanvas.transform.GetChild(0).GetComponent<Text>();
        numCollectedText.text = numCollected + " / " + numToCollect;
        tmpCanvas = Instantiate(timeTextPrefab) as GameObject;
        timeText = tmpCanvas.transform.GetChild(0).GetComponent<Text>();
        initCollectableSprite();
    }



    // Initializes the sprite of the object currently being collected
    void initCollectableSprite() {
        GameObject collectable = Instantiate(NoteManager.getCurrentCollectable()) as GameObject;
        collectable.transform.localScale *= 2;
        Destroy(collectable.GetComponent<Collider2D>());
    }



    // Initializes the song for the level
    void initSong() {
        SongManager.addStartTime(startTime);
        SongManager.playSongsDuringLevel();
    }



    // Unpauses the game by updating the time and hiding the pause menu
    public static void unPauseGame() {
        instance.timePaused = Time.time - instance.timePaused;
        instance.startTime += instance.timePaused;
        instance.gamePlaying = true;
    }



    // Brings up the pause menu
    public void pause() {
        gamePlaying = false;
        timePaused = Time.time;
        Instantiate(pauseMenuPrefab);
    }



    // Activates the relevant UI (failure or success) at the end of a level
    public static void endGame(bool success) {
        instance.gamePlaying = false;
        if(success) {
            instance.activateSuccessfulUI();
        } else {
            instance.activateFailureUI();
        }
    }



    // Activates the failure UI
    void activateFailureUI() {
        SongManager.stopSongs();
        PopUpController failUI = (Instantiate(failUIPrefab) as GameObject).GetComponent<PopUpController>();
        if(freePlay) failUI.setMessage("Uh-oh, you fell!");
    }



    // Activates the successful UI
    void activateSuccessfulUI() {
        SongManager.stopSongs();
        PopUpController successUI = (Instantiate(successUIPrefab) as GameObject).GetComponent<PopUpController>();
        if(curLevel >= numLevels - 1) {
            successUI.nextLevelButton.gameObject.SetActive(false);
            successUI.setMessage("You win!");
        }
    }



    // Opens the start menu
    public static void goToMainMenu() {
        instance.freePlay = false;
        instance.resetLevel();
        SongManager.stopSongs();
        SongManager.resetAllSongs();
        SceneManager.LoadScene("StartScreen");
    }



    // Resets the game so it starts at the first level again
    void resetLevel() {
        numToCollect -= numCollectIncrement * curLevel;
        NoteManager.resetLevel();
        curLevel = 0;
    }



    // Reloads the current level
    public static void restartLevel() {
        SongManager.stopSongs();
        SongManager.resetCurrentSong();
        startNormalGame();
    }



    // Moves to the next level
    public static void proceedToNextLevel() {
        instance.curLevel++;
        instance.numToCollect += numCollectIncrement;
        NoteManager.proceedToNextLevel();
        restartLevel();
    }



    // Quits the game
    public static void exitGame() {
        Application.Quit();
    }



    // Returns the number of levels
    public static int getNumLevels() {
        return instance.numLevels;
    }



    // Returns the current level
    public static int getCurLevel() {
        return instance.curLevel;
    }



    // Returns the game state
    public static bool isGamePlaying() {
        return instance.gamePlaying;
    }



    // Returns whether or not currently in free play
    public static bool isFreePlay() {
        return instance.freePlay;
    }



    // Adds one more to the collected score
    public static void addCollectable() {
        if(instance.numCollected < instance.numToCollect) instance.numCollected++;
        instance.numCollectedText.text = instance.numCollected + " / " + instance.numToCollect;
    }
}