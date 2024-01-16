using UnityEngine;

public class Monster_lv1_Controller : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveRange = 5f;
    public LayerMask obstacleLayer;
    public LayerMask playerLayer; // Thêm một LayerMask cho người chơi
    private bool movingRight = true;
    [Header("Sprites")]
    public AnimatedSpriteRenderer spriteRendererLeft;
    //public AnimatedSpriteRenderer spriteRendererRight;
    public AnimatedSpriteRenderer spriteRendererDeath;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MoveMonster();
    }

    void MoveMonster()
    {
        Vector2 currentPosition = transform.position;
        Vector2 movement = movingRight ? Vector2.right : Vector2.left;
        currentPosition += movement * moveSpeed * Time.deltaTime;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, movement, 0.2f, obstacleLayer);

        if (hit.collider != null)
        {
            movingRight = !movingRight;
            movement = movingRight ? Vector2.right : Vector2.left;
            currentPosition.x = Mathf.Clamp(currentPosition.x, -moveRange, moveRange);
        }

        if (currentPosition.x >= moveRange || currentPosition.x <= -moveRange)
        {
            movingRight = !movingRight;
            movement = movingRight ? Vector2.right : Vector2.left;
            currentPosition.x = Mathf.Clamp(currentPosition.x, -moveRange, moveRange);
        }

        rb.MovePosition(currentPosition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int playerLayerValue = LayerMask.NameToLayer("Explosion");
        int playerLayer = LayerMask.NameToLayer("Player");
        if (other.gameObject.layer == playerLayerValue) //|| other.gameObject.layer == playerLayer)
        {

            // Nếu quái vật va chạm với người chơi, thực hiện logic chết
            enabled = false;
            spriteRendererLeft.enabled = false;
            spriteRendererDeath.enabled = true;
            HandleGame.Instance.IncreaseScore(2);
            Invoke(nameof(OnDeathSequenceEnded), 1.25f);
        }
    }


    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
        GameManager.Instance.totalMonsters--;

        if (GameManager.Instance.totalMonsters == 0)
            HandleGame.Instance.setPanel("win");
    }

    public void SetActive()
    {
        gameObject.SetActive(true);
        enabled = true;
        spriteRendererLeft.enabled = true;
        spriteRendererDeath.enabled = false;
    }
}