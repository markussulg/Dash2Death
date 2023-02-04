using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMovement : MonoBehaviour
{
    public Transform target;
    public float orbitDistance = 1.0f;
    public float orbitDegreesPerSec = 180.0f;
    public bool isRotating = false;
    public float targetAngle;
    public bool rotateLeft = false;

    private Rigidbody2D rb;
    private bool collisionEntered = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Orbit()
    {
        if (target != null)
        {
            //print(rotateLeft);
            //print(transform.eulerAngles);
            //print(direction);
            // Keep us at orbitDistance from target
            if (Mathf.Abs(transform.eulerAngles.z - Mathf.Abs(targetAngle)) > 1f)
            {
                transform.position = target.position + (transform.position - target.position).normalized * orbitDistance;
                transform.RotateAround(target.position, rotateLeft? Vector3.forward : Vector3.back, orbitDegreesPerSec * Time.deltaTime);
            } else
            {
                isRotating = false;  
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isRotating && !collisionEntered)
        {
            Orbit();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform != target && (collision.gameObject.tag == "Player"))
        {
            if (collision.gameObject.GetComponent<Movement>().knockback) return;
            print("collide");
            collision.gameObject.GetComponent<Movement>().knockback = true;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(target.gameObject.GetComponent<Enemy>().direction * 1000f);;
            GetComponent<BoxCollider2D>().isTrigger = true;
            StartCoroutine(wait(collision.gameObject.GetComponent<Movement>(), 0.25f));

        }
        else if (collision.transform != target && (collision.gameObject.tag == "Enemy"))
        {
            if (collision.gameObject.GetComponent<Enemy>().knockback) return;
            print("collide");
            collision.gameObject.GetComponent<Enemy>().knockback = true;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(target.gameObject.GetComponent<Movement>().direction * 1000f); ;
            GetComponent<BoxCollider2D>().isTrigger = true;
            StartCoroutine(wait(collision.gameObject.GetComponent<Enemy>(), 0.25f));

        }
        else if (collision.gameObject.tag != "Player" && !collisionEntered)
        {
            collisionEntered = true;
            print("terrainCollider");
            isRotating = false;
            float angle = Vector3.Angle(Vector3.right, collision.contacts[0].normal);
            if (collision.contacts[0].normal.y < 0.0f)
            {
                angle = 360f - angle;
            }
            float diff = angle - transform.eulerAngles.z;
            if (diff < 0f)
            {
                diff = diff + 360;
            }
            if (diff < 180f)
            {
                rotateLeft = true;
            }
            else
            {
                rotateLeft = false;
            }
            StartCoroutine(moveSword(angle, 0.5f));
            //rb.AddForce(collision.contacts[0].normal * 10f);
        }
    }
    IEnumerator wait(Movement movement, float duration)
    {
        yield return new WaitForSeconds(duration);
        movement.knockback = false;
        GetComponent<BoxCollider2D>().isTrigger = false;
    }
    IEnumerator wait(Enemy movement, float duration)
    {
        yield return new WaitForSeconds(duration);
        movement.knockback = false;
        GetComponent<BoxCollider2D>().isTrigger = false;
    }
    IEnumerator moveSword(float direction, float duration)
    {
        float timeAggregate = 0f;
        bool stop = false;
        while (duration > timeAggregate && !stop)
        {
            timeAggregate += Time.deltaTime;

            if (Mathf.Abs(transform.eulerAngles.z - Mathf.Abs(direction)) > 1f)
            {
                transform.position = target.position + (transform.position - target.position).normalized * orbitDistance;
                transform.RotateAround(target.position, rotateLeft ? Vector3.forward : Vector3.back, 2f * orbitDegreesPerSec * Time.deltaTime);
            }
            else
            {
                stop = true;                

            }
            yield return null;

        }
        collisionEntered = false;

    }
}
