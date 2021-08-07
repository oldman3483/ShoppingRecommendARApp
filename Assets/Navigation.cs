using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Navigation : MonoBehaviour
{
    [SerializeField] GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        GetNaviData orient = NaviData();

        arrow.transform.eulerAngles = new Vector3(0f, 0f, orient.orient);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GetNaviData NaviData()
    {
        float orient_angle = Random.Range(-360, 360);
        return new GetNaviData
        {
            orient = orient_angle
        };
    }
}


public class GetNaviData
{
    public float orient { get; set; }
}
