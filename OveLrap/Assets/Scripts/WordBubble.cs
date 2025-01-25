using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordBubble : MonoBehaviour
{
    public Word word;
    public TextMeshPro text;
    
    public Color color;

    public void SetWord(Word word)
    {
        this.word = word;
        text.text = word.word;
    }

    // Point text toward camera
    void Update() {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}