using System.Collections;
using UnityEngine;
using Photon.Pun;

public class CoinSpawner : MonoBehaviourPunCallbacks
{
    public GameObject coinPrefab;
    public float spawnInterval = 5f;
    public GameObject[] spawnPoints;

    private void Start()
    {
        // ��������� �������� SpawnCoin() ��� ������ �����
        StartCoroutine(SpawnCoin());
    }

    private IEnumerator SpawnCoin()
    {
        while (true)
        {
            // ������� ������ �� ��������� ����� ������
            int randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);
            GameObject coin = PhotonNetwork.Instantiate(coinPrefab.name, spawnPoints[randomSpawnPointIndex].transform.position, Quaternion.identity);

            // ���� ��������� �������� ������� ����� ��������� �������
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
