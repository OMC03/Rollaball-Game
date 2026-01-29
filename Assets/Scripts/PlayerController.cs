using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine.InputSystem.Processors;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI playerLives;
    public GameObject WinTextObject;
    private MeshRenderer mesh;
    private SphereCollider col;

    [SerializeField] bool isDead = false;
    private bool gameOver = false;
    private int count;
    private static int lives = 3;
    public float speed = 0;
    private float movementX;
    private float movementY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
        col = GetComponent<SphereCollider>();
        count = 0;

        WinTextObject.SetActive(false);

        SetCountText();
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

        SetLives();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;

            SetCountText();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            OnDeath();
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnRestart(InputValue value)
    {
        if (isDead && value.isPressed && !gameOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= 11)
        {
            WinTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }

    void SetLives()
    {
        playerLives.text = "Lives: " + lives.ToString();

        if (lives == 0)
        {
            OnDeath();
            gameOver = true;
        }
    }

    void OnDeath()
    {
        mesh.enabled = false;
        col.enabled = false;
        rb.isKinematic = true;

        if (lives > 0)
        {
            lives--;
        }

        WinTextObject.gameObject.SetActive(true);
        WinTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose";
        isDead = true;
    }
}
