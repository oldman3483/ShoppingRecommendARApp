using System;
using System.IO;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class HighlightBox : MonoBehaviour
{

    
	[SerializeField] Image m_LineUp;
	[SerializeField] Image m_LineDown;
	[SerializeField] Image m_LineLeft;
	[SerializeField] Image m_LineRight;
    [SerializeField] GameObject[] m_TextContent = new GameObject[2];
    [SerializeField] GameObject[] m_BtnContent = new GameObject[2];
    [SerializeField] Image[] m_image = new Image[2];


    string[] jpg_url = new string[2];

    float L1w = 0;
    float L2w = 0;
    float L3w = 0;
    float L4w = 0;

    public string[] SrcValue = new string[0];
    public string[] SrcidValue = new string[0];
    public string[] Pid = new string[0];
    public string[] PValue = new string[0];
    string[,] PValue_2D = new string[20, 20];
    string[,] SrcValue_2D = new string[20, 20];



    // Start is called before the first frame update
    void Start()
    {
        

        string url = "http://140.112.42.28:12345/output";
        //string url_wrong = "https://error.html";
        StartCoroutine(GetServerData(url));


        //Debug.Log(PValue.Length);
        //Debug.Log("----*-**-*-*-*--" + PValue[1]+"++++++++++++++++++++++");
    }
    private void Update()
    {

        string url = "http://140.112.42.28:12345/output";
        StartCoroutine(GetServerData(url));
    }

    private void LateUpdate()
    {
        BoxPoint point = TestBoxPos();

        L1w = Mathf.Sqrt(Mathf.Pow(point._x2 - point._x1, 2) + Mathf.Pow(point._y2 - point._y1, 2));
        L2w = Mathf.Sqrt(Mathf.Pow(point._x3 - point._x2, 2) + Mathf.Pow(point._y3 - point._y2, 2));
        L3w = Mathf.Sqrt(Mathf.Pow(point._x4 - point._x3, 2) + Mathf.Pow(point._y4 - point._y3, 2));
        L4w = Mathf.Sqrt(Mathf.Pow(point._x1 - point._x4, 2) + Mathf.Pow(point._y1 - point._y4, 2));

        m_LineUp.rectTransform.sizeDelta = new Vector2(L1w, 3);
        m_LineDown.rectTransform.sizeDelta = new Vector2(L2w, 3);
        m_LineLeft.rectTransform.sizeDelta = new Vector2(L3w, 3);
        m_LineRight.rectTransform.sizeDelta = new Vector2(L4w, 3);

        float x1x2 = (point._x1 + point._x2) / 2;
        float y1y2 = (point._y1 + point._y2) / 2;
        float x2x3 = (point._x2 + point._x3) / 2;
        float y2y3 = (point._y2 + point._y3) / 2;
        float x3x4 = (point._x3 + point._x4) / 2;
        float y3y4 = (point._y3 + point._y4) / 2;
        float x4x1 = (point._x4 + point._x1) / 2;
        float y4y1 = (point._y4 + point._y1) / 2;

        m_LineUp.transform.localPosition = new Vector3(x1x2, y1y2, 0f);
        m_LineDown.transform.localPosition = new Vector3(x2x3, y2y3, 0f);
        m_LineLeft.transform.localPosition = new Vector3(x3x4, y3y4, 0f);
        m_LineRight.transform.localPosition = new Vector3(x4x1, y4y1, 0f);

        float L1Angle, L2Angle, L3Angle, L4Angle;

        if (PValue_2D[0,0] == null)
        {
            L1Angle = 0;
            L2Angle = 0;
            L3Angle = 0;
            L4Angle = 0;
        }
        else
        {
            L1Angle = Mathf.Atan((point._y2 - point._y1) / (point._x2 - point._x1)) * Mathf.Rad2Deg;
            L2Angle = Mathf.Atan((point._y3 - point._y2) / (point._x3 - point._x2)) * Mathf.Rad2Deg;
            L3Angle = Mathf.Atan((point._y4 - point._y3) / (point._x4 - point._x3)) * Mathf.Rad2Deg;
            L4Angle = Mathf.Atan((point._y1 - point._y4) / (point._x1 - point._x4)) * Mathf.Rad2Deg;
        }

 
        m_LineUp.transform.eulerAngles = new Vector3(0f, 0f, L1Angle);
        m_LineDown.transform.eulerAngles = new Vector3(0f, 0f, L2Angle);
        m_LineLeft.transform.eulerAngles = new Vector3(0f, 0f, L3Angle);
        m_LineRight.transform.eulerAngles = new Vector3(0f, 0f, L4Angle);



        //Debug.Log("Start ->>> " + PValue_2D[2, 2]);

    }
    // Update is called once per frame
    /*
        void Update()
        {
            BoxPoint point = TestBoxPos();
            width = Mathf.Abs(point.y1 - point.x1);
            height = Mathf.Abs(point.y2 - point.x2);

            m_LineUp.rectTransform.sizeDelta = new Vector2(width, 5);
            m_LineDown.rectTransform.sizeDelta = new Vector2(width, 5);
            m_LineLeft.rectTransform.sizeDelta = new Vector2(5, height);
            m_LineRight.rectTransform.sizeDelta = new Vector2(5, height);



        }
    */


    // 抓Server資料子程序
    IEnumerator GetServerData(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        Debug.Log(request.error);
        Debug.Log(request.responseCode);

        if (request.responseCode != 0)
        {
            Debug.Log("Succeed to connect server!");

            string myString = request.downloadHandler.text;
            Debug.Log(myString);

            int StartIndex = myString.IndexOf("<p class=\"box - text\" id=\"imgbox1\">");
            int EndIndex = myString.IndexOf("</p>");

            //Debug.Log(myString.Substring(StartIndex + 3, EndIndex - StartIndex - 3));

            //Debug.Log(myString);

            string[] array = myString.Split('\n');//

            var temp1 = new List<string>(SrcValue);
            var temp2 = new List<string>(SrcidValue);
            var temp3 = new List<string>(Pid);
            var temp4 = new List<string>(PValue);

            temp1.Clear();
            temp2.Clear();
            temp3.Clear();
            temp4.Clear();
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
                    string str1 = s.Substring(first + 4, last - first - 4);
                    string str2 = s.Substring(last + 2, final - last - 2);

                    //Debug.Log("Pid: " + str1 + "--> PValue" + str2);

                    temp3.Add(str1);
                    temp4.Add(str2);

                }
            }
            PValue = new string[0];
            SrcValue = temp1.ToArray(); //src¤§­È
            SrcidValue = temp2.ToArray(); //src¤§id­È
            Pid = temp3.ToArray(); //p¤§id­È
            PValue = temp4.ToArray(); //p¤§value­È



            int i = 0;
            foreach (var sub in PValue)
            {
                if (i < 20)
                {
                    //Debug.Log(Pid[i]);
                    string[] PValueArray = sub.Split(',');
                    //Debug.Log("Sub: " + sub);
                    for (int j = 0; j < PValueArray.Length; j++)
                    {
                        //Debug.Log("i, j is ==" + i + ", " + j);
                        //Debug.Log(PValueArray[j]);
                        PValue_2D[i, j] = PValueArray[j];
                        //Debug.Log("i= "+i.ToString()+", j= "+j.ToString()+"  "+PValue_2D[i, j].ToString());
                    }
                }
                else break;

                i++;
            }


            i = 0;
            foreach (var sub in SrcidValue)
            {
                if (i < SrcValue.Length)
                {
                    Debug.Log(SrcValue[i]);
                    string[] SrcidValueArray = sub.Split(',');
                    //Debug.Log("Sub: " + sub);
                    for (int j = 0; j < SrcidValueArray.Length; j++)
                    {
                        //Debug.Log("i, j is ==" + i + ", " + j);
                        //Debug.Log(PValueArray[j]);
                        SrcValue_2D[i, j] = SrcidValueArray[j];
                        Debug.Log("i= " + i.ToString() + ", j= " + j.ToString() + "  " + SrcValue_2D[i, j].ToString());
                    }
                }
                else break;

                i++;
            }

            i = 0;
            foreach (var subid in Pid)
            {
                if (subid == "txtcontent0")
                {
                    //Debug.Log("------------- txtcontent0 ----------");
                    //Debug.Log(PValue_2D[i, 0]);
                    m_TextContent[0].GetComponent<Text>().text = "--------->> " + PValue_2D[i, 0].ToString() + "<<------------";
                }
                else if (subid == "txtcontent1")
                {
                    //Debug.Log("------------- txtcontent1 ----------");
                    //Debug.Log(PValue_2D[i, 0]);
                    m_TextContent[0].GetComponent<Text>().text = "--------->> " + PValue_2D[i, 0].ToString() + "<<------------";
                }
                else if (subid == "btncontent0")
                {
                    //Debug.Log("------------- btncontent0 ----------");
                    //Debug.Log(PValue_2D[i, 0]);
                    m_BtnContent[0].GetComponent<Text>().text = PValue_2D[i, 0].ToString();

                }
                else if (subid == "btncontent1")
                {
                    //Debug.Log("------------- btncontent1 ----------");
                    //Debug.Log(PValue_2D[i, 0]);
                    m_BtnContent[1].GetComponent<Text>().text = PValue_2D[i, 0].ToString();
                }

                i++;
            }

            int src_i = 0;
            for (src_i = 0; src_i < SrcValue.Length; src_i++)
            {
                Debug.Log(SrcValue_2D[src_i, 0]);
                
                if(SrcValue_2D[src_i, 0] == "pic0")
                {
                    jpg_url[0] = "http://140.112.42.28:12345"+SrcValue[src_i].ToString();
                    Debug.Log(jpg_url[0]);
                    StartCoroutine(GetSeverImg(jpg_url[0], 0));
                }
                else if(SrcValue_2D[src_i, 0] == "pic1")
                {
                    jpg_url[1] = "http://140.112.42.28:12345" + SrcValue[src_i].ToString();
                    Debug.Log(jpg_url[1]);
                    StartCoroutine(GetSeverImg(jpg_url[1], 1));
                }
            }


            //Debug.Log(PValue_2D[1, 9]);
            //Debug.Log(PValue.Length);
            //Debug.Log("----*-**-*-*-*--" + PValue[1]);
        }
    }


    IEnumerator GetSeverImg(string url, int ImgID)
    {
        var request = UnityWebRequestTexture.GetTexture(url);

        yield return request.SendWebRequest();

        if (!request.isNetworkError && !request.isHttpError)
        {
            //var tex = (request.downloadHandler as DownloadHandlerTexture).texture;
            var tex = DownloadHandlerTexture.GetContent(request);
            var rect = new Rect(0, 0, tex.width, tex.height);
            var pivot = new Vector2(0.5f, 0.5f);
            var sprite = Sprite.Create(tex, rect, pivot);

            // TODO: Sprite Rendering
            m_image[ImgID].sprite = sprite;
        }
        /*
        Debug.Log("******************************************");
        Debug.Log(request.error);
        Debug.Log(request.responseCode);
        Debug.Log(request.downloadHandler.text);//輸出伺服器返回結果。
        Debug.Log("******************************************");
        */
    }

    public BoxPoint TestBoxPos()
    {
        /*
        float x1 = UnityEngine.Random.Range(-190, 190);
        float y1 = UnityEngine.Random.Range(-190, 190);
        float x2 = UnityEngine.Random.Range(-190, 190);
        float y2 = UnityEngine.Random.Range(-190, 190);
        float x3 = UnityEngine.Random.Range(-190, 190);
        float y3 = UnityEngine.Random.Range(-190, 190);
        float x4 = UnityEngine.Random.Range(-190, 190);
        float y4 = UnityEngine.Random.Range(-190, 190);
        */

        float x1, y1, x2, y2, x3, y3, x4, y4;
        if (PValue_2D[0, 1] == null)
        {
            x1 = 0;
            y1 = 0;
            x2 = 0;
            y2 = 0;
            x3 = 0;
            y3 = 0;
            x4 = 0;
            y4 = 0;
        }
        else
        {
            x1 = float.Parse(PValue_2D[0, 0])*100;
            y1 = float.Parse(PValue_2D[0, 1])*100;
            x2 = float.Parse(PValue_2D[0, 2])*100;
            y2 = float.Parse(PValue_2D[0, 3])*100;
            x3 = float.Parse(PValue_2D[0, 4])*100;
            y3 = float.Parse(PValue_2D[0, 5])*100;
            x4 = float.Parse(PValue_2D[0, 6])*100;
            y4 = float.Parse(PValue_2D[0, 7])*100;
        }
        /*
        Debug.Log(x1);
        Debug.Log(x2);
        Debug.Log(x3);
        Debug.Log(x4);
        Debug.Log(y1);
        Debug.Log(y2);
        Debug.Log(y3);
        Debug.Log(y4);
        */
        return new BoxPoint
        {
            _x1 = x1,
            _y1 = y1,
            _x2 = x2,
            _y2 = y2,
            _x3 = x3,
            _y3 = y3,
            _x4 = x4,
            _y4 = y4

        };
    }


}

public class BoxPoint
{
    public float _x1 { get; set; }
    public float _y1 { get; set; }
    public float _x2 { get; set; }
    public float _y2 { get; set; }
    public float _x3 { get; set; }
    public float _y3 { get; set; }
    public float _x4 { get; set; }
    public float _y4 { get; set; }
}

