﻿using UnityEngine;

public class CollectableController : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            GameManager.addCollectable();
            Destroy(gameObject);
        }
    }
}