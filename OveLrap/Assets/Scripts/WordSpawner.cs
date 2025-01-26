using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WordSpawner : MonoBehaviour {
    public GameObject wordSpawnerRoot;
    public GameObject wordBubblePrefab;
    public float areaWidth = 10f;
    public float areaDepth = 10f;
    public float bubbleRadius = 4f; // scale of the uniform sphere / 2. Used for checking collisions
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

    private void Start() {
        string[] adjectivesArray = adjectiveWordList.Split(',');
        foreach (string word in adjectivesArray) {
            adjectives.Add(new Word(word, WordType.Adjective));
        }
        // randomize the list
        adjectives = adjectives.OrderBy(x => Random.value).ToList();

        string[] nounsArray = nounWordList.Split(',');
        foreach (string word in nounsArray) {
            nouns.Add(new Word(word, WordType.Noun));
        }
        // randomize the list
        nouns = nouns.OrderBy(x => Random.value).ToList();

    }

    public void Update() {
        bubbleSpawnTimer += Time.deltaTime;
        if (bubbleSpawnTimer > secondsBetweenSpawn)
        {
            bubbleSpawnTimer = 0f;

            Vector3 randomPosition = new Vector3(wordSpawnerRoot.transform.position.x + Random.Range(-areaWidth / 2, areaWidth / 2), wordSpawnerRoot.transform.position.y, wordSpawnerRoot.transform.position.z + Random.Range(-areaDepth / 2, areaDepth / 2));

            // if bubble is colliding with another bubble (collider with tag Bubble), reposition.
            // after X number of attempts, just destroy the bubble. This could happen if there's no space for the bubble to spawn
            int attempts = 0;
            Collider[] colliders = Physics.OverlapSphere(randomPosition, bubbleRadius);
            while (colliders.Count(x => x.CompareTag("Bubble")) > 0 && attempts < 10) {
                randomPosition = new Vector3(wordSpawnerRoot.transform.position.x + Random.Range(-areaWidth / 2, areaWidth / 2), wordSpawnerRoot.transform.position.y, wordSpawnerRoot.transform.position.z + Random.Range(-areaDepth / 2, areaDepth / 2));
                colliders = Physics.OverlapSphere(randomPosition, bubbleRadius);
                attempts++;
                Debug.Log("Spawn collision at " + randomPosition + ". Repositioning bubble. Attempt: " + attempts);
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

                // randomly set color, more likely to be none color
                var randomColor = Random.Range(0, 6); // 0, 1, 2 = colored, 3,4,5 = none
                if (randomColor >= 3) {
                    newWordBubble.GetComponent<WordBubble>().SetColor(PlayspaceController.Instance.bubbleColors[3]);
                } else {
                    newWordBubble.GetComponent<WordBubble>().SetColor(PlayspaceController.Instance.bubbleColors[randomColor]);
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

                // randomly set color, more likely to be none color
                var randomColor = Random.Range(0, 6); // 0, 1, 2 = colored, 3,4,5 = none
                if (randomColor >= 3) {
                    newWordBubble.GetComponent<WordBubble>().SetColor(PlayspaceController.Instance.bubbleColors[3]);
                } else {
                    newWordBubble.GetComponent<WordBubble>().SetColor(PlayspaceController.Instance.bubbleColors[randomColor]);
                }

                nextWordAdjIndex++;
                nextWordIsNoun = true;
            }
            newWordBubble.GetComponent<WordBubble>().SetWord(newWord);
            wordBubbles.Add(newWordBubble);
        }
    }

    // Reposition all bubbles in the playspace, useful if a bubble goes out of bounds
    public void RepositionBubbles() {
        foreach (GameObject bubble in wordBubbles) {
            Vector3 randomPosition = new Vector3(wordSpawnerRoot.transform.position.x + Random.Range(-areaWidth / 2, areaWidth / 2), wordSpawnerRoot.transform.position.y, wordSpawnerRoot.transform.position.z + Random.Range(-areaDepth / 2, areaDepth / 2));

            // if bubble is colliding with another bubble (collider with tag Bubble), reposition.
            // after X number of attempts, just destroy the bubble. This could happen if there's no space for the bubble to spawn
            int attempts = 0;
            Collider[] colliders = Physics.OverlapSphere(randomPosition, bubbleRadius);
            while (colliders.Count(x => x.CompareTag("Bubble")) > 0 && attempts < 10) {
                randomPosition = new Vector3(wordSpawnerRoot.transform.position.x + Random.Range(-areaWidth / 2, areaWidth / 2), wordSpawnerRoot.transform.position.y, wordSpawnerRoot.transform.position.z + Random.Range(-areaDepth / 2, areaDepth / 2));
                colliders = Physics.OverlapSphere(randomPosition, bubbleRadius);
                attempts++;
                Debug.Log("Spawn collision at " + randomPosition + ". Repositioning bubble. Attempt: " + attempts);
            }

            if (attempts >= 10) {
                return;
            }

            if(bubble != null) // check if bubble is not destroyed if they happen to collide with each other
                bubble.transform.position = randomPosition;
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