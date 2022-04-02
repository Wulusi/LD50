using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private Transform characterController;

    [SerializeField]
    private int movementSpeed;

    [SerializeField]
    SpriteRenderer sprite;

    private bool isLastInputNegative;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 horizontalMovement = Vector2.right * Input.GetAxis("Horizontal");

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            sprite.flipX = false;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            sprite.flipX = true;
        }

        if (Mathf.Sign(Input.GetAxis("Horizontal")) != 0 && Mathf.Sign(Input.GetAxis("Horizontal")) > 0)
        {
            isLastInputNegative = false;
        }

        characterController.Translate(horizontalMovement * movementSpeed * Time.deltaTime, Space.World);
    }
}
