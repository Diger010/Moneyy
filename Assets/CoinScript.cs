using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public int coinValue = 1; // количество очков, начисляемых за подбор монеты

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Coin picked up");
        if (collision.gameObject.CompareTag("Player"))
        {
            // получаем скрипт игрока и добавляем ему очки
            PlayerScore playerScore = collision.gameObject.GetComponent<PlayerScore>();
            playerScore.AddScore(coinValue);

            // уничтожаем монету
            Destroy(gameObject);
        }
    }
}
