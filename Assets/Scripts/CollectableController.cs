using UnityEngine;

public class CollectableController : MonoBehaviour {

    public Sprite[] sprites;

    SpriteRenderer spriteRenderer;



    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(GameManager.isRandomFreePlay()) {
            spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        } else {
            spriteRenderer.sprite = sprites[NoteManager.getInstrumentIndex()];
        }
    }



    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            GameManager.addCollectable();
            Destroy(gameObject);
        }
    }
}
