using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class GetImg : MonoBehaviour
{

    [SerializeField] Image m_image;
    string jpg_url;
    // Start is called before the first frame update
    void Start()
    {
        jpg_url = "http://140.112.42.28:12345/static/img/1.png";
        StartCoroutine(GetSeverImg(jpg_url));
    }

    // Update is called once per frame
    void Update()
    {
        jpg_url = "http://140.112.42.28:12345/static/img/1.png";
        StartCoroutine(GetSeverImg(jpg_url));
    }


    IEnumerator GetSeverImg(string url)
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
            m_image.sprite = sprite;
        }
        Debug.Log("******************************************");
        Debug.Log(request.error);
        Debug.Log(request.responseCode);
        Debug.Log(request.downloadHandler.text);//輸出伺服器返回結果。
        Debug.Log("******************************************");
    }
}
