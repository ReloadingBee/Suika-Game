using UnityEngine;

public class Fruit : MonoBehaviour
{
	public int fruitType;
	public Rigidbody2D rb;
	Game game;

	public bool isDeleted;

	public GameObject explosionParticles;
	public GameObject mergeParticles;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		game = Game.instance;
		isDeleted = false;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.gameObject.CompareTag("Fruit")) return;

		// other IS a fruit
		var fruit = other.gameObject.GetComponent<Fruit>();

		// only merge once before deleting!
		if (isDeleted || fruit.isDeleted) return;

		if (fruit.fruitType != fruitType) return;
		
		// fruit types match - combine
		// check which one's velocity is higher
		// delete both fruits and spawn another one
		// in the position of the fruit with a lower velocity
		var fruitVelocity = fruit.rb.velocity.magnitude;
		if (fruitVelocity >= rb.velocity.magnitude)
		{
			// this is slower than other fruit
			// spawn new fruit in my position
			var newFruit = game.SpawnNewFruit(transform.position, fruitType + 1);
			game.AddScore(fruit.fruitType);
			Instantiate(mergeParticles, newFruit.transform.position, Quaternion.identity);
			
			isDeleted = true;
			fruit.isDeleted = true;
			Destroy(other.gameObject);
			Destroy(gameObject);
		}
	}

	void Update()
	{
		if(isDeleted) Destroy(gameObject);

		if (!game.isGameOver)
			return;
		if (game.timeSinceGameOver >= Mathf.Abs(transform.position.y / 2f))
		{
			Instantiate(explosionParticles, transform.position, Quaternion.identity);
			isDeleted = true;
		}
	}
}
