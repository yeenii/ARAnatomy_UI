using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformData : MonoBehaviour
{
    //scale
    public float[,] orgScale = { { 968F, 3400F, 516F }, { 604F, 648F, 378F }, { 968F, 3400F, 516F } }; //org model scale
    public float[,] mScale = { { 484.65F, 1700.9F, 258.33F }, { 302.15f, 324.03F, 189.75F }, { 484.65F, 1700.9F, 258.33F } }; //unity model scale 


    //OrgPosition
    public float[,] Position = { { -242.325F, 850.45F, 129.165F }, { -233F, 1215F, 129F }, { -242.325F, 850.45F, 129.165F } };

    //rotation
    public float[,] Rotation = { { 90, 180, 0 }, { 90, 0, 0 }, { 90, 180, 0 } };


}
