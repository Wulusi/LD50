using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UnityCustomEvent : UnityEngine.Events.UnityEvent
{

}
public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private Transform characterController;

    [SerializeField]
    private int movementSpeed;

    [SerializeField]
    SpriteRenderer sprite;

    [SerializeField]
    private GameObject crossHair, projectile;

    private GameObject savedCrosshair;

    [SerializeField]
    private Transform gunBarrel;

    [SerializeField]
    private float fireRate;

    [SerializeField]
    private Animator animator;

    public UnityCustomEvent fireAtTarget;

    private float HVelocity;
    [SerializeField]
    private float HAcceleration, HDeceleration, HMinSpeed, HMaxSpeed;

    private bool isLastInputNegative;
    private bool routineActive;
    private bool moveUpdate;

    private Vector3 PlayerInput;
    private float LastPlayerInput;

    private Rigidbody2D _rb;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        characterController = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
        savedCrosshair = Instantiate(crossHair, this.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 horizontalMovement = Vector2.right * Input.GetAxis("Horizontal");

        if (Input.GetAxis("Horizontal") != 0)
        {
            moveUpdate = true;
        }

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            sprite.flipX = false;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            sprite.flipX = true;
        }

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        characterController.Translate(horizontalMovement * movementSpeed * Time.deltaTime, Space.World);
        //GetMoveUpdate();
        //Movement();
        //_rb.MovePosition(transform.position + PlayerInput.normalized * HVelocity);

        moveCrossHair();
        rotateGunBarrel();

        if (Input.GetMouseButtonDown(0))
        {
            CoolDown(fireRate, fireAtTarget);
        }
    }
    private void GetMoveUpdate()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            moveUpdate = true;
            PlayerInput = new Vector3(Input.GetAxis("Horizontal"), PlayerInput.y);
        }
        else
        {
            moveUpdate = false;
        }
    }
    private void Movement()
    {
        //Horizontal Movement Controller
        //Velocity Magnitude calculation
        HVelocity += PlayerInput.magnitude * HAcceleration * Time.deltaTime;
        HVelocity = Mathf.Clamp(HVelocity, HMinSpeed, HMaxSpeed);

        if (PlayerInput.x != 0)
        {
            LastPlayerInput = PlayerInput.x;
        }
        //DeAccelerate
        if (!moveUpdate)
        {
            HVelocity -= HDeceleration * Time.deltaTime;
            HVelocity = Mathf.Clamp(HVelocity, HMinSpeed, HMaxSpeed);
        }
    }

    private void moveCrossHair()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        savedCrosshair.transform.position = mousePosition;
    }
    private void rotateGunBarrel()
    {
        if (gunBarrel != null)
        {
            Vector3 targetDir = savedCrosshair.transform.position - gunBarrel.transform.position;
            Vector3 RotatedTarget = Quaternion.Euler(0, 0, 90) * targetDir;

            Vector3 newDir = Vector3.RotateTowards(gunBarrel.forward, RotatedTarget, 6f * Time.deltaTime, 0f);
            gunBarrel.rotation = Quaternion.LookRotation(gunBarrel.forward, newDir.normalized);
        }
    }
    public void ShootProjecTile()
    {
        Instantiate(projectile, gunBarrel.transform.position, gunBarrel.transform.rotation);
    }

    public void killPlayer()
    {
        Destroy(savedCrosshair);
        Destroy(gameObject);
    }

    public void CoolDown(float duration, UnityCustomEvent eventToInvoke)
    {
        StartCoroutine(Countdown(duration, eventToInvoke));
    }
    public IEnumerator Countdown(float duration, UnityCustomEvent eventToInvoke)
    {
        if (!routineActive)
        {
            routineActive = true;
            float totalTime = 0;
            while (totalTime <= duration)
            {
                totalTime += Time.deltaTime;
                yield return null;
            }
            routineActive = false;
            eventToInvoke?.Invoke();
        }
    }
}
