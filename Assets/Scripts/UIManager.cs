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
        DontDestroyOnLoad(gameObject);
    }

    void Update() {
        if(GameManager.isGamePlaying()) {
            pauseMenu.SetActive(false);
            timeDisplayed = Mathf.Round(levelTime - (Time.time - startingTime));
            if(timeDisplayed == 0) GameManager.pause();
            timer.text = timeDisplayed.ToString();
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            GameManager.pause();
            pauseMenu.SetActive(true);
        }
    }

    IEnumerator slideOut(GameObject objectToSlide, float speed, int direction) {
        float offscreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 250;
        while(objectToSlide.transform.position.x > offscreen) {
            objectToSlide.transform.position += Vector3.right * direction * speed;
            yield return new WaitForSeconds(1/60);
        }
    }

    public static void startGame() {
        instance.startingTime = Time.time;
        instance.StartCoroutine(instance.slideOut(instance.startUI, 8f, -1));
    }

    public static void endGame() {
    }
}
