using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }
    private void FixedUpdate()
    {
        MoveProjectile();
    }
    private void MoveProjectile()
    {
        if (rb != null)
            rb.velocity = transform.right * (projectileSpeed * Time.fixedDeltaTime);
    }
}
