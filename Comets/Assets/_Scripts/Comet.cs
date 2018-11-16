using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : MonoBehaviour {

    [SerializeField] float maxSpeed;
    [SerializeField] float maxSpin;
    [SerializeField] Rigidbody2D cometRigid;
    [SerializeField] GameObject smallComet;
    [SerializeField] int cometSize;

    [Header("Screen Wrapping")]
    [SerializeField] float screenHeight;
    [SerializeField] float screenWidth;

    // Use this for initialization
    void Start () 
    {
        Vector2 speed = new Vector2(Random.Range(-maxSpeed, maxSpeed), Random.Range(-maxSpeed, maxSpeed));
        float spin = Random.Range(-maxSpin, maxSpin);
        cometRigid.AddForce(speed);
        cometRigid.AddTorque(spin);
	}
	
	// Update is called once per frame
	void Update () {
        ScreenWrap();
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bullet"))
        {
            Destroy(other.gameObject);

            if (cometSize == 2)
            {
                //make more small comets
                Instantiate(smallComet, transform.position, transform.rotation);
                Instantiate(smallComet, transform.position, transform.rotation);
            }
            else if (cometSize == 1)
            {
                //add points
            }
            Destroy(gameObject);
        }
    }

    private void ScreenWrap()
    {
        Vector2 cometPos = transform.position;

        if (transform.position.y > screenHeight)
        {
            cometPos.y = -screenHeight;
        }
        if (transform.position.y < -screenHeight)
        {
            cometPos.y = screenHeight;
        }
        if (transform.position.x > screenWidth)
        {
            cometPos.x = -screenWidth;
        }
        if (transform.position.x < -screenWidth)
        {
            cometPos.x = screenWidth;
        }
        transform.position = cometPos;
    }
}
