using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {


    [Header("Player Movement")]
    [SerializeField] Rigidbody2D playerRigid;
    [SerializeField] float yVelocity;
    [SerializeField] float xRotation;
    private float thrustInput;
    private float turnInput;

    [Header("Screen Wrapping")]
    [SerializeField] float screenHeight;
    [SerializeField] float screenWidth;

    [Header("FX")]
    [SerializeField] AudioClip deathSound;
    [SerializeField] GameObject explosionFX;
    [SerializeField] Animator animator;

    [Header("Shooting")]
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] float timeBetweenShots = 1f;
    Coroutine autoFire;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        PlayerMovement();
        ScreenWrap();
        Fire();
	}

    private void ScreenWrap()
    {
        Vector2 playerPos = transform.position;

        if (transform.position.y > screenHeight)
        {
            playerPos.y = -screenHeight;
        }
        if (transform.position.y < -screenHeight)
        {
            playerPos.y = screenHeight;
        }
        if (transform.position.x > screenWidth)
        {
            playerPos.x = -screenWidth;
        }
        if (transform.position.x < -screenWidth)
        {
            playerPos.x = screenWidth;
        }
        transform.position = playerPos;
    }

    private void PlayerMovement()
    {
        thrustInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        animator.SetFloat("Thrust", thrustInput);
        animator.SetFloat("Turn", turnInput);

        playerRigid.AddRelativeForce(Vector2.up * thrustInput * yVelocity * Time.deltaTime);
        playerRigid.AddTorque(-turnInput * xRotation * Time.deltaTime);
    }

    private void Fire()
    {
         if(Input.GetButtonDown("Fire1"))
        {
            autoFire = StartCoroutine(AutoFire());
        }
        if(Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(autoFire);
        }
    }

    IEnumerator AutoFire()
    {
        if(Input.GetButtonDown("Fire1"))
            while(true)
        {
                GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
                newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletSpeed);

                yield return new WaitForSeconds(timeBetweenShots);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(PlayerDeath());
    }

    IEnumerator PlayerDeath()
    {
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);
        Instantiate(explosionFX, transform.position, transform.rotation);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        SceneManager.LoadScene("GameOver");
    }
}
