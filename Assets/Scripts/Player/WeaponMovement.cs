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

    private bool collisionEntered = false;

    public void Orbit()
    {
        if (target != null)
        {
            // rotate until close enough
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform != target && (collision.gameObject.tag == "Player"))
        {
            if (!collision.gameObject.GetComponent<Movement>()) return;
            if (collision.gameObject.GetComponent<Movement>().knockback) return;
            GetComponent<BoxCollider2D>().isTrigger = true;
            StartCoroutine(collision.gameObject.GetComponent<Movement>().GetHit(target.gameObject.GetComponent<Enemy>().direction, 0.25f, GetComponent<BoxCollider2D>()));
        }
        else if (collision.transform != target && (collision.gameObject.tag == "Enemy"))
        {
            if (!collision.gameObject.GetComponent<Enemy>()) return;
            if (collision.gameObject.GetComponent<Enemy>().knockback) return;
            GetComponent<BoxCollider2D>().isTrigger = true;
            StartCoroutine(collision.gameObject.GetComponent<Enemy>().GetHit(target.gameObject.GetComponent<Movement>().direction, 0.25f, GetComponent<BoxCollider2D>()));

        }
        else if (collision.gameObject.tag != target.tag && !collisionEntered)
        {
            collisionEntered = true;
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
            rotateLeft = diff < 180f;
            StartCoroutine(moveSword(angle, 0.5f));
        }
    }

}
