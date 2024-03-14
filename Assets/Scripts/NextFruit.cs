using UnityEngine;
using System.Collections.Generic;
public class NextFruit : MonoBehaviour
{
	public List<Sprite> fruitTextures;

	SpriteRenderer rend;

	void Start()
	{
		rend = gameObject.GetComponent<SpriteRenderer>();
		rend.enabled = false;
	}
	void Update()
	{
		if (rend.enabled) return;
		
		rend.sprite = fruitTextures[Game.instance.nextFruit];
		rend.enabled = !Game.instance.isGameOver;
	}
}
