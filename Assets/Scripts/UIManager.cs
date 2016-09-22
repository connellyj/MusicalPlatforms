using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {
    
    public Button startButton;
    public GameObject startUI;
    public GameObject pauseMenu;
    public Text timer;

    public float levelTime;

    static UIManager instance;

    float startingTime;
    float timeDisplayed;



    void Awake() {
        instance = this;
        //DontDestroyOnLoad(gameObject);
    }



    void Update() {
        if(GameManager.isGamePlaying()) {
            updateTime();
        }
    }



    // Slides UI Objects on and off screen
    IEnumerator slideOut(GameObject objectToSlide, float speed, int direction) {
        float offscreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 600;
        while(objectToSlide.transform.position.x > offscreen) {
            objectToSlide.transform.position += Vector3.right * direction * speed;
            yield return new WaitForSeconds(1/60);
        }
    }



    // Updates the level time
    void updateTime() {
        timeDisplayed = Mathf.Round(levelTime - (Time.time - startingTime));
        if(timeDisplayed == 0) GameManager.endGame();
        timer.text = timeDisplayed.ToString();
    }



    // Hides the pause menu
    public static void unPause() {
        instance.pauseMenu.SetActive(false);
    }



    // Brings up the pause menu
    public static void pause() {
        instance.pauseMenu.SetActive(true);
    }



    // Starts level time and removes the start screen
    public static void startGame() {
        instance.startingTime = Time.time;
        instance.StartCoroutine(instance.slideOut(instance.startUI, 8f, -1));
    }



    // Shows the ending screen
    public static void endGame() {
    }
}