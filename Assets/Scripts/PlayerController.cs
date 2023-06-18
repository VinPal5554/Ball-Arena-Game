using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public bool hasPowerup = false;
    private float powerupStrength = 15.0f;
    public GameObject powerupIndicator;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");

        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed);

        powerupIndicator.transform.position = playerRb.transform.position + new Vector3(0, -0.5f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCounterdownRoutine());
            powerupIndicator.gameObject.SetActive(true);
        }
    }

    IEnumerator PowerupCounterdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidBody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRigidBody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);


            Debug.Log("Collided with " + collision.gameObject.name + " and powerup set to " + hasPowerup);

        }
    }

}
