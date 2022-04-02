using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private Transform characterController;

    [SerializeField]
    private int movementSpeed;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 horizontalMovement = Vector2.right * Input.GetAxis("Horizontal");

        characterController.Translate(horizontalMovement * movementSpeed * Time.deltaTime, Space.World);
    }

    
}
