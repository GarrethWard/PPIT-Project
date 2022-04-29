using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerDirection direction;
    [HideInInspector]
    public float stepLength = 0.2f;
    [HideInInspector]
    public float movementFrequency = 0.1f;

    private float counter;
    private bool move;

    [SerializeField]
     private GameObject tailPrefab;


    private List<Vector3> deltaPosition;
    private List<Rigidbody> nodes;

    private Rigidbody mainBody;
    private Rigidbody headBody;
    private Transform tr;

    private bool createNodeAtTail;




    void Awake()
    {
        tr = transform;
        mainBody = GetComponent<Rigidbody>();

        InitSnakeNodes();
        InitPlayer();

        deltaPosition = new List<Vector3>()
        {
            new Vector3(-stepLength,0f), //left
            new Vector3(0f, stepLength), //up
            new Vector3(stepLength,0f), //right
            new Vector3(0f, -stepLength) //down
        };
    }

    void Update()
    {
        CheckMovementFrequency();
    }

    void FixedUpdate()
    {
        if (move) {
            move = false;

            Move();

        }
    }
    void InitSnakeNodes()
    {
        nodes = new List<Rigidbody>();
        nodes.Add(tr.GetChild(0).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(1).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(2).GetComponent<Rigidbody>());

        headBody = nodes[0];

     

    }

    void SetDirectionRandom()
    {
        int dirRandom = Random.Range(0,(int)PlayerDirection.COUNT);
        direction = (PlayerDirection)dirRandom;
    }
    void InitPlayer()
    {
        SetDirectionRandom();

        switch (direction)
        {
            case PlayerDirection.RIGHT: //shifts all player game objects to right using x val
                nodes[1].position = nodes[0].position - new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position - new Vector3(Metrics.NODE * 2f, 0f, 0f);
                break;
            case PlayerDirection.LEFT:
                nodes[1].position = nodes[0].position + new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position + new Vector3(Metrics.NODE * 2f, 0f, 0f);
                break;
            case PlayerDirection.UP:
                nodes[1].position = nodes[0].position - new Vector3(0f,Metrics.NODE, 0f);//y val
                nodes[2].position = nodes[0].position - new Vector3(0f,Metrics.NODE * 2f, 0f);
                break;
            case PlayerDirection.DOWN:
                nodes[1].position = nodes[0].position + new Vector3(0f, Metrics.NODE, 0f);//y val
                nodes[2].position = nodes[0].position + new Vector3(0f, Metrics.NODE * 2f, 0f);
                break;

        }


    }



    void Move()
    {
        Vector3 dPosition = deltaPosition[(int)direction];

        Vector3 parentPos = headBody.position;
        Vector3 previousPosition;

        mainBody.position = mainBody.position + dPosition;
        headBody.position = headBody.position + dPosition;

        for (int i = 1; i < nodes.Count; i++)
        {
            previousPosition = nodes[i].position;

            nodes[i].position = parentPos;
            parentPos = previousPosition;
        }

        //check if we need to create a new node because we ate food

        if (createNodeAtTail)
        {
            createNodeAtTail = false;

            GameObject newNode = Instantiate(tailPrefab, nodes[nodes.Count - 1].position,  Quaternion.identity); //creates child object identical to parent

            newNode.transform.SetParent(transform, true);
            nodes.Add(newNode.GetComponent<Rigidbody>());
        }
    }

    void CheckMovementFrequency()
    {
        counter += Time.deltaTime;

        if(counter >= movementFrequency) // sp snake doesnt move every frame
        {
            counter = 0f;
            move = true;
        }
    }

    public void SetInputDirection(PlayerDirection dir) // Can only move one direction if going up cant go down, left cant go right
    {

        if (dir == PlayerDirection.UP && direction == PlayerDirection.DOWN ||
            dir == PlayerDirection.DOWN && direction == PlayerDirection.UP ||
            dir == PlayerDirection.RIGHT && direction == PlayerDirection.LEFT||
            dir == PlayerDirection.LEFT && direction == PlayerDirection.RIGHT){

            return;
        }
        direction = dir;

        ForceMove();
    }

    void ForceMove()
    {
        counter = 0;
        move = false;
        Move();
    }

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == Tags.FRUIT)
        {
            target.gameObject.SetActive(false);

            createNodeAtTail = true;

            GameplayController.instance.IncreaseScore();
            AudioManager.instance.PlayPickUpSound();

        }

        if (target.tag == Tags.WALL|| target.tag == Tags.BOMB || target.tag == Tags.TAIL)
        {
            Time.timeScale = 0f;
            AudioManager.instance.PlayDeadSound();            
        }
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
