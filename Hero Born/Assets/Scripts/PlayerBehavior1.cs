using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerBehavior1 : MonoBehaviour
{

    private GameBehavior _gameManager;
    public GameObject Bullet;
    public float BulletSpeed = 100f;

    private Rigidbody rb;
    public float moveSpeed = 5f; 
    
    public float sprintSpeed = 100f; 
    
    private bool isJump = false;
    private bool isGrounded = false;
    private bool isShooting;
    
    private bool isSprinting = false; 

    private Vector2 inputVector;

    public Transform CameraTransform;

    void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        
        if (value.isPressed)
        {
            isJump = true;
        }
    }
    
    void OnSprint(InputValue value)
{
    isSprinting = value.isPressed;
}

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
        _gameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();

        if (CameraTransform == null && Camera.main != null)
            CameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            isShooting = true;
        }

        // 점프 처리
        if (isJump && isGrounded)
        {
            // 점프력은 유지 (Vector3.up * 20f)
            rb.AddForce(Vector3.up * 20f, ForceMode.Impulse);
            isJump = false;
        }
    }

    [System.Obsolete]
    void FixedUpdate()
    {
        // 💡 수정된 부분: 현재 속도 결정
        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        // 카메라 기준으로 이동 방향 계산 (수평면만 사용)
        Vector3 move = Vector3.zero;
        if (CameraTransform != null)
        {
            Vector3 camForward = Vector3.ProjectOnPlane(CameraTransform.forward, Vector3.up).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(CameraTransform.right, Vector3.up).normalized;
            Vector3 desiredDir = camRight * inputVector.x + camForward * inputVector.y;
            
            move = desiredDir.normalized * currentSpeed; 
        }
        else
        {
            move = new Vector3(inputVector.x, 0f, inputVector.y).normalized * currentSpeed;
        }

    
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);

        if (isShooting)
        {
            Vector3 aimDir = (CameraTransform != null) ? CameraTransform.forward : transform.forward;

            Vector3 spawnPos = transform.position + Vector3.up * 0.5f + aimDir * 1f;

            Quaternion rot = Quaternion.LookRotation(aimDir);
            GameObject newBullet = Instantiate(Bullet, spawnPos, rot);
            Rigidbody BulletRB = newBullet.GetComponent<Rigidbody>();
            if (BulletRB != null)
                BulletRB.velocity = aimDir.normalized * BulletSpeed;
        }
        isShooting = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player Hit by Enemy!");
        }
        else if (collision.gameObject.CompareTag("HpPickup"))
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.name == "Enemy")
        {
            _gameManager.Hp -= 1;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}