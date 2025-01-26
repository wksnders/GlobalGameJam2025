using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour, ISingleton<GameManager>
{
    private static GameManager _inst;
    public static GameManager Instance
    {
        get
        {
            if (_inst == null)
            {
                _inst = GameObject.FindObjectOfType<GameManager>();
            }
            return _inst;
        }
    }

    public GameObject EndGamePanel;
    public TextMeshProUGUI PhrasesText;

    void Start() {
        EndGamePanel.SetActive(false);
    }

    void Update() {
        CheckForWin();
    }

    public void CheckForWin() {
        if (PlayspaceController.Instance.currentNumberOfPairs >= PlayspaceController.Instance.goalPairs) {
            Debug.Log("You win!");

            GetAllPairWords();
            EndGamePanel.SetActive(true);
        }
    }

    // Get all the pairs, color them based on color
    void GetAllPairWords() {
        string phrase = "";
        foreach (WordBubble pair in PlayspaceController.Instance.pairs) {
            // add color html to phrase
            phrase += "<color=#" + ColorUtility.ToHtmlStringRGB(pair.currentColor.color) + ">" + pair.word.word + "</color>\n";
        }
        PhrasesText.text = phrase;
    }
}
