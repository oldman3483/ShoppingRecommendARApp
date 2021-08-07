using System;
using System.IO;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GetServerData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start!");
        string url = "http://140.112.42.28:12345/output";
        //string url_wrong = "https://error.html";
        Debug.Log("Start!");
        StartCoroutine(ProcessData(url));
    }

    public string[] SrcValue = new string[0];
    public string[] SrcidValue = new string[0];
    public string[] Pid = new string[0];
    public string[] PValue = new string[0];

    // get server thread
    IEnumerator ProcessData(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        Debug.Log(request.error);
        Debug.Log(request.responseCode);
        Debug.Log(request.downloadHandler.text);

        if (request.responseCode != 0)
        {
            Debug.Log("Succeed to connect server!");

            string myString = request.downloadHandler.text;

            int StartIndex = myString.IndexOf("<p class=\"box - text\" id=\"imgbox1\">");
            int EndIndex = myString.IndexOf("</p>");

            //Debug.Log(myString.Substring(StartIndex + 3, EndIndex - StartIndex - 3));

            Debug.Log(myString);
            string[] array = myString.Split('\n');//

            var temp1 = new List<string>(SrcValue);
            var temp2 = new List<string>(SrcidValue);
            var temp3 = new List<string>(Pid);
            var temp4 = new List<string>(PValue);

            foreach (string s in array)
            {
                if (s.Contains("src"))
                {
                    int first = s.IndexOf("src=");
                    int last = s.LastIndexOf("id=");
                    int final = s.IndexOf(">");
                    string str1 = s.Substring(first + 5, last - first - 7);
                    string str2 = s.Substring(last + 4, final - last - 6);

                    temp1.Add(str1);
                    temp2.Add(str2);
                }
                else if (s.Contains("<p class"))
                {
                    int first = s.LastIndexOf("id=");
                    int last = s.IndexOf("\">");
                    int final = s.IndexOf("</p>");
                    if (final == -1)
                    {
                        final = s.IndexOf("<p/>");
                    }
                    string str1 = s.Substring(first + 1, last - first);
                    string str2 = s.Substring(last + 2, final - last - 2);

                    
                    Debug.Log("Pid: "+str1+"--> PValue"+str2);

                    temp3.Add(str1);
                    temp4.Add(str2);
                }
            }
            SrcValue = temp1.ToArray(); //src¤§­È
            SrcidValue = temp2.ToArray(); //src¤§id­È
            Pid = temp3.ToArray(); //p¤§id­È
            PValue = temp4.ToArray(); //p¤§value­È

            
            
            Debug.Log(SrcValue.Length);
            Debug.Log("----*-**-*-*-*--" + SrcValue[0]);
        }
    }
}


