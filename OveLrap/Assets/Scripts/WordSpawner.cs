using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSpawner : MonoBehaviour {
    public GameObject wordSpawnerRoot;
    public GameObject wordBubblePrefab;
    public float areaWidth = 10f;
    public float areaHeight = 10f;
    public float areaDepth = 10f;
    public float bubbleRadius = 2.5f; // scale of the uniform sphere / 2. Used for checking collisions
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
        foreach(var bubbleColor in PlayspaceController.Instance.bubbleColors)
        {
            // select index from adjectives
            int randomIndex = Random.Range(0, adjectives.Count);
            // if index is already in map, select another
            while (adjectiveBubbleColorMap.ContainsKey(randomIndex)) {
                randomIndex = Random.Range(0, adjectives.Count);
            }
            adjectiveBubbleColorMap[randomIndex] = bubbleColor;

            // select index from nouns
            randomIndex = Random.Range(0, nouns.Count);
            // if index is already in map, select another
            while (nounBubbleColorMap.ContainsKey(randomIndex)) {
                randomIndex = Random.Range(0, nouns.Count);
            }
            nounBubbleColorMap[randomIndex] = bubbleColor;
        }

    }

    public void Update() {
        bubbleSpawnTimer += Time.deltaTime;
        // spawn bubbles every X seconds until there is max bubbles, goal is to get many pairs
        if (bubbleSpawnTimer > secondsBetweenSpawn && wordBubbles.Count <= PlayspaceController.Instance.goalPairs * 2)
        {
            bubbleSpawnTimer = 0f;
            Vector3 randomPosition = new Vector3(wordSpawnerRoot.transform.position.x + Random.Range(-areaWidth / 2, areaWidth / 2), wordSpawnerRoot.transform.position.y + Random.Range(-areaHeight / 2, areaHeight / 2), wordSpawnerRoot.transform.position.z + Random.Range(-areaDepth / 2, areaDepth / 2));
            //Vector3 randomPosition = new Vector3(wordSpawnerRoot.transform.position.x + Random.Range(-areaWidth / 2, areaWidth / 2), wordSpawnerRoot.transform.position.y + Random.Range(-areaHeight / 2, areaHeight / 2), wordSpawnerRoot.transform.position.z);

            // if bubble is colliding with another bubble, reposition
            // after X number of attempts, just destroy the bubble. This could happen if there's no space for the bubble to spawn
            int attempts = 0;
            Collider[] colliders = Physics.OverlapSphere(randomPosition, bubbleRadius);
            while (colliders.Length > 0 && attempts < 10) {
                randomPosition = new Vector3(wordSpawnerRoot.transform.position.x + Random.Range(-areaWidth / 2, areaWidth / 2), wordSpawnerRoot.transform.position.y + Random.Range(-areaHeight / 2, areaHeight / 2), wordSpawnerRoot.transform.position.z + Random.Range(-areaDepth / 2, areaDepth / 2));
                //randomPosition = new Vector3(wordSpawnerRoot.transform.position.x + Random.Range(-areaWidth / 2, areaWidth / 2), wordSpawnerRoot.transform.position.y + Random.Range(-areaHeight / 2, areaHeight / 2), wordSpawnerRoot.transform.position.z);
                colliders = Physics.OverlapSphere(randomPosition, bubbleRadius);
                attempts++;
                Debug.Log("Spawn collision. Repositioning bubble. Attempt: " + attempts);
            }

            if (attempts >= 10) {
                return;
            }

            GameObject newWordBubble = Instantiate(wordBubblePrefab, wordSpawnerRoot.transform);
            newWordBubble.layer = 3;
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
    Noun,
    Pair
}

public class Word {
    public string word;
    public WordType type;
    public Word(string word, WordType type) {
        this.word = word;
        this.type = type;
    }
}