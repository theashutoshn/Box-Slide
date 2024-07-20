using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovement : MonoBehaviour
{
    public GameObject plane;
    public Material[] material = new Material[6];
    private Material selectedMaterial;



    private float moveSpeed = 5f;
    public int randomNumber;
    public int randomNumberColor;
    Vector3 rayOrigin;
    Vector3 forwardVector;
    Vector3 rotationAxis;
    Quaternion rotation;
    Vector3 rotatedVector;
    bool isMoving = false;

    private float detectionThreshold = 0.434f; // distance at which the object will stop if any object is detected
    private Vector3 startPosition;  // to track the starting position
    private float maxDistance = 5f;
    private bool isResetting = false;

    Vector3 currentPosition;

    private SpawnManager spawnManager;
    private UIManager uiManager;
    void Awake()
    {
        randomNumber = Random.Range(1, 7);
        randomNumberColor = Random.Range(1, 7);
        startPosition = rayOrigin; // Store the starting position

        spawnManager = FindObjectOfType<SpawnManager>();
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager not found in the scene!");
        }
    }

    private void Start()
    {
        CalculateRotatedVector(); // Initializing in start becasue we need the updated position of the rotatedVector once the script is started after spawnning

        selectedMaterial = material[randomNumberColor - 1];
        this.GetComponent<MeshRenderer>().material = selectedMaterial;
        if (plane != null)
        {
            plane.transform.rotation = Quaternion.LookRotation(-1 * rotatedVector);
        }
    }

    void CalculateRotatedVector() // calculating the rotatedVector
    {
        rayOrigin = transform.position;
        forwardVector = this.transform.forward;
        rotationAxis = this.transform.up;
        rotation = Quaternion.AngleAxis(randomNumber * 90, rotationAxis);
        rotatedVector = rotation * forwardVector;
    }
    // Update is called once per frame
    void Update()
    {
        // Checking if there is any object in the direction of rotatedVector.

        Debug.Log("rotatedVector:" + rotatedVector);
        Vector3 endpoint = rotatedVector * 10f;
        Ray ray = new Ray(rayOrigin, rotatedVector);
        Debug.DrawRay(rayOrigin, forwardVector, Color.green);
        Debug.DrawRay(rayOrigin, rotationAxis, Color.blue);
        //Debug.DrawLine(rayOrigin, endpoint * 1, Color.yellow);
        Debug.DrawRay(rayOrigin, rotatedVector, Color.cyan);

        //Debug.DrawLine(rayOrigin, forwardVector * 1, Color.red);
        //RaycastHit objectHit;


        //if (Physics.Raycast(ray, out objectHit))
        //{
        //    GameObject hitObject = objectHit.collider.gameObject;
        //    if (hitObject != this.gameObject)
        //    {
        //        Debug.Log("Hit Object: " + hitObject.name); // print out detected object
        //        if (objectHit.distance <= detectionThreshold)
        //        {
        //            isMoving = false; // bool to deactivate MoveObject because an object is detected
        //        }

        //    }
        //}
        //else
        //{
        //    Debug.Log("No Object Detected");
        //}
        //----------------------------------------------------------------------------------------

        // Checking mouse input
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput(Input.mousePosition);
        }

        // Checking touch input
        if (Input.touchCount > 0)
        {
            Debug.Log("Touch detected!");
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("Touch began!");
                HandleInput(touch.position);
            }
        }

        //---------------------------------------------------------------------------------------------
        if (isMoving == true)
        {
            MoveObject(); //ray.direction is the direction of the rotatedVector
        }


        //if (isMoving == true)
        //{

        //    // Check if we've reached the maximum move distance
        //    if (Vector3.Distance(transform.position, startPosition) >= maxDistance)
        //    {
        //        isMoving = false;
        //    }
        //    else
        //    {
        //        // Object move funtion here.
        //        MoveObject(rotatedVector); //ray.direction is the direction of the rotatedVector
        //    }

        //}


        // Destroy the game object if it has moved beyond the maximum distance
        if (Vector3.Distance(transform.position, startPosition) >= maxDistance)
        {
            Destroy(gameObject);
            spawnManager.TileDestroyed();
        }



    }

    void HandleInput(Vector3 inputPosition)
    {
        RaycastHit hit;
        Ray cameraRay = Camera.main.ScreenPointToRay(inputPosition);

        if (Physics.Raycast(cameraRay, out hit))
        {
            Debug.Log("Object is Clicked: " + hit.collider.name); // if clicked on main object, name will print
            if (hit.collider.gameObject == this.gameObject || hit.collider.gameObject == plane) // only move if the correct object is selected.   //hit.collider.gameObject.CompareTag("HexaTile"
            {
                isMoving = true; // bool to activate MoveObject 
                startPosition = transform.position; // resetting the position

                if (uiManager != null)
                {
                    uiManager.DecrementMoves();
                }
            }
        }
    }

    //claudee MoveObject method updated rotateedVector
    void MoveObject()
    {
        if (isResetting) return;

        currentPosition = transform.position;
        Vector3 movement = rotatedVector.normalized * 1f; //0.64f
        Vector3 targetPosition = currentPosition + movement;

        Debug.DrawRay(currentPosition, rotatedVector * detectionThreshold, Color.red, 0.1f);


        RaycastHit hit;
        if (Physics.Raycast(currentPosition, rotatedVector, out hit, detectionThreshold))
        {
            if (hit.collider.gameObject != this.gameObject && hit.collider.gameObject != this.plane)
            {
                Debug.Log("Hit Object: " + hit.collider.name);
                isMoving = false;
                //this.transform.position = startPosition; // for non-smooth transition
                StartCoroutine(SmoothResetPosition()); // to have a smooth transition, we have used Coroutine.
                return;
            }
        }

        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, startPosition) >= maxDistance)
        {
            isMoving = false;
        }


    }

    // Bleow is the correct working method
    //void MoveObject()
    //{
    //    Debug.Log("Move Object");
    //    Vector3 movement = rotatedVector.normalized * 0.64f;

    //    RaycastHit hit;
    //    Debug.DrawRay(transform.position, rotatedVector * 100f, Color.cyan);
    //    if (Physics.Raycast(transform.position, rotatedVector, out hit, movement.magnitude))
    //    {

    //        GameObject hitObject = hit.collider.gameObject;
    //        if (hitObject != this.gameObject)
    //        {
    //            if (hitObject != this.plane)
    //            {

    //                Debug.Log("Hit Object: " + hitObject.name); // print out detected object
    //                if (hit.distance <= detectionThreshold)
    //                {
    //                    isMoving = false;
    //                    return;// bool to deactivate MoveObject because an object is detected
    //                }
    //            }

    //        }

    //    }
    //    //this.transform.position = Vector3.Lerp(this.transform.position, rotatedVector.normalized * 0.64f, 0.5f);
    //    //transform.Translate(movement, Space.World);
    //    this.transform.position = Vector3.MoveTowards(this.transform.position, rotatedVector * 10f, 0.05f);

    //}



    // bleow not working corrrectly

    /*void MoveObject()
    {
        Vector3 targetLocalPosition = rotatedVector.normalized * 0.64f;
        Vector3 worldMoveDirection = transform.TransformDirection(rotatedVector);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, worldMoveDirection, out hit, detectionThreshold))
        {
            if (hit.collider.gameObject != this.gameObject && hit.collider.gameObject != this.plane)
            {
                isMoving = false;
                return;
            }
        }

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetLocalPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.localPosition, targetLocalPosition) < 0.01f)
        {
            isMoving = false;
        }
    }
    */


    // Coroutine for smooth backward animation
    public IEnumerator SmoothResetPosition()
    {
        isResetting = true;
        Vector3 currentPos = transform.position;
        float elapsedTime = 0f;
        float resetDuration = 1f; // Adjust this value to change how long the reset takes

        while (elapsedTime < resetDuration)
        {
            transform.position = Vector3.Lerp(currentPos, startPosition, elapsedTime / resetDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition;
        isResetting = false;
    }

}
