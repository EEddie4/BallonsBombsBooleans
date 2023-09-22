using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce = 50f;

    public float bounceOffGroundForce = 250.0f;
    public float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;

    public AudioClip bounceSound;
    public AudioClip moneySound;
    public AudioClip explodeSound;

    public float heightBound =14.0f;
    public bool isLowEnough;

    public float heightBoundOffset = 2.0f;



    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if(transform.position.y < heightBound)
        {
            isLowEnough =true;
        }
        else if (transform.position.y > heightBound){
            isLowEnough = false;
            transform.position = new Vector3 (transform.position.x , heightBound, transform.position.z);
            playerRb.AddForce(Vector3.down * heightBoundOffset, ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.Space) && !gameOver && isLowEnough == true )
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }

        
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }

        else if (other.gameObject.CompareTag("Ground"))
        {
            playerRb.AddForce(Vector3.up * bounceOffGroundForce);
            playerAudio.PlayOneShot(bounceSound, 1.0f);
        }

    }

}
