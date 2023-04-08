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
        // �������� ��������� Rigidbody2D � ������
        rb = GetComponent<Rigidbody2D>();

        // ������� ��������� ��������� �� �������
        GameObject joystickObject = Instantiate(joystickPrefab, transform.position, Quaternion.identity);

        // �������������� ������� ��������
        currentHealth = maxHealth;

        // ��������� �������� �� ����� ��������
        UpdateHealthText();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if ((Input.GetKeyDown(shootKey) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)))
        {
            // �������� ����������� �������� ������
            Vector2 direction = rb.velocity.normalized;

            // ������� ����
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
        bulletScript.owner = photonView.Owner; // ������������� ��������� ����
        Destroy(bullet, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // �������� ���������� � ����
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();

            // ���������, ��� ���� ��������� ������ �����, � �� ������� ��������� �����
            if (bullet.owner != photonView.Owner)
            {
                if (photonView.IsMine)
                {
                    // �������� �������� ��� ��������� ����
                    currentHealth -= 10;

                    // ��������� �������� �� ����� ��������
                    UpdateHealthText();

                    // ���������, ���� �������� ������ ��� ����� ����, ���������� ������
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
