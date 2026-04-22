using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public float moveInterval;
    public int StartSize = 10;
    private Vector3 direction;
    private int xpos, ypos;

    public GameObject bodyPrefab;
    public GameObject headPrefab;

    public List<Transform> bodySegments = new List<Transform>();
    public bool snakeGenerated = false;
    public bool GameOver = false;

    public GameObject UIpanel;
    public GameObject gameoverpanel;

    private void Start()
    {
        StartCoroutine(moveCoroutine());
        direction = Vector3.right;

        for(int i=0;i<StartSize;i++)
        {
            GrowSnake();
        }
        Invoke("SetSnakeGenerated", 5);
    }
    private void SetSnakeGenerated()
    {
        snakeGenerated = true;
    }

    private void Update()
    {
        HandleInput();
    }

    private void movement()
    {
        if (!GameOver)
        {
            
            Vector3 newPosition = transform.position + direction;
            transform.position = newPosition;

            
            for (int i = bodySegments.Count - 1; i > 0; i--)
            {
                bodySegments[i].position = bodySegments[i - 1].position;
            }
            if (bodySegments.Count > 0)
            {
                bodySegments[0].position = newPosition;
            }
        }
        
    }

    private void HandleInput()
    {
        

        if (Input.GetKey(KeyCode.W) && direction != -Vector3.forward)
        {
            direction = Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S) && direction != Vector3.forward)
        {
            direction = -Vector3.forward;
        }

        if (Input.GetKey(KeyCode.A) && direction != Vector3.right)
        {
            direction = Vector3.left;
        }

        if (Input.GetKey(KeyCode.D) && direction != Vector3.left)
        {
            direction = Vector3.right;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            GrowSnake();
        }
    }

    public void GrowSnake()
    {
        GameObject newSegment;
        if(bodySegments.Count==0)
        {
            newSegment = Instantiate(headPrefab, transform.position, Quaternion.identity);
          
        }
        else
        {
            newSegment = Instantiate(bodyPrefab, bodySegments[bodySegments.Count - 1].position, Quaternion.identity);
            
        }
        Transform segmentTransform = newSegment.transform;
       
        

        
        bodySegments.Add(segmentTransform);
    }
    public void CheckSelfCol()
    {
        if (bodySegments.Count < 2)
        {
            // No self-collision is possible with less than 2 body segments
            return;
        }

        Vector3 headPosition = bodySegments[0].position; 

        
        for (int i = 1; i < bodySegments.Count; i++)
        {
            Vector3 bodySegmentPosition = bodySegments[i].position;

           
            if (headPosition == bodySegmentPosition)
            {
                
                Debug.Log("Self Collision Detected");
               
                GameOverFunc();

                break;
                
            }
        }
    }
    public void GameOverFunc()
    {
        GameOver = true;
        UIpanel.SetActive(false);
        gameoverpanel.SetActive(true);
        for(int i=0;i<bodySegments.Count;i++)
        {
            bodySegments[i].gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0);
            if(i==0)
            {
                bodySegments[0].GetComponent<BoxCollider>().isTrigger = false;
                bodySegments[0].GetComponent<Rigidbody>().isKinematic = false;
                bodySegments[0].GetComponent<Rigidbody>().useGravity = true;
            }
            else
            {
                bodySegments[i].gameObject.AddComponent<Rigidbody>();
            }
            bodySegments[i].transform.localScale = new Vector3(1f, 1f, 1f)*0.75f;
            bodySegments[i].GetComponent<Rigidbody>().AddForce(200 * Vector3.up);
        }
       
       
    }

    private IEnumerator moveCoroutine()
    {
          while (true)
          {
                yield return new WaitForSeconds(moveInterval);
                movement();
                if (snakeGenerated == true)
                {
                    CheckSelfCol();
                }

          }
        
       
    }
}
