using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    public int damage = 10;
    public Photon.Realtime.Player owner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // если объект, с которым столкнулась пуля, имеет компонент PlayerHealth
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // наносим урон
            playerHealth.TakeDamage(damage);

            // уничтожаем пулю
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
