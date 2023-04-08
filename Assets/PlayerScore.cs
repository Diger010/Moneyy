using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class PlayerScore : MonoBehaviourPunCallbacks
{
    public int score = 0;
    public Text scoreText;

    private void Start()
    {
        if (photonView.IsMine)
        {
            // игрок может изменять только свои очки
            scoreText.color = Color.green;
        }
        else
        {
            // очки противника нельзя изменять
            scoreText.color = Color.red;
            scoreText.text = "Opponent: " + score;
        }
    }

    public void AddScore(int value)
    {
        photonView.RPC("AddScoreRPC", RpcTarget.AllBuffered, value);
    }

    [PunRPC]
    private void AddScoreRPC(int value)
    {
        score += value;
        scoreText.text = "Score: " + score;
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
