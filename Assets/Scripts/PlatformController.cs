/**
 * Written by Julia Connelly, 9/28/2016
 * 
 * A script on a trigger to be attached to the child of a platform
 * Allows the player to jump through the bottom of the platform and land on top
 */

using UnityEngine;

public class PlatformController : MonoBehaviour {

    // Ignores collisons between platform and player if player hits trigger
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            Physics2D.IgnoreCollision(transform.parent.GetComponent<Collider2D>(), other.GetComponent<Collider2D>());
        }
    }

    // Stops ignoring collsions once the player leaves the trigger
    void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player") {
            Physics2D.IgnoreCollision(transform.parent.GetComponent<Collider2D>(), other.GetComponent<Collider2D>(), false);
        }
    }
}