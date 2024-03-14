using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	[SerializeField] bool isInMenu;
	[SerializeField] bool menuLoaded;
	
	public GameObject gameOverCanvas;
	public TMP_Text finalScoreText;
	public TMP_Text highScoreText;

	public TMP_Text scoreText;

	void Awake()
	{
		if (instance != null) Destroy(gameObject);
		instance = this;
	}
	void Start()
	{
		StartGame();
	}
	void StartGame()
	{
		isInMenu = false;
		Game.instance.StartGame();
	}
	public void GameOver()
	{
		isInMenu = true;
	}

	void Update()
	{
		if (!isInMenu)
		{
			// hide menu
			menuLoaded = false;
			gameOverCanvas.SetActive(false);
			scoreText.gameObject.SetActive(true);
			scoreText.text = $"{Game.instance.score}";
			return;
		}

		// play again
		if (Input.GetMouseButtonDown(0) && !GameObject.FindGameObjectWithTag("Fruit"))
		{
			StartGame();
		}
		
		// load menu only once
		if (menuLoaded) return;
		menuLoaded = true;
		
		gameOverCanvas.SetActive(true);
		scoreText.gameObject.SetActive(false);
		
		finalScoreText.text = $"Score: {Game.instance.score}";
		highScoreText.text = $"High Score: {PlayerPrefs.GetInt("HighScore", 0)}";
	}
}
