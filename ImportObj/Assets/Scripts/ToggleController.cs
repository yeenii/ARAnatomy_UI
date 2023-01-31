using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    ObjFromFile off;

    public GameObject currentGameObject;
    public Transform tf;
    Toggle m_Toggle;
    

    void Start()
    {
        off = GameObject.Find("GameManager").GetComponent<ObjFromFile>(); //ObjFromFile.cs

        currentGameObject = gameObject;

        tf = GameObject.Find("toggles").transform.Find(off.ModelName + "Toggle"); //각 모델마다 토글 할당 
        m_Toggle = tf.GetComponent<Toggle>(); //Fetch the Toggle GameObject


        //Add listener for when the state of the Toggle changes, to take action
        m_Toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(m_Toggle);
        });
    }

    //Output the new state of the Toggle into Text
    void ToggleValueChanged(Toggle change)
    {
        currentGameObject.SetActive(m_Toggle.isOn);
    }
}
