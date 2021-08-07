using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSize : MonoBehaviour
{
    [SerializeField] GameObject m_Object;
    private Vector3 ScaleChange =  new Vector3(0.01f, 0.01f, 0.01f);
    private Vector3 leftward = new Vector3(0.1f, 0f, 0f);



    public void Downsize()
    { 
        m_Object.transform.localScale -= ScaleChange;
    }

    public void Upsize()
    { 
        m_Object.transform.localScale += ScaleChange;
    }

    public void Leftward()
    {
        m_Object.transform.localPosition += leftward;
    }
    

    public void Rightward()
    {

    }
}
