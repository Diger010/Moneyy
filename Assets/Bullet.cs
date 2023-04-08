using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    public int damage = 10;
    public Photon.Realtime.Player owner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ������, � ������� ����������� ����, ����� ��������� PlayerHealth
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // ������� ����
            playerHealth.TakeDamage(damage);

            // ���������� ����
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
