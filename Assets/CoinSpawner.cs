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
        // «апускаем корутину SpawnCoin() дл€ спавна монет
        StartCoroutine(SpawnCoin());
    }

    private IEnumerator SpawnCoin()
    {
        while (true)
        {
            // —павним монету на случайной точке спавна
            int randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);
            GameObject coin = PhotonNetwork.Instantiate(coinPrefab.name, spawnPoints[randomSpawnPointIndex].transform.position, Quaternion.identity);

            // ∆дем указанный интервал времени перед следующим спавном
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
