using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshButtonManager : MonoBehaviour
{
    Animator BirdAnimator;
    GameObject bird;

    // Start is called before the first frame update
    void Start()
    {
        bird = GameObject.Find("Bird");
        BirdAnimator = bird.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Super()
    {
        BirdAnimator.SetBool("Super", true);
        bird.GetComponent<cshFlyBird>().super = true;
        bird.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    public void Normal()
    {
        BirdAnimator.SetBool("Super", false);
        bird.GetComponent<cshFlyBird>().super = false;
        bird.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
    }
}
