using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    public GameObject startingPlatform;
    public Button startButton;
    public GameObject startUI;

    void Start() {
        startButton.onClick.AddListener(() => {
            StartCoroutine(slideOut(startUI, 3f, -1));
            GameManager.startGame();
        });
    }

    IEnumerator slideOut(GameObject canvasToSlide, float time, int direction) {
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
