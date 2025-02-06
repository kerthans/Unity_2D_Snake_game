using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    bool isshow=false;
    public RectTransform obj;
    public Transform parentObj;
    // Start is called before the first frame update
    void Start()
    {
        RectTransform initObj = Instantiate(obj, parentObj);
        //initObj.anchoredPosition = Vector3.zero;
        Debug.Log(initObj.anchoredPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isshow)
        {
            foreach (RectTransform item in parentObj)
            {
                Debug.Log(item.anchoredPosition);
            }
            isshow = true;
        }   
    }
}
