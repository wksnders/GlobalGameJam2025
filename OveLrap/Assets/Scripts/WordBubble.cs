using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordBubble : MonoBehaviour
{
    public Word word;
    public TextMeshPro text;
    public MeshRenderer renderer;
    public BubbleColor currentColor;

    bool SetupDone = false;
    //public bool IsColliding { get; private set; }
    public bool IsColliding = false;

    public void SetWord(Word word) {
        this.word = word;
        text.text = word.word;
        SetupDone = true;
    }

    public void SetColor(BubbleColor color) {
        currentColor = color;
        renderer.material.color = currentColor.color;
    }

    // Point text toward camera
    void Update() {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }

    private void OnTriggerEnter(Collider other) {
        // if colliding with another bubble, set IsColliding to true
        if (other.gameObject.GetComponent<WordBubble>() != null) {
            IsColliding = true;
            if(SetupDone) {
                // only color change after word is set, this is to prevent color spread when first spawning bubbles
                HandleColorChange(other.gameObject.GetComponent<WordBubble>());
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        // if colliding with another bubble, set IsColliding to true
        if (other.gameObject.GetComponent<WordBubble>() != null) {
            IsColliding = true;
            if(SetupDone) {
                // only color change after word is set, this is to prevent color spread when first spawning bubbles
                HandleColorChange(other.gameObject.GetComponent<WordBubble>());
            }
        }
    }

    
    private void OnTriggerExit(Collider other) {
        // if no longer colliding with another bubble, set IsColliding to false
        if (other.gameObject.GetComponent<WordBubble>() != null) {
            IsColliding = false;
        }
    }
    

    private void HandleColorChange(WordBubble colliding) {
        // if this bubble is already colored, do nothing. only non-colored bubbles will take the color.
        if(currentColor.tone != BubbleTone.None) {
            return;
        }

        // if this bubble is non-colored and is colliding with a bubble that already has a color, take its color
        if(colliding.currentColor.tone != BubbleTone.None) {
            Debug.Log("Color spread from " + colliding.word.word + " to " + word.word);
            SetColor(colliding.currentColor);
        }
    }
}