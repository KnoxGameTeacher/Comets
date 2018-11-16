using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Screen Wrapping")]
    [SerializeField] float screenHeight;
    [SerializeField] float screenWidth;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ScreenWrap();
    }
    private void ScreenWrap()
    {

        if (transform.position.y > screenHeight)
        {
            Destroy(gameObject);
        }
        if (transform.position.y < -screenHeight)
        {
            Destroy(gameObject);
        }
        if (transform.position.x > screenWidth)
        {
            Destroy(gameObject);
        }
        if (transform.position.x < -screenWidth)
        {
            Destroy(gameObject);
        }

    }
}
