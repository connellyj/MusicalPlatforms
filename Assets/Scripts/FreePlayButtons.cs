using UnityEngine;
using UnityEngine.UI;

public class FreePlayButtons : MonoBehaviour {

    public Button[] buttons;

    void Start() {
        buttons[0].onClick.AddListener(() => {
            NoteManager.setInstrumentIndex(0);
            GameManager.startNormalGame();
        });
        buttons[1].onClick.AddListener(() => {
            NoteManager.setInstrumentIndex(1);
            GameManager.startNormalGame();
        });
        buttons[2].onClick.AddListener(() => {
            NoteManager.setInstrumentIndex(2);
            GameManager.startNormalGame();
        });
        buttons[3].onClick.AddListener(() => {
            NoteManager.setInstrumentIndex(3);
            GameManager.startNormalGame();
        });
    }
}
