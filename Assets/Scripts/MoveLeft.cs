using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private float leftBound = -15.0f;
    private PlayerController player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player && !player.gameOver) transform.Translate(Vector3.left * Time.deltaTime * player.speed, Space.World);
        if (transform.position.x < leftBound && (gameObject.CompareTag("Obstacle") || gameObject.CompareTag("Parent"))) Destroy(gameObject);
    }
}
