using System;
using UnityEngine;

public class Fruit : MonoBehaviour
{
	public int fruitType;
	Rigidbody2D rb;
	Game game;

	[SerializeField] bool isDeleted;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		game = FindObjectOfType<Game>();
		isDeleted = false;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.gameObject.CompareTag("Fruit")) return;

		// other IS a fruit
		var fruit = other.gameObject.GetComponent<Fruit>();

		// only merge once before deleting!
		if (isDeleted || fruit.isDeleted) return;

		if (fruit.fruitType == fruitType)
		{
			// fruit types match - combine
			// check which one's velocity is higher
			// delete both fruits and spawn another one
			// in the position of the fruit with a lower velocity
			var fruitVelocity = fruit.rb.velocity.magnitude;
			if (fruitVelocity >= rb.velocity.magnitude)
			{
				// this is slower than other fruit
				// spawn new fruit in my position
				game.SpawnNewFruit(transform.position, fruitType + 1);
				// only one if statement avoids duplicate fruits from spawning
				// since only one fruit has a lower velocity
				isDeleted = true;
				fruit.isDeleted = true;
				Destroy(other.gameObject);
				Destroy(gameObject);
			}
		}
	}

	void Update()
	{
		if(isDeleted) Destroy(gameObject);
	}
}
