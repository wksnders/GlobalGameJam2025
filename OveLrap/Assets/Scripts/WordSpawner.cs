using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSpawner : MonoBehaviour {
    public GameObject wordSpawnerRoot;
    public GameObject wordBubblePrefab;
    public float areaWidth = 10f;
    public float areaHeight = 10f;
    public float areaDepth = 10f;
    public int secondsBetweenSpawn;
    public List<GameObject> wordBubbles;
    float bubbleSpawnTimer = 0f;
    bool nextWordIsNoun = false;
    int nextWordNounIndex = 0;
    int nextWordAdjIndex = 0;

    public string adjectiveWordList; // comma separated list of adjectives
    public string nounWordList; // comma separated list of nouns

    private List<Word> adjectives = new List<Word>();
    private List<Word> nouns = new List<Word>();

    // at start, there will be one of each color in noun and adjective list
    // if there are 3 bubble colors, there will be 6 total
    public List<BubbleColor> bubbleColors;
    private Dictionary<int, BubbleColor> adjectiveBubbleColorMap = new Dictionary<int, BubbleColor>(); // index -> color
    private Dictionary<int, BubbleColor> nounBubbleColorMap = new Dictionary<int, BubbleColor>(); // index -> color

    private void Start() {
        string[] adjectivesArray = adjectiveWordList.Split(',');
        foreach (string word in adjectivesArray) {
            adjectives.Add(new Word(word, WordType.Adjective));
        }

        string[] nounsArray = nounWordList.Split(',');
        foreach (string word in nounsArray) {
            nouns.Add(new Word(word, WordType.Noun));
        }

        // randomly select index of the word that will be spawned colored
        foreach(var bubbleColor in bubbleColors)
        {
            // select index from adjectives
            int randomIndex = Random.Range(0, adjectives.Count);
            // if index is already in map, select another
            while (adjectiveBubbleColorMap.ContainsKey(randomIndex)) {
                randomIndex = Random.Range(0, adjectives.Count);
            }
            adjectiveBubbleColorMap[randomIndex] = bubbleColor;
            Debug.Log("Adjective at index " + randomIndex + " will be " + bubbleColor.color);

            // select index from nouns
            randomIndex = Random.Range(0, nouns.Count);
            // if index is already in map, select another
            while (nounBubbleColorMap.ContainsKey(randomIndex)) {
                randomIndex = Random.Range(0, nouns.Count);
            }
            nounBubbleColorMap[randomIndex] = bubbleColor;
            Debug.Log("Noun at index " + randomIndex + " will be " + bubbleColor.color);
        }

        //SpawnWordBubbles();
    }

    private void SpawnWordBubbles() {
        for (int i = 0; i < adjectives.Count; i++) {
            Vector3 randomPosition = new Vector3(wordSpawnerRoot.transform.position.x + Random.Range(-areaWidth / 2, areaWidth / 2), wordSpawnerRoot.transform.position.y + Random.Range(-areaHeight / 2, areaHeight / 2), wordSpawnerRoot.transform.position.z + Random.Range(-areaDepth / 2, areaDepth / 2));
            GameObject wordBubble = Instantiate(wordBubblePrefab, randomPosition, Quaternion.identity);
            wordBubble.transform.parent = wordSpawnerRoot.transform;
            wordBubble.GetComponent<WordBubble>().SetWord(adjectives[i]);

            // if word is in map, set color for starting bubble colors
            if (adjectiveBubbleColorMap.ContainsKey(i)) {
                wordBubble.GetComponent<WordBubble>().SetColor(adjectiveBubbleColorMap[i]);
            }
        }
        for (int i = 0; i < nouns.Count; i++) {
            Vector3 randomPosition = new Vector3(wordSpawnerRoot.transform.position.x + Random.Range(-areaWidth / 2, areaWidth / 2), wordSpawnerRoot.transform.position.y + Random.Range(-areaHeight / 2, areaHeight / 2), wordSpawnerRoot.transform.position.z + Random.Range(-areaDepth / 2, areaDepth / 2));
            GameObject wordBubble = Instantiate(wordBubblePrefab, randomPosition, Quaternion.identity);
            wordBubble.transform.parent = wordSpawnerRoot.transform;
            wordBubble.GetComponent<WordBubble>().SetWord(nouns[i]);

            // if word is in map, set color for starting bubble colors
            if (nounBubbleColorMap.ContainsKey(i)) {
                wordBubble.GetComponent<WordBubble>().SetColor(nounBubbleColorMap[i]);
            }
        }
    }

    public void Update() {
        // Reset for quick debug
        if (Input.GetKeyDown(KeyCode.R)) {
            foreach (Transform child in wordSpawnerRoot.transform) {
                Destroy(child.gameObject);
            }
            SpawnWordBubbles();
        }

        bubbleSpawnTimer += Time.deltaTime;
        if (bubbleSpawnTimer > secondsBetweenSpawn)
        {
            bubbleSpawnTimer = 0f;
            GameObject newWordBubble = Instantiate(wordBubblePrefab, wordSpawnerRoot.transform);
            Vector3 randomPosition = new Vector3(wordSpawnerRoot.transform.position.x + Random.Range(-areaWidth / 2, areaWidth / 2), wordSpawnerRoot.transform.position.y + Random.Range(-areaHeight / 2, areaHeight / 2), wordSpawnerRoot.transform.position.z + Random.Range(-areaDepth / 2, areaDepth / 2));
            newWordBubble.transform.position = randomPosition;
            Word newWord;
            if( nextWordIsNoun )
            {
                // Loops through list again?
                if (nextWordNounIndex >= nouns.Count)
                    nextWordNounIndex = 0;
                newWord = nouns[nextWordNounIndex];
                
                // if word is in map, set color for starting bubble colors
                if (nounBubbleColorMap.ContainsKey(nextWordNounIndex)) {
                    newWordBubble.GetComponent<WordBubble>().SetColor(nounBubbleColorMap[nextWordNounIndex]);
                }

                nextWordNounIndex++;
                nextWordIsNoun = false;
            }
            else
            {
                // Loops through list again?
                if (nextWordAdjIndex >= adjectives.Count)
                    nextWordAdjIndex = 0;
                newWord = adjectives[nextWordAdjIndex];

                // if word is in map, set color for starting bubble colors
                if (adjectiveBubbleColorMap.ContainsKey(nextWordAdjIndex)) {
                    newWordBubble.GetComponent<WordBubble>().SetColor(adjectiveBubbleColorMap[nextWordAdjIndex]);
                }

                nextWordAdjIndex++;
                nextWordIsNoun = true;
            }
            newWordBubble.GetComponent<WordBubble>().SetWord(newWord);
            wordBubbles.Add(newWordBubble);
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

public enum BubbleTone {
    Happy,
    Angry,
    Sad,
}

[System.Serializable]
public class BubbleColor {
    public Color color;
    public BubbleTone tone;
}