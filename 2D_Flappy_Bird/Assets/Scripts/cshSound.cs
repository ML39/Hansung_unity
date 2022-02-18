using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshSound : MonoBehaviour
{
    public AudioSource audioSource;
    public int check;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (check == 1)
        {
            audioSource.Play();
            check = 0;
        }
    }
}
