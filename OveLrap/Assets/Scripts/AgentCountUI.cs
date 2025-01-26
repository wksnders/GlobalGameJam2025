using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AgentCountUI : MonoBehaviour
{
    public TMP_Text displayNumText;
    [Tooltip("remember to set the image type to radial,filled.")]
    public Image fillCircle;
    [Tooltip("set to negative value for no upperlimit.")]
    public int MaxCharges = -1;
    [Tooltip("number of seconds between each increase.")]
    public float rechargeTime;

    private float timeTillAgentAvailible;

    private void Start()
    {
        timeTillAgentAvailible = rechargeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGameOver) {
            return;
        }
        if(MaxCharges > 0)
        {
            if (PlayspaceController.Instance.NumPlaceableAgents >= MaxCharges)
            {
                Debug.Log("player at max number of charges for placeable agents");
                UpdateUI();
                return;
            }
        }

        timeTillAgentAvailible -= Time.deltaTime;

        if (timeTillAgentAvailible <= 0)
        {
            AddPlaceableAgent();
            timeTillAgentAvailible = rechargeTime;
        }

        UpdateUI();
    }

    private void AddPlaceableAgent()
    {
        PlayspaceController.Instance.NumPlaceableAgents++;
    }

    private void UpdateUI()
    {
        // Update filling circle based on timeTillAgentAvailible
        fillCircle.fillAmount = 1 - (timeTillAgentAvailible / rechargeTime);
        displayNumText.text = ""+PlayspaceController.Instance.NumPlaceableAgents;
    }
}
