using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OpacityController : MonoBehaviour
{
    ObjFromFile off;
    public GameObject currentGameObject;
    public float alpha = 0.2f;
    //Get current Material
    private Material currentMat;
    public Slider currentSlider;
    public Transform tf;

    void Start()
    {
        off = GameObject.Find("GameManager").GetComponent<ObjFromFile>(); //ObjFromFile.cs

        currentGameObject = gameObject; 
        currentMat = currentGameObject.GetComponent<Renderer>().material;

        tf = GameObject.Find("sliders").transform.Find(off.ModelName + "Slider"); //각 모델마다 슬라이더 할당 
        currentSlider = tf.GetComponent<Slider>(); 

        currentSlider.onValueChanged.AddListener(delegate { ChangeAlphaOnValueChange(currentSlider); }); 
    }


    void ChangeAlpha(Material mat, float alphaVal)
    {
        Color oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaVal);
        mat.SetColor("_Color", newColor);
    }

    public void ChangeAlphaOnValueChange(Slider slider)
    {
        ChangeAlpha(currentMat, slider.value);
    }
}
