using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAutoDestroy : MonoBehaviour
{
    [SerializeField]
    GroundTile tile;

    private void Start()
    {
        tile = GetComponentInParent<GroundTile>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterController player = collision.GetComponent<CharacterController>();
        if (player != null)
        {
            tile.startAutoDestroyTime();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CharacterController player = collision.GetComponent<CharacterController>();
        if (player != null)
        {
            tile.disableAutoDestroy();
        }
    }
}
