using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
	public GameObject MainMenu;
	public GameObject OptionsMenu;
	public GameObject InstructionsMenu;
	public GameObject CreditsMenu;
	public bool withInstructions = true;

	public void HandleTutorialOnOff(bool value) {
		withInstructions = value;
	}

	public void HandleStart()
	{
		if (!withInstructions) {
			playGame();
			return;
		}
		MainMenu.SetActive(false);
		InstructionsMenu.SetActive(true);
	}
	public void playGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void options()
	{
		MainMenu.SetActive(false);
		OptionsMenu.SetActive(true);
	}

	public void credits() {
		MainMenu.SetActive(false);
		CreditsMenu.SetActive(true);
	}

	public void back()
	{
		MainMenu.SetActive(true);
		OptionsMenu.SetActive(false);
		CreditsMenu.SetActive(false);
	}

	public void exitGame()
	{
		Application.Quit();

		// For testing in editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
}
