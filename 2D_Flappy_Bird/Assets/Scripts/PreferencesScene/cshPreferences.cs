using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class cshPreferences : MonoBehaviour
{
    public InputField BS;

    public InputField[] E = new InputField[6];
    public InputField[] N = new InputField[6];
    public InputField[] H = new InputField[6];

    public InputField ET;
    public InputField NT;
    public InputField HT;

    public InputField ES;
    public InputField NS;
    public InputField HS;

    // Start is called before the first frame update
    void Start()
    {
        BS.text = cshValueSetting.speed.ToString();

        for (int i = 0; i < 6; i++)
        {
            E[i].text = cshValueSetting.EASY_spawnPosition[i].ToString();
            N[i].text = cshValueSetting.NORMAL_spawnPosition[i].ToString();
            H[i].text = cshValueSetting.HARD_spawnPosition[i].ToString();
        }

        ET.text = cshValueSetting.EASYmaxTime.ToString();
        NT.text = cshValueSetting.NORMALmaxTime.ToString();
        HT.text = cshValueSetting.HARDmaxTime.ToString();

        ES.text = cshValueSetting.EASYspeed.ToString();
        NS.text = cshValueSetting.NORMALspeed.ToString();
        HS.text = cshValueSetting.HARDspeed.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PositionSettings() 
    {
        cshValueSetting.speed = float.Parse(BS.text);

        for (int i = 0; i < 6; i++)
        {
            cshValueSetting.EASY_spawnPosition[i] = float.Parse(E[i].text);
            cshValueSetting.NORMAL_spawnPosition[i] = float.Parse(N[i].text);
            cshValueSetting.HARD_spawnPosition[i] = float.Parse(H[i].text);
        }

        cshValueSetting.EASYmaxTime = float.Parse(ET.text);
        cshValueSetting.NORMALmaxTime = float.Parse(NT.text);
        cshValueSetting.HARDmaxTime = float.Parse(HT.text);

        cshValueSetting.EASYspeed = float.Parse(ES.text);
        cshValueSetting.NORMALspeed = float.Parse(NS.text);
        cshValueSetting.HARDspeed = float.Parse(HS.text);
    }

    public void Back()
    {
        SceneManager.LoadScene("StartScene");
    }
}
