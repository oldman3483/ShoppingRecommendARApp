using System;
using System.IO;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class GetServerData2 : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        string url = "http://140.112.42.28:12345/output";
        //string url_wrong = "https://error.html";
        StartCoroutine(GetData(url));

    }


    // get server thread
    IEnumerator GetData(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        Debug.Log(request.error);
        Debug.Log(request.responseCode);
        Debug.Log(request.downloadHandler.text);

        if(request.responseCode != 0)
        {
            Debug.Log("Succeed to connect server!");

            string myString = request.downloadHandler.text;

            int StartIndex = myString.IndexOf("<p class=\"box - text\" id=\"imgbox1\">");
            int EndIndex = myString.IndexOf("</p>");

            //Debug.Log(myString.Substring(StartIndex + 3, EndIndex - StartIndex - 3));

            Debug.Log(myString);
            
            
        }

        
    }

}



