using System.Collections;
using UnityEngine;
using System;

[System.Serializable]
public class UnityCustomEvent : UnityEngine.Events.UnityEvent
{

}
public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private Transform characterController, feetPosition;

    [SerializeField]
    private int movementSpeed, jumpForce;

    [SerializeField]
    SpriteRenderer sprite, arm;

    [SerializeField]
    private GameObject crossHair, projectile;

    private GameObject savedCrosshair;

    [SerializeField]
    private Transform gunBarrel, gunTip;

    [SerializeField]
    private float fireRate;
    private float fireTimer;

    [SerializeField]
    private Animator animator;

    public UnityCustomEvent fireAtTarget;

    private float HVelocity;
    [SerializeField]
    private float HAcceleration, HDeceleration, HMinSpeed, HMaxSpeed;

    [SerializeField]
    private AudioSource laserFire;

    private bool isLastInputNegative;
    private bool routineActive;
    private bool moveUpdate;
    private Vector3 PlayerInput;
    private float LastPlayerInput;
    private Rigidbody2D _rb;
    private GameManager GM;

    public event Action onPlayerKilled;

    Vector2 horizontalMovement;
    [SerializeField]
    private float jumpCircleRadius;
    [SerializeField]
    bool isGrounded;
    private float jumpTimeCounter;
    public float jumpTime;
    public bool isJumping;

    public LayerMask m_LayerMask;
    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType<GameManager>();
        Cursor.visible = false;
        characterController = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
        savedCrosshair = Instantiate(crossHair, this.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMovement = Vector2.right * Input.GetAxis("Horizontal");

        if (Input.GetAxis("Horizontal") != 0)
        {
            moveUpdate = true;
        }

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            sprite.flipX = false;
            arm.flipX = false;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            sprite.flipX = true;
            arm.flipX = true;
        }

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }


        //GetMoveUpdate();
        //Movement();
        //_rb.MovePosition(transform.position + PlayerInput.normalized * HVelocity);

        moveCrossHair();
        rotateGunBarrel();

        if (Input.GetMouseButton(0))
        {
            //CoolDown(fireRate, fireAtTarget);
            //StartCoroutine(Shake(0.1f, 0.2f));
            FireBullet();
        }

        //For Vertical Variable Jump
        isGrounded = Physics2D.OverlapCircle(feetPosition.position, jumpCircleRadius, m_LayerMask);

        Debug.Log("Grounded: " + isGrounded);

        if (isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            _rb.velocity = Vector2.up * jumpForce;
            animator.speed = 4.0f;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if(jumpTimeCounter > 0)
            {
                _rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            } else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            animator.speed = 1.0f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(feetPosition.position, jumpCircleRadius);
    }

    private void FixedUpdate()
    {
        characterController.Translate(horizontalMovement * movementSpeed * Time.deltaTime, Space.World);
        _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y);
    }
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = gunBarrel.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1, 1) * magnitude;
            gunBarrel.localPosition = new Vector3(x, originalPos.y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        gunBarrel.localPosition = originalPos;
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
        GameObject bullet = Instantiate(projectile, gunTip.transform.position, gunTip.transform.rotation);
        bullet.GetComponent<ProjectileBehaviour>().setGM(GM);
    }

    public void killPlayer()
    {
        onPlayerKilled?.Invoke();
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
    private void FireBullet()
    {
        if (fireTimer < Time.time)
        {
            fireTimer = Time.time + fireRate;
            laserFire.Play();
            fireAtTarget?.Invoke();
        }
    }

    public void changeFireRate(float change)
    {
        if (fireRate >= 0.15f)
        {
            fireRate += change;
        }
    }
}
