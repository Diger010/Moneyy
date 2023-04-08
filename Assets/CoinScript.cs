using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public int coinValue = 1; // ���������� �����, ����������� �� ������ ������

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Coin picked up");
        if (collision.gameObject.CompareTag("Player"))
        {
            // �������� ������ ������ � ��������� ��� ����
            PlayerScore playerScore = collision.gameObject.GetComponent<PlayerScore>();
            playerScore.AddScore(coinValue);

            // ���������� ������
            Destroy(gameObject);
        }
    }
}
