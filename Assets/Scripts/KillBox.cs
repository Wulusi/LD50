using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterController player = collision.gameObject.GetComponent<CharacterController>();

        if (player != null)
        {
            player.killPlayer();
            Destroy(gameObject);
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
