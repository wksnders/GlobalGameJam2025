using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
	public GameObject MainMenu;
	public GameObject OptionsMenu;

	public void playGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void options()
	{
		MainMenu.SetActive(false);
		OptionsMenu.SetActive(true);
	}

	public void back()
	{
		MainMenu.SetActive(true);
		OptionsMenu.SetActive(false);
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
