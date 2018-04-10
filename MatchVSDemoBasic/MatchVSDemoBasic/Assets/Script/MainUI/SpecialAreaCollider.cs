using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAreaCollider : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) 
    {
        CharacterMove move = other.gameObject.GetComponent<CharacterMove>();
        move.EnterSpecial();
    }

    void OnTriggerExit2D(Collider2D other) { 

        CharacterMove move = other.gameObject.GetComponent<CharacterMove>();
        move.ExitSpecial();
    }
}
