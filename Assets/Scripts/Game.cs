using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] List<GameObject> fruits;

    public Fruit SpawnNewFruit(Vector2 pos, int type)
    {
        //the biggest fruit becomes the smallest
        //when the fruit type becomes too large
        type %= fruits.Count;

        var fruit = Instantiate(fruits[type], pos, Quaternion.identity).GetComponent<Fruit>();
        fruit.fruitType = type;
        return fruit;
    }

    public int currentFruit;
    public int nextFruit;
    public bool isFalling;
    public bool isThinking;
    public bool isGameOver;

    Camera cam;

    void Start()
    {
        isGameOver = false;
        isFalling = false;
        isThinking = false;
        currentFruit = Random.Range(0, 3);
        nextFruit = Random.Range(0, 3);
        
        cam = Camera.main;
    }
    async void Update()
    {
        if (isGameOver) return;
        if (isFalling) return;

        if (isThinking) return;
        var hit = Physics2D.Raycast(new Vector2(-2.5f, 3f), Vector2.right, 5f);
        if (hit && hit.collider.CompareTag("Fruit"))
        {
            GameOver();
            return;
        }
        
        isThinking = true;
        
        var fruit = SpawnNewFruit(new Vector2(0, 4), currentFruit);
        fruit.GetComponent<Rigidbody2D>().gravityScale = 0;
        
        await new WaitUntil(()=>Input.GetKeyDown(KeyCode.Mouse0));
        
        // Mouse clicked - fall
        fruit.GetComponent<Rigidbody2D>().gravityScale = 1;
        
        // Move fruit to mouse position
        var x = cam.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, 0)).x;
        x = Mathf.Clamp(x, -2.5f, 2.5f);
        fruit.transform.position = new Vector2(x, 4);
        
        isFalling = true;
        currentFruit = nextFruit;
        nextFruit = Random.Range(0, 3);
        
        await new WaitForSeconds(1);
        
        isFalling = false;
        isThinking = false;
    }

    public void GameOver()
    {
        isGameOver = true;
    }
}
