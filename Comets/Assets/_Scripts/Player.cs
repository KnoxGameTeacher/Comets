using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    [Header("Player States")]
    [SerializeField] int lives = 3;
    [SerializeField] bool isInvincible;
    [SerializeField] bool isTripleShotOn;
    GameObject levelManager;

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
    [SerializeField] GameObject leftBarrel;
    [SerializeField] GameObject rightBarrel;
    Coroutine autoFire;
    Coroutine autoFireTriple;

	// Use this for initialization
	void Start () {
        levelManager = GameObject.FindWithTag("level manager");
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
        if (isTripleShotOn == false)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                autoFire = StartCoroutine(AutoFire());
            }
            if (Input.GetButtonUp("Fire1"))
            {
                StopCoroutine(autoFire);
            }
        }
        if (isTripleShotOn == true)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                autoFireTriple = StartCoroutine(AutoFireTriple());
            }
            if (Input.GetButtonUp("Fire1"))
            {
                StopCoroutine(autoFireTriple);
            }
        }
    }

    IEnumerator AutoFire()
    {
        if (Input.GetButtonDown("Fire1"))
            while (true)
            {
                GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
                newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletSpeed);

                yield return new WaitForSeconds(timeBetweenShots);
            }

    }

    IEnumerator AutoFireTriple()
    {

        if (Input.GetButtonDown("Fire1"))
            while (isTripleShotOn == true)
            {
                GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
                newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletSpeed);

                GameObject leftBullet = Instantiate(bullet, leftBarrel.transform.position, leftBarrel.transform.rotation) as GameObject;
                leftBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletSpeed);

                GameObject rightBullet = Instantiate(bullet, rightBarrel.transform.position, rightBarrel.transform.rotation) as GameObject;
                rightBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletSpeed);

                yield return new WaitForSeconds(timeBetweenShots);
            }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isInvincible)
        {
            if (lives > 0)
            {
                StartCoroutine(LoseLife());
            }
            else
            StartCoroutine(PlayerDeath());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Shield PowerUp"))
        {
            Destroy(other.gameObject);
            StartCoroutine(ShieldPowerUp());
        }
        else if (other.CompareTag("Triple PowerUp"))
        {
            Destroy(other.gameObject);
            StartCoroutine(TripleShotPowerup());
        }
    }

    IEnumerator TripleShotPowerup()
    {
        isTripleShotOn = true;
        yield return new WaitForSeconds(5);
        isTripleShotOn = false;
    }

    IEnumerator LoseLife()
    {
        isInvincible = true;
        lives--;
        levelManager.SendMessage("ManageLives", lives);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);
        GameObject explosion = Instantiate(explosionFX, transform.position, transform.rotation);
        yield return new WaitForSeconds(2);
        Destroy(explosion);
        transform.position = new Vector2(0, 0);
        StartCoroutine(ShieldActive());
    }

    IEnumerator ShieldActive()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        isInvincible = false;
    }

    IEnumerator ShieldPowerUp()
    {
        isInvincible = true;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        isInvincible = false;
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
