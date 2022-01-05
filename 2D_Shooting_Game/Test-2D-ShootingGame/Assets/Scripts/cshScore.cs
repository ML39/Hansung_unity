using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cshScore : MonoBehaviour
{

    public int score;
    public Text txt;

    // Start is called before the first frame update
    void Start()
    {
        txt = GameObject.Find("Score_show").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        txt.text = score.ToString();
    }
}
