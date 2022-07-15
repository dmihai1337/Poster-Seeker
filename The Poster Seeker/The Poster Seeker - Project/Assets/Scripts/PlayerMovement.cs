using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    public TextMeshProUGUI exitMessage;
    public int counter = 0;
    public Transform cam;
    // velocities
    public float moveSpeed = 4f;
    public float jumpSpeed = 20f;
    public float cam_speed = 2.5f;
    // camera rotation <=> player rotation
    public float yaw = 0f;
    // movement vector
    private Vector3 mvmt;
    private bool isGrounded = false;
    private Rigidbody rb = null;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        // locking the cursor middle screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        exitMessage.text = "";
    }
    
    // Update is called once per frame
    void Update()
    {
        if (gameOver())
            exitMessage.text = "Good job! Now jump!";
        showText();
        Move();
    }

    void showText()
    {
        string message = "Posters found: ";
        string count = counter.ToString();
        counterText.text = message + count;
    }

    void Move()
    {
        // horizontal and vertical orientation
        float dirx = Input.GetAxisRaw("Horizontal");
        float dirz = Input.GetAxisRaw("Vertical");
        
        yaw += cam_speed * Input.GetAxis("Mouse X");

        dirx *= - moveSpeed * Time.deltaTime;
        dirz *= - moveSpeed * Time.deltaTime;
        Vector2 input = new Vector2(dirx, dirz);

        // camera relative movement
        Vector3 camF = cam.forward;
        Vector3 camR = cam.right;

        camF.y = 0f;
        camR.y = 0f;
        camF = camF.normalized;
        camR = camR.normalized;

        // relation between input and camera direction
        transform.position += (camF * input.y + camR * input.x);
        
        rb.MovePosition(transform.position);
        this.transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        Jump();  
    }

    void Jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            if (counter == 10)
            {
                SceneManager.LoadScene("Ending");
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }      
        }      
    }

    bool gameOver()
    {
        if (counter == 10)
            return true;
        return false;
    }

    // check if the player is touching the ground
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = true;
    }
    public void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = true;
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = false;
    }
}
