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
            // ����� ����� �������� ������ ���� ����
            scoreText.color = Color.green;
        }
        else
        {
            // ���� ���������� ������ ��������
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
        // ���� ��������� ����� �� �������, ��������� ����
        if (photonView.IsMine)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}
