using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.SceneManagement;

public class cshSurveyButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VeryGood()
    {
        cshUserInfo.Survey = 4;
    }

    public void Good() 
    {
        cshUserInfo.Survey = 3;
    }

    public void Noraml()
    {
        cshUserInfo.Survey = 2;
    }

    public void Bad()
    {
        cshUserInfo.Survey = 1;
    }

    public void VeryBad()
    {
        cshUserInfo.Survey = 0;
    }

    public void MakeUserInfo()
    {
        cshUserInfo.UserInfo user_tmp = new cshUserInfo.UserInfo
        {
            EmailInfo = cshUserInfo.Email,
            LogINTimeInfo = cshUserInfo.LogINTime,
            GameLevelInfo = cshUserInfo.GameLevel,
            GameScoreInfo = cshUserInfo.GameScore,
            BreathInfo = cshUserInfo.BreathList,
            GameTimeInfo = cshUserInfo.GameTime,
            GameStartTimeInfo = cshUserInfo.GameStartTime,
            GameOverTimeInfo = cshUserInfo.GameOverTime,
            SurveyInfo = cshUserInfo.Survey,
        };

        List<cshUserInfo.UserInfo> user_list = new List<cshUserInfo.UserInfo>();
        user_list.Add(user_tmp);

        cshUserInfo.UserPost user = new cshUserInfo.UserPost
        {
            playerInfo = user_list
        };

        string json = JsonUtility.ToJson(user);
        Debug.Log(json);

        StartCoroutine(Upload(
            "https://kv2s3duleh.execute-api.ap-northeast-2.amazonaws.com/default/lungrow-minecraft-create-data"
            , json));
        StartCoroutine(GetRequest("https://aeptktnm55.execute-api.ap-northeast-2.amazonaws.com/default/lungrow-minecraft-getInfo-ByPlayerName?playerName=LJMo_-"));

        SceneManager.LoadScene("StartScene");
    }

    IEnumerator Upload(string URL, string json)
    {
        var request = new UnityWebRequest(URL, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
        }
    }

    IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }

    IEnumerator Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
    }
}
