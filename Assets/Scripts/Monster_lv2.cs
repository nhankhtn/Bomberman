using UnityEngine;
using System.Collections;

public class Monster_lv2_Controller : MonoBehaviour
{
    public float moveSpeedHorizontal = 2f;
    public float moveSpeedVertical = 2f;
    public float moveRangeHorizontal = 3f;
    public float moveRangeVertical = 5f;
    public LayerMask obstacleLayer;
    public LayerMask playerLayer;

    [Header("Sprites")]
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererTrans;
    public AnimatedSpriteRenderer spriteRendererDeath;

    private bool movingRight = true;
    private bool movingUp = true;
    private bool isTransformed = false;
    private Vector2 initialPosition;

    public float timeToTransform = 5f;
    public float transformedMoveSpeedMultiplier = 3f;
    public float timeToReturnToInitialState = 5f;

    void Start()
    {
        initialPosition = transform.position;
        StartCoroutine(MonsterRoutine());
    }

    IEnumerator MonsterRoutine()
    {
        while (true)
        {
            // Biến hình sau mỗi 5 giây
            yield return new WaitForSeconds(timeToTransform);
            TransformAndMove();

            // Chờ 5 giây để quay về trạng thái ban đầu
            yield return new WaitForSeconds(timeToReturnToInitialState);
            ReturnToInitialState();
        }
    }

    void Update()
    {
        MoveMonster();
    }

    void MoveMonster()
    {
        Vector2 currentPosition = transform.position;

        float currentMoveSpeedHorizontal = moveSpeedHorizontal;
        float currentMoveSpeedVertical = moveSpeedVertical;

        if (isTransformed)
        {
            // Nếu đã biến hình, áp dụng hệ số tốc độ mới
            currentMoveSpeedHorizontal *= transformedMoveSpeedMultiplier;
            currentMoveSpeedVertical *= transformedMoveSpeedMultiplier;
        }

        // Di chuyển theo chiều ngang
        if (movingRight)
            currentPosition.x += currentMoveSpeedHorizontal * Time.deltaTime;
        else
            currentPosition.x -= currentMoveSpeedHorizontal * Time.deltaTime;

        // Di chuyển theo chiều dọc
        if (movingUp)
            currentPosition.y += currentMoveSpeedVertical * Time.deltaTime;
        else
            currentPosition.y -= currentMoveSpeedVertical * Time.deltaTime;

        // Kiểm tra va chạm với vật cản theo chiều ngang
        RaycastHit2D hitHorizontal = Physics2D.Raycast(currentPosition, movingRight ? Vector2.right : Vector2.left, 0.2f, obstacleLayer);

        // Kiểm tra va chạm với vật cản theo chiều dọc
        RaycastHit2D hitVertical = Physics2D.Raycast(currentPosition, movingUp ? Vector2.up : Vector2.down, 0.2f, obstacleLayer);

        if (hitHorizontal.collider != null)
        {
            movingRight = !movingRight;
            currentPosition.x = Mathf.Clamp(currentPosition.x, initialPosition.x - moveRangeHorizontal, initialPosition.x + moveRangeHorizontal);
        }

        if (currentPosition.x >= initialPosition.x + moveRangeHorizontal || currentPosition.x <= initialPosition.x - moveRangeHorizontal)
        {
            movingRight = !movingRight;
            currentPosition.x = Mathf.Clamp(currentPosition.x, initialPosition.x - moveRangeHorizontal, initialPosition.x + moveRangeHorizontal);
        }

        if (hitVertical.collider != null)
        {
            movingUp = !movingUp;
            currentPosition.y = Mathf.Clamp(currentPosition.y, initialPosition.y - moveRangeVertical, initialPosition.y + moveRangeVertical);
        }

        if (currentPosition.y >= initialPosition.y + moveRangeVertical || currentPosition.y <= initialPosition.y - moveRangeVertical)
        {
            movingUp = !movingUp;
            currentPosition.y = Mathf.Clamp(currentPosition.y, initialPosition.y - moveRangeVertical, initialPosition.y + moveRangeVertical);
        }

        transform.position = currentPosition;
    }

    void TransformAndMove()
    {
        isTransformed = true;

        // Tắt render của spriteRendererLeft và mở render của spriteRendererDeath
        spriteRendererLeft.enabled = false;
        spriteRendererDeath.enabled = false;
        spriteRendererTrans.enabled = true;
    }

    void ReturnToInitialState()
    {
        // Reset các biến khác
        isTransformed = false;

        // Tắt render của spriteRendererTrans và mở render của spriteRendererLeft
        spriteRendererLeft.enabled = true;
        spriteRendererDeath.enabled = false;
        spriteRendererTrans.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int playerLayerValue = LayerMask.NameToLayer("Explosion");
        int playerLayer = LayerMask.NameToLayer("Player");
        if (other.gameObject.layer == playerLayerValue) //|| other.gameObject.layer == playerLayer)
        {
            // Nếu quái vật va chạm với người chơi, thực hiện logic chết
            StartCoroutine(DeathSequence());
        }
    }

    IEnumerator DeathSequence()
    {
        // Tắt script và hiển thị spriteRendererDeath
        enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererDeath.enabled = true;
        spriteRendererTrans.enabled = false;
        HandleGame.Instance.IncreaseScore(3);

        // Chờ 1.25 giây
        yield return new WaitForSeconds(1.25f);

        // Reset các biến khác
        isTransformed = false;

        // Tắt render của spriteRendererDeath
        spriteRendererDeath.enabled = false;

        // Disable game object hoặc thực hiện các hành động khác
        gameObject.SetActive(false);
        GameManager.Instance.totalMonsters--;

        if (GameManager.Instance.totalMonsters == 0)
            HandleGame.Instance.setPanel("win");
    }
}