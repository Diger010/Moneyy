
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviourPunCallbacks, IPunObservable
{
    // ���������� ��� �������� �������� ������ � ����������� ��������
    public float speed = 5f;
    private float xDir = 0f;
    private float yDir = 0f;

    // ������ �� ���������� ������
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private PhotonView view;

    // ������ �� ��������
    public Joystick joystick;

    // ������ �� ��������� ���� ��� ����� ������
    public Text textName;
    public GameObject joystickObject;

    // Start ���������� ��� ������ ������� �������
    void Start()
    {
        // �������� ������ �� ���������� ������
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        view = GetComponent<PhotonView>();

        // ������������� ��� ������ � ��������� ����
        textName.text = view.Owner.NickName;

        // ���� ��� ��������� �����, �� ������������� ������ �� ����
        if (view.IsMine)
        {
            Camera.main.GetComponent<CameraFollow>().player = gameObject.transform;

            // ���������� �������� ������ �� ��������� ������
            joystickObject.SetActive(true);
        }
        else
        {
            // �������� �������� �� ��������� �������
            joystickObject.SetActive(false);
        }
    }

    // Update ���������� ������ ����
    void Update()
    {
        // ���� ��� ��������� �����
        if (view.IsMine)
        {
            // �������� �������� �� ��������� � ������������� ����������� ��������
            xDir = joystick.Horizontal;
            yDir = joystick.Vertical;

            // ������������� �������� ������ � ����������� �� ����������� ��������
            rb.velocity = new Vector2(xDir * speed, yDir * speed);

            // ����������, �������� �� ����� � ������ ������
            bool isMoving = Mathf.Abs(xDir) > 0 || Mathf.Abs(yDir) > 0;

            // ���� ����� ��������, �� ������������� �������� ����, ����� - �������� �������
            anim.SetBool("isRunning", isMoving);

            // �������� ������ ������ � ����������� �� ����������� ��������
            if (xDir < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (xDir > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���������, ��� ����������� � ������ �������
        if (collision.gameObject.CompareTag("Player"))
        {
            // �������� ����������� ��������� ������� ������
            var otherPlayerPhotonView = collision.gameObject.GetComponent<PhotonView>();

            // ���������, ��� ���� ��������� �� ������� � ��� �� ��� �����
            if (otherPlayerPhotonView != null && !otherPlayerPhotonView.IsMine)
            {
                // ���������� RPC ��� ������ ������ Flip � ������� ������, ������� �������� ��� ����� � �����
                otherPlayerPhotonView.RPC("Flip", RpcTarget.AllBuffered);
            }
        }
    }
    public void Flip()
    {
        textName.transform.Rotate(0f, 180f, 0f);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}

   