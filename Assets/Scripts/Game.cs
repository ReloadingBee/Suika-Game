using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    [SerializeField] List<GameObject> fruits;

    public void SpawnNewFruit(Vector2 pos, int type)
    {
        //the biggest fruit becomes the smallest
        //when the fruit type becomes too large
        type = type % fruits.Count;

        var fruit = Instantiate(fruits[type], pos, Quaternion.identity).GetComponent<Fruit>();
        fruit.fruitType = type;
        //print("new fruit");
    }
}
