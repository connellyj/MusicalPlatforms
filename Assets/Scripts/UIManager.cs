using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {
    
    public Button startButton;
    public GameObject startUI;
    public Text timer;
    public float levelTime;

    static UIManager instance;

    float startingTime;
    float timeDisplayed;

    void Awake() {
        instance = this;
        DontDestroyOnLoad(this);
    }

    void Start() {
        startButton.onClick.AddListener(() => {
            startingTime = Time.time;
            GameManager.startGame();
            startUI.SetActive(false);
        });
    }

    void Update() {
        if(GameManager.isGameStarted()) {
            timeDisplayed = Mathf.Round(levelTime - (Time.time - startingTime));
            if(timeDisplayed == 0) GameManager.endGame();
            timer.text = timeDisplayed.ToString();
        }
    }

    IEnumerator slideOut(Canvas canvasToSlide, float time, int direction) {
        while(time > 0) {
            foreach(Transform child in canvasToSlide.transform) {
                child.position += Vector3.right * direction * 10;
            }
            yield return new WaitForSeconds(1/60);
        }
    }

    public static void endGame() {
    }
}
