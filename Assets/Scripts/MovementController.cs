

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    public static MovementController Instance;
    private Rigidbody2D rb;
    private Vector2 direction = Vector2.down;
    public float speed = 5f;
    private Vector3 respawnPosition = new Vector3(-6f, 5f, 0f);


    [Header("Input")]
    public KeyCode inputUp;
    public KeyCode inputDown;
    public KeyCode inputLeft;
    public KeyCode inputRight;

    [Header("Sprites")]
    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererRight;
    public AnimatedSpriteRenderer spriteRendererDeath;
    private AnimatedSpriteRenderer activeSpriteRenderer;

    AudioManager audioManager;
    private void Awake()
    {
        if (MovementController.Instance == null)
        {
            MovementController.Instance = this;
        }
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        rb = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;
    }

    private void Update()
    {
        if (Input.GetKey(inputUp))
        {
            SetDirection(Vector2.up, spriteRendererUp);
        }
        else if (Input.GetKey(inputDown))
        {
            SetDirection(Vector2.down, spriteRendererDown);
        }
        else if (Input.GetKey(inputLeft))
        {
            SetDirection(Vector2.left, spriteRendererLeft);
        }
        else if (Input.GetKey(inputRight))
        {
            SetDirection(Vector2.right, spriteRendererRight);
        }
        else
        {
            SetDirection(Vector2.zero, activeSpriteRenderer);
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        Vector2 translation = speed * Time.fixedDeltaTime * direction;

        rb.MovePosition(position + translation);
    }

    public void SetDirection(Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer)
    {
        direction = newDirection;

        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        activeSpriteRenderer = spriteRenderer;
        activeSpriteRenderer.idle = direction == Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int explosionLayer = LayerMask.NameToLayer("Explosion");
        int monsterLayer = LayerMask.NameToLayer("Monster");

        if (other.gameObject.layer == explosionLayer)
        {
            DeathSequence();
        }
        else if (other.gameObject.layer == monsterLayer)
        {
            // Đối tượng va chạm là quái vật
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        audioManager.PlaySFX(audioManager.deathEffect);
        enabled = false;
        GetComponent<BombController>().enabled = false;

        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererDeath.enabled = true;

        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }
    private void OnDeathSequenceEnded()
    {
        GameManager.Instance.SetRespawnPosition(respawnPosition);
        GameManager.Instance.RespawnPlayer(gameObject);
    }
    public void RespawnPlayer(Vector3 respawnPosition)
    {
        enabled = true;
        GetComponent<BombController>().enabled = true;

        spriteRendererUp.enabled = true;
        spriteRendererDown.enabled = true;
        spriteRendererLeft.enabled = true;
        spriteRendererRight.enabled = true;
        spriteRendererDeath.enabled = false;

        // Thiết lập vị trí hồi sinh
        transform.position = respawnPosition;
    }

}