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

    public void SetWord(Word word) {
        this.word = word;
        text.text = word.word;
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
}