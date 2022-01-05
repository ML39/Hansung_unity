using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshShooting : MonoBehaviour
{

    public Transform firePoint;
    public GameObject arrowPrefab;
    public Animator animator;

    public float arrowForce = 20f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //animator.SetBool("Shoot", true);
            //Shoot();
        }
    }
    public void ButtonDown()
    {
        animator.SetBool("Shoot", true);
        Shoot();
    }

    void Shoot()
    {
        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.Euler(0, 0, 0);
        if (animator.GetFloat("Vertical") == 1)
        {
            pos = firePoint.up * arrowForce;
            rot = Quaternion.Euler(0, 0, 0f);
        }
        else if (animator.GetFloat("Vertical") == -1)
        {
            pos = -firePoint.up * arrowForce;
            rot = Quaternion.Euler(0, 0, 180f);
        }
        else if (animator.GetFloat("Horizontal") == 1)
        {
            pos = firePoint.right * arrowForce;
            rot = Quaternion.Euler(0, 0, -90f);
        }
        else if (animator.GetFloat("Horizontal") == -1)
        {
            pos = -firePoint.right * arrowForce;
            rot = Quaternion.Euler(0, 0, 90f);
        }
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, rot);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(pos, ForceMode2D.Impulse);
    }

    void EndAnimation()
    {
        animator.SetBool("Shoot", false);
    }
}
