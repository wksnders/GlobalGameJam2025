using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialAgentcounter : MonoBehaviour
{
    public TMP_Text displayNumText;
    [Tooltip("Remember to set the image type to radial, filled.")]
    public Image fillCircle;
    [Tooltip("Number of seconds between each increase.")]
    public float rechargeTime;

    private float timeTillAgentAvailable;
    private int currentAgents;

    private void Start()
    {
        timeTillAgentAvailable = rechargeTime;
        currentAgents = 1;
    }

    void Update()
    {

        timeTillAgentAvailable -= Time.deltaTime;

        if (timeTillAgentAvailable <= 0)
        {
            AddPlaceableAgent();
            timeTillAgentAvailable = rechargeTime;
        }

        UpdateUI();
    }

    private void AddPlaceableAgent()
    {
        currentAgents++;
        if (currentAgents > 3) {
            currentAgents = 1;
        }
    }

    private void UpdateUI()
    {
        // Update filling circle based on timeTillAgentAvailable
        fillCircle.fillAmount = 1 - (timeTillAgentAvailable / rechargeTime);
        displayNumText.text = currentAgents.ToString();
    }

}
