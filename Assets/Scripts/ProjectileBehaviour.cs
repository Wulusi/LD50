using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    Rigidbody2D rb;

    GameManager GM;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    public void setGM(GameManager _gm)
    {
        GM = _gm;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Meteor bulletTarget = collision.GetComponent<Meteor>();

        if (bulletTarget != null)
        {
            bulletTarget.DestroyMeteor();
            Destroy(gameObject);

            if (GM != null)
            {
                GM.IncreaseScore();
            }
        }
    }
}
