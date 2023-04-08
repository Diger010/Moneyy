
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviourPunCallbacks, IPunObservable
{
    // Переменные для хранения скорости игрока и направления движения
    public float speed = 5f;
    private float xDir = 0f;
    private float yDir = 0f;

    // Ссылки на компоненты игрока
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private PhotonView view;

    // Ссылка на джойстик
    public Joystick joystick;

    // Ссылка на текстовое поле для имени игрока
    public Text textName;
    public GameObject joystickObject;

    // Start вызывается при первом запуске скрипта
    void Start()
    {
        // Получаем ссылки на компоненты игрока
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        view = GetComponent<PhotonView>();

        // Устанавливаем имя игрока в текстовое поле
        textName.text = view.Owner.NickName;

        // Если это локальный игрок, то устанавливаем камеру на него
        if (view.IsMine)
        {
            Camera.main.GetComponent<CameraFollow>().player = gameObject.transform;

            // Показываем джойстик только на локальной машине
            joystickObject.SetActive(true);
        }
        else
        {
            // Скрываем джойстик на удаленных машинах
            joystickObject.SetActive(false);
        }
    }

    // Update вызывается каждый кадр
    void Update()
    {
        // Если это локальный игрок
        if (view.IsMine)
        {
            // Получаем значения от джойстика и устанавливаем направление движения
            xDir = joystick.Horizontal;
            yDir = joystick.Vertical;

            // Устанавливаем скорость игрока в зависимости от направления движения
            rb.velocity = new Vector2(xDir * speed, yDir * speed);

            // Определяем, движется ли игрок в данный момент
            bool isMoving = Mathf.Abs(xDir) > 0 || Mathf.Abs(yDir) > 0;

            // Если игрок движется, то устанавливаем анимацию бега, иначе - анимацию стояния
            anim.SetBool("isRunning", isMoving);

            // Отражаем спрайт игрока в зависимости от направления движения
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
        // Проверяем, что столкнулись с другим игроком
        if (collision.gameObject.CompareTag("Player"))
        {
            // Получаем фотоновский компонент другого игрока
            var otherPlayerPhotonView = collision.gameObject.GetComponent<PhotonView>();

            // Проверяем, что этот компонент не нулевой и это не наш игрок
            if (otherPlayerPhotonView != null && !otherPlayerPhotonView.IsMine)
            {
                // Отправляем RPC для вызова метода Flip у другого игрока, который повернет его текст с ником
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

   