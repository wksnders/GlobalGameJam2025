using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPages : MonoBehaviour
{
    public List<GameObject> pages;
    int index = 0;
	public void HandleContinue()
	{
		pages[index].SetActive(false);
		index++;
		pages[index].SetActive(true);
	}
}
