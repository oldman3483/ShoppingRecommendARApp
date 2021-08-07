using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
//using Imgur.API.Endpoints.Impl;
//using Imgur.API.Authentication.Impl;
using System.Threading.Tasks;




public class CaptureUpload : MonoBehaviour
{

    public Camera cutFrameCamer;
    Rect canvas;
    // Start is called before the first frame update
    void Start()
    {
        //設定畫布大小等於當前螢幕的寬和高。
        //CaptureScreen(cutFrameCamer, canvas); //執行截圖方法。

    }

    // Update is called once per frame
    void Update()
    {
        canvas.Set(0, 0, Screen.width, Screen.height);
        CaptureScreen(cutFrameCamer, canvas); //執行截圖方法。
        Debug.Log("-----------------Send------------------");
    }

    public void CaptureScreen(Camera c, Rect r)
    {
        //screenshot -> array
        RenderTexture rt = new RenderTexture((int)r.width, (int)r.height, 0);
        c.targetTexture = rt;
        c.Render();

        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)r.width, (int)r.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(r, 0, 0);
        screenShot.Apply();

        c.targetTexture = null;
        RenderTexture.active = null;
        GameObject.Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        string url = "http://140.112.42.28:12345/upload";
        string fileName = "Uploadfile1";
        //string imgurPath = UploadImgurImageByBytesAsync(bytes).GetAwaiter().GetResult();

        StartCoroutine(UploadImg(url, bytes, fileName));//thread 
        Destroy(screenShot);
    }

    IEnumerator UploadImg(string url, byte[] bytes, string fileName)
    {
        
        WWWForm form = new WWWForm();
        form.AddField("filename", fileName);
        form.AddBinaryData("data", bytes, "upload.png", "image/png");//upload img stream
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest(); //"yeild return" is a way of "return" for thread. 給子程序用的return方法

        if (request.isNetworkError || request.isHttpError)
        {

            Debug.Log("CaptureUpload");
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("Get Request Completed!");
        }


        Debug.Log("------------------------------------------");
        Debug.Log(request.error);
        Debug.Log(request.responseCode);
        Debug.Log(request.downloadHandler.text);//server result
        Debug.Log("------------------------------------------");
    }

}

