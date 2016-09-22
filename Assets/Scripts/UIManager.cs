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
            StartCoroutine(slideOut(startUI, 8f, -1));
        });
    }

    void Update() {
        if(GameManager.isGameStarted()) {
            timeDisplayed = Mathf.Round(levelTime - (Time.time - startingTime));
            if(timeDisplayed == 0) GameManager.endGame();
            timer.text = timeDisplayed.ToString();
        }
    }

    IEnumerator slideOut(GameObject objectToSlide, float speed, int direction) {
        float offscreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 250;
        while(objectToSlide.transform.position.x > offscreen) {
            objectToSlide.transform.position += Vector3.right * direction * speed;
            yield return new WaitForSeconds(1/60);
        }
    }

    public static void endGame() {
    }
}
