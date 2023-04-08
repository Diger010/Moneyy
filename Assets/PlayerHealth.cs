using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
    public int maxHealth = 100;
    public int currentHealth;

    public Text healthText;

    private void Start()
    {
        currentHealth = maxHealth;

        if (photonView.IsMine)
        {
            // игрок может изменять только свои очки
            healthText.color = Color.green;
        }
        else
        {
            // очки противника нельзя изменять
            healthText.color = Color.red;
            healthText.text = "Opponent: " + currentHealth + "/" + maxHealth;
        }

        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        healthText.text = "Health: " + currentHealth + "/" + maxHealth;
    }

    public void TakeDamage(int damage)
    {
        photonView.RPC("TakeDamageRPC", RpcTarget.AllBuffered, damage);
    }

    [PunRPC]
    private void TakeDamageRPC(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            UpdateHealthText();
        }
    }

    private void Die()
    {
        // уничтожаем игрока
        PhotonNetwork.Destroy(gameObject);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // если противник вышел из комнаты, завершаем игру
        if (photonView.IsMine)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}

