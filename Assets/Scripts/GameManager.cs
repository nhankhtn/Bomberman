
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject player;
    public int totalMonsters = 7;
    private int playerLives = 1; // Số mạng hiện tại của người chơi
    private int maxLives = 5; // Số mạng tối đa cho người chơi
    private Vector3 respawnPosition = new Vector3(-6f, 5f, 0f);
    [SerializeField] public TextMeshProUGUI namePlayer;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (namePlayer != null)
                namePlayer.text = PlayerPrefs.GetString("PlayerName", "No Name");
        }
    }
    public void SetRespawnPosition(Vector3 position)
    {
        respawnPosition = position;
    }

    public void AddLifeToPlayer(MovementController playerController)
    {
        if (playerLives < maxLives)
        {
            playerLives++;
            HandleGame.Instance.heartText.text = playerLives.ToString();
        }
    }

    public void RespawnPlayer(GameObject player)
    {
        MovementController playerController = player.GetComponent<MovementController>();
        if (playerLives > 1)
        {
            playerLives--;
            HandleGame.Instance.heartText.text = playerLives.ToString();
            if (playerController != null)
            {
                // Hồi sinh nhân vật
                playerController.RespawnPlayer(respawnPosition);

                // Đặt hướng mặt về xuống khi hồi sinh
                playerController.SetDirection(Vector2.down, playerController.spriteRendererDown);
            }
        }
        else
        {
            HandleGame.Instance.setPanel("lose");
        }
    }
}