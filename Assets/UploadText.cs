using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UploadText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextDataUpload("Text Upload once testing !");

        Debug.Log("-----------------Send------------------");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TextDataUpload(string TextData)
    {           

        byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(TextData);
        string url = "http://140.112.42.28:12345/upload";
        string DataName = "DataName";
        Debug.Log("Run the TextDataUpload Function");

        StartCoroutine(UploadTextThread(url, bytes, DataName));//thread 

    }

    IEnumerator UploadTextThread(string url, byte[] bytes, string TextKey)
    {

        WWWForm form = new WWWForm();
        form.AddField(TextKey, bytes.ToString());

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest(); //"yeild return" is a way of "return" for thread. 給子程序用的return方法

        if (request.isNetworkError || request.isHttpError)
        {

            Debug.Log("TextUpload");
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("Get Request Completed!");
        }


        Debug.Log("------------------------------------------");
        Debug.Log(request.responseCode);
        Debug.Log(request.downloadHandler.text);//server result
        Debug.Log("------------------------------------------");
    }
}
