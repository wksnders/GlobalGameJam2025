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
    public AudioSource audioSource;
    public float scaleMultiplier = 1.5f; // growth of bubble when it becomes a pair

    [Header("SFX")]
    public AudioClip mergeBubbleSFX;

    bool SetupDone = false;
    public bool IsColliding { get; private set; }

    public void SetWord(Word word) {
        this.word = word;
        text.text = word.word;
        SetupDone = true;
    }

    public void AppendWords(Word firstWord, Word secondWord) {
        string newText = "";

        // order of words should be [adjective + noun]
        if(firstWord.type == WordType.Adjective) {
            newText = firstWord.word + " " + secondWord.word;
        } else {
            newText = secondWord.word + " " + firstWord.word;
        }
        
        Word newWord = new Word(newText, WordType.Pair);
        SetWord(newWord);
    }

    public void SetColor(BubbleColor color) {
        currentColor = color;
        renderer.material.color = currentColor.color;
    }

    // Point text toward camera
    void Update() {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);

        // DEBUG
        if(Input.GetKeyDown(KeyCode.M)) {
            audioSource.PlayOneShot(mergeBubbleSFX);
        }
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

        // if this bubble's word type matches the colliding bubble's word type, don't do anything
        // we only want to spread color between different word types [noun + adj]
        if(word.type == colliding.word.type) {
            return;
        }

        // if this bubble or the colliding bubble is already a pair, do nothing
        if(word.type == WordType.Pair || colliding.word.type == WordType.Pair) {
            return;
        }

        // if this bubble is non-colored and is colliding with a bubble that already has a color
        //    - create a new word that is [adjective + noun] with type Pair
        //    - set the color of this bubble to the color of the colliding bubble
        //    - destroy colliding bubble
        //    - scale this bubble to be bigger
        if(colliding.currentColor.tone != BubbleTone.None) {
            Debug.Log("Color spread from " + colliding.word.word + " to " + word.word);
            AppendWords(word, colliding.word);
            SetColor(colliding.currentColor);
            // add bubble to list of pair counts for goal tracking
            PlayspaceController.Instance.IncrementPairBubbleCount(this);
            Destroy(colliding.gameObject);
            transform.localScale *= scaleMultiplier;
            audioSource.PlayOneShot(mergeBubbleSFX);
        }
    }
}