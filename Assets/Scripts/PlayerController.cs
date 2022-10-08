using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;
    private AudioSource playerAudio;

    [Header("Start Control")]
    public Vector3 startPoint;
    public Vector3 endPoint;

    private float journeyLength;

    [Header("Jump Control")]
    public int maxJumps = 2;
    public float jumpForce;
    private int numJumps;

    [Header("Game Control")]
    public int scoreMultiplier = 100;
    public Text scoreText;
    public Text speedText;

    [HideInInspector] public bool gameOver = false;
    [HideInInspector] public float speed = 25.0f;

    [HideInInspector] private bool canRestart = false;
    [HideInInspector] private float decimalScore = 0.0f;
    [HideInInspector] private int score = 0;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public Text finalScoreText;

    [Header("Particles")]
    public ParticleSystem explosionParticles;
    public ParticleSystem dirtParticles;

    [Header("Audio")]
    public AudioClip jumpSound;
    public AudioClip crashSound;

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        numJumps = maxJumps;

        transform.position = startPoint;
        gameOver = true;
        StartCoroutine(PlayIntro());
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameOver)
        {
            if (Input.GetButtonDown("Jump") && numJumps > 0)
            {
                numJumps--;
                dirtParticles.Stop();
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                playerAnim.SetTrigger("Jump_trig");
                playerAudio.PlayOneShot(jumpSound, 1.0f);
            }

            bool isMovingFast = Input.GetButton("Submit");
            speedText.enabled = isMovingFast;
            Time.timeScale = isMovingFast ? 2.0f : 1.0f;

            decimalScore += (Time.deltaTime * (isMovingFast ? scoreMultiplier * 2.0f : scoreMultiplier));
            score = Mathf.RoundToInt(decimalScore);
            scoreText.text = string.Format("Score: {0}", score);
        }
        else if(canRestart && Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground") && !gameOver)
        {
            numJumps = maxJumps;
            dirtParticles.Play();
        }
        else if(collision.gameObject.CompareTag("Obstacle") && !gameOver)
        {
            gameOver = true;
            canRestart = true;

            speedText.enabled = false;
            Time.timeScale = 1.0f;

            dirtParticles.Stop();
            explosionParticles.Play();

            playerAudio.PlayOneShot(crashSound, 1.0f);

            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);

            gameOverPanel.SetActive(true);
            finalScoreText.text = string.Format("Final Score: {0}", score);
        }
    }

    IEnumerator PlayIntro()
    {
        float journeyLength = Vector3.Distance(startPoint, endPoint);
        float startTime = Time.time;
        float distanceCovered = (Time.time - startTime) * speed * 0.5f;
        float fractionOfJourney = distanceCovered / journeyLength;

        Time.timeScale = 0.5f;

        while (fractionOfJourney < 1)
        {
            distanceCovered = (Time.time - startTime) * speed * 0.5f;
            fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(startPoint, endPoint, fractionOfJourney);
            yield return null;
        }

        Time.timeScale = 1.0f;
        gameOver = false;
    }

}
