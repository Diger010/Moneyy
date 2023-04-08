using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviourPun
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 20f;
    public KeyCode shootKey = KeyCode.Space;
    public GameObject joystickPrefab;

    public int maxHealth = 100;
    public int currentHealth;

    public Text healthText;
    private Rigidbody2D rb;

    private void Start()
    {
        // получаем компонент Rigidbody2D у игрока
        rb = GetComponent<Rigidbody2D>();

        // создаем экземпляр джойстика из префаба
        GameObject joystickObject = Instantiate(joystickPrefab, transform.position, Quaternion.identity);

        // инициализируем текущее здоровье
        currentHealth = maxHealth;

        // обновляем значение на шкале здоровья
        UpdateHealthText();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if ((Input.GetKeyDown(shootKey) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)))
        {
            // получаем направление движения игрока
            Vector2 direction = rb.velocity.normalized;

            // создаем пулю
            photonView.RPC("SpawnBullet", RpcTarget.All, direction);
        }
    }

    [PunRPC]
    private void SpawnBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = direction * bulletSpeed;
        bulletScript.owner = photonView.Owner; // Устанавливаем владельца пули
        Destroy(bullet, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Получаем информацию о пуле
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();

            // Проверяем, что пулю выстрелил другой игрок, а не текущий локальный игрок
            if (bullet.owner != photonView.Owner)
            {
                if (photonView.IsMine)
                {
                    // Отнимаем здоровье при попадании пули
                    currentHealth -= 10;

                    // Обновляем значение на шкале здоровья
                    UpdateHealthText();

                    // Проверяем, если здоровье меньше или равно нулю, уничтожаем игрока
                    if (currentHealth <= 0)
                    {
                        photonView.RPC("DestroyPlayer", RpcTarget.All);
                    }
                }
            }
        }
    }

    [PunRPC]
    private void DestroyPlayer()
    {
        Destroy(gameObject);
    }

    private void UpdateHealthText()
    {
        healthText.text = "Health: " + currentHealth;
    }
}
