using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public static GameController instance;
	public GameObject gameOverPanel;
	public GameObject gameWinPanel;

	private bool gameWin = false;

	[HideInInspector]
	public bool gameOver = false;

	private void Awake()
	{
		instance = this;
	}

	/// <summary>
	/// Chame sempre que um gato morrer
	/// </summary>
	public void Killed()
	{
		// Essa condicional é só pra evitar que um gato faça besteira...
		if (!gameWin)
		{
			gameOver = true;
			gameOverPanel.SetActive(true);
		}
	}

	/// <summary>
	/// Recarrega o level atual
	/// </summary>
	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	/// <summary>
	/// Abre a cena do menu
	/// </summary>
	public void OpenMenu()
	{
		SceneManager.LoadScene("Menu");
	}
}
