using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSpawner : MonoBehaviour {
    public GameObject wordSpawnerRoot;
    public GameObject wordBubblePrefab;
    public float areaWidth = 10f;
    public float areaHeight = 10f;
    public float areaDepth = 10f;

    public string adjectiveWordList; // comma separated list of adjectives
    public string nounWordList; // comma separated list of nouns

    private List<Word> adjectives = new List<Word>();
    private List<Word> nouns = new List<Word>();

    private void Start() {
        string[] adjectivesArray = adjectiveWordList.Split(',');
        foreach (string word in adjectivesArray) {
            adjectives.Add(new Word(word, WordType.Adjective));
        }

        string[] nounsArray = nounWordList.Split(',');
        foreach (string word in nounsArray) {
            nouns.Add(new Word(word, WordType.Noun));
        }

        SpawnWordBubbles();
    }

    private void SpawnWordBubbles() {
        for (int i = 0; i < adjectives.Count; i++) {
            Vector3 randomPosition = new Vector3(wordSpawnerRoot.transform.position.x + Random.Range(-areaWidth / 2, areaWidth / 2), wordSpawnerRoot.transform.position.y + Random.Range(-areaHeight / 2, areaHeight / 2), wordSpawnerRoot.transform.position.z + Random.Range(-areaDepth / 2, areaDepth / 2));
            GameObject wordBubble = Instantiate(wordBubblePrefab, randomPosition, Quaternion.identity);
            wordBubble.transform.parent = wordSpawnerRoot.transform;
            wordBubble.GetComponent<WordBubble>().SetWord(adjectives[i]);
        }
        for (int i = 0; i < nouns.Count; i++) {
            Vector3 randomPosition = new Vector3(wordSpawnerRoot.transform.position.x + Random.Range(-areaWidth / 2, areaWidth / 2), wordSpawnerRoot.transform.position.y + Random.Range(-areaHeight / 2, areaHeight / 2), wordSpawnerRoot.transform.position.z + Random.Range(-areaDepth / 2, areaDepth / 2));
            GameObject wordBubble = Instantiate(wordBubblePrefab, randomPosition, Quaternion.identity);
            wordBubble.transform.parent = wordSpawnerRoot.transform;
            wordBubble.GetComponent<WordBubble>().SetWord(nouns[i]);
        }
    }

    // Reset
    public void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            foreach (Transform child in wordSpawnerRoot.transform) {
                Destroy(child.gameObject);
            }
            SpawnWordBubbles();
        }
    }

}

public enum WordType {
    Adjective,
    Noun
}

public class Word {
    public string word;
    public WordType type;
    public Word(string word, WordType type) {
        this.word = word;
        this.type = type;
    }
}