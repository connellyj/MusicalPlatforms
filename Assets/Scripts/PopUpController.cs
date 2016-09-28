using UnityEngine;
using UnityEngine.UI;

public class PopUpController : MonoBehaviour {

    public Button unPauseButton;
    public Button mainMenuButton;
    public Button restartLevelButton;
    public Button nextLevelButton;
    public Button playSongButton;
    public Text messageText;

    void Start() {
        if(unPauseButton != null) {
            unPauseButton.onClick.AddListener(() => {
                GameManager.unPauseGame();
                Destroy(gameObject);
            });
        }
        mainMenuButton.onClick.AddListener(() => GameManager.goToMainMenu());
        restartLevelButton.onClick.AddListener(() => GameManager.restartLevel());
        if(nextLevelButton != null) nextLevelButton.onClick.AddListener(() => GameManager.proceedToNextLevel());
        if(playSongButton != null) playSongButton.onClick.AddListener(() => SongManager.playSong());
    }

    public void setMessage(string newMessage) {
        messageText.text = newMessage;
    }
}
