using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFood : MonoBehaviour
{
    public GameObject foodPrefab;
    public float xMin, xMax, yMin, yMax;
    public SnakeMovement snakeMovement;

    private GameObject currentFood;

   
    void Start()
    {
        GenerateInitialFood();
    }

    
    void Update()
    {
        
        if (currentFood == null)
        {
            GenerateInitialFood();
        }
    }

    void GenerateInitialFood()
    {
        
        Vector3 foodPosition;

        do
        {
            int randomX = Mathf.FloorToInt(Random.Range(xMin, xMax));
            int randomY = Mathf.FloorToInt(Random.Range(yMin, yMax));

            foodPosition = new Vector3(randomX, 0.0f, randomY);
        } while (IsFoodOverlappingWithSnake(foodPosition));

        
        currentFood = Instantiate(foodPrefab, foodPosition, Quaternion.identity);
    }

    bool IsFoodOverlappingWithSnake(Vector3 foodPosition)
    {
        if (snakeMovement == null)
        {
            return false; 
        }

       
        foreach (Transform bodySegment in snakeMovement.bodySegments)
        {
            float distance = Vector3.Distance(bodySegment.position, foodPosition);
            if (distance < 1.0f) 
            {
                return true;
            }
        }

        return false; 
    }
}
