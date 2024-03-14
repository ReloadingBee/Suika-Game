using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
	public static Game instance;
	
	[SerializeField] List<GameObject> fruits;

	public Fruit SpawnNewFruit(Vector2 pos, int type)
	{
		//the biggest fruit becomes the smallest
		//when the fruit type becomes too large
		type %= fruits.Count;

		var f = Instantiate(fruits[type], pos, Quaternion.identity).GetComponent<Fruit>();
		f.fruitType = type;
		return f;
	}

	public int score;
	int highScore;
	
	public int currentFruit;
	public int nextFruit;
	
	public bool isFalling;
	public bool isThinking;
	public bool isGameOver;

	public float timeSinceGameOver;

	Camera cam;

	Fruit fruit;
	Fruit fruitNext;
	float x;

	AudioSource source;
	public AudioClip popSound;
	public AudioClip throwSound;

	void Awake()
	{
		if (instance != null) Destroy(gameObject);
		instance = this;
		
		isGameOver = false;
		isFalling = false;
		isThinking = false;
		timeSinceGameOver = 0f;
		currentFruit = Random.Range(0, 3);
		nextFruit = Random.Range(0, 3);

		cam = Camera.main;
		source = gameObject.GetComponent<AudioSource>();
		
		highScore = PlayerPrefs.GetInt("HighScore", 0);
	}
	async void Update()
	{
		//if(Input.GetKeyDown(KeyCode.M)) GameOver();
		if (isGameOver)
		{
			timeSinceGameOver += Time.deltaTime;
			return;
		}
		if (isFalling) return;

		if (isThinking)
		{
			MoveFruitToMouse();
			return;
		}

		var hit = Physics2D.Raycast(new Vector2(-2.5f, 3f), Vector2.right, 5f);
		if (hit && hit.collider.CompareTag("Fruit"))
		{
			GameOver();
			return;
		}

		isThinking = true;

		fruit = SpawnNewFruit(new Vector2(0, 4), currentFruit);
		fruit.rb.gravityScale = 0;
		

		await new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));

		// Mouse clicked - fall
		MoveFruitToMouse();
		if (!fruit) return;
		fruit.rb.gravityScale = 1;
		fruit.rb.angularVelocity = Random.Range(-80f, 80f);
		isFalling = true;
		
		source.PlayOneShot(throwSound, 0.5f);

		// wait a second before spawning the next fruit
		await new WaitForSeconds(1);
		
		currentFruit = nextFruit;
		nextFruit = Random.Range(0, 3);
		
		isFalling = false;
		isThinking = false;

		void MoveFruitToMouse()
		{
			if (!fruit) return;
			x = cam.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, 0)).x;

			x = fruit.fruitType switch
			{
				// UGLY!
				0 => Mathf.Clamp(x, -2.755f, 2.755f),
				1 => Mathf.Clamp(x, -2.715f, 2.715f),
				2 => Mathf.Clamp(x, -2.591f, 2.551f),
				_ => Mathf.Clamp(x, -2.5f, 2.5f)
			};
			fruit.transform.position = new Vector2(x, 4);
		}
	}

	readonly List<int> pointsForFruitTypes = new List<int>
	{
		// Don't ask me
		// https://gaming.stackexchange.com/questions/405265/how-does-scoring-work-in-suika-game
		1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 66
	};
	public void AddScore(int fruitType)
	{
		score += pointsForFruitTypes[fruitType];
		source.PlayOneShot(popSound, 10f); // it is very quiet
	}

	public void StartGame()
	{
		score = 0;
		isFalling = false;
		isThinking = false;
		timeSinceGameOver = 0f;
		currentFruit = Random.Range(0, 3);
		nextFruit = Random.Range(0, 3);
		isGameOver = false;
	}

	void GameOver()
	{
		isGameOver = true;
		fruit.isDeleted = true;
		if(score > highScore)
		{
			highScore = score;
			PlayerPrefs.SetInt("HighScore", highScore);
		}
		
		GameManager.instance.GameOver();
	}
}
