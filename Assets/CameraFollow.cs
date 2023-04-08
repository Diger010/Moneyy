using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public int speed;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private void Update()
    {
        if (player == null) return; // добавляем проверку

        Vector3 playerVector = player.position;
        playerVector.z = -10;
        float clampedX = Mathf.Clamp(playerVector.x, minX, maxX);
        float clampedY = Mathf.Clamp(playerVector.y, minY, maxY);
        Vector3 clampedVector = new Vector3(clampedX, clampedY, playerVector.z);
        transform.position = Vector3.Lerp(transform.position, clampedVector, speed * Time.deltaTime);
    }
}
