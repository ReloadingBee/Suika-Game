using UnityEngine;

public class Deathzone : MonoBehaviour
{
	public float raycastY;

	SpriteRenderer rend;

	void Start()
	{
		Game.instance.onFruitAppeared.AddListener(ShootRaycast);
		
		rend = gameObject.GetComponent<SpriteRenderer>();
		rend.enabled = false;
	}

	void ShootRaycast()
	{
		var hit = Physics2D.Raycast(new Vector2(-2.5f, raycastY), Vector2.right, 5f);
		if (hit && hit.collider.CompareTag("Fruit"))
		{
			rend.enabled = true;
			Game.instance.onFruitAppeared.RemoveListener(ShootRaycast);
		}
	}
}
