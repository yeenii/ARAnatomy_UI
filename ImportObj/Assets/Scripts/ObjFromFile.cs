using Dummiesman;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ObjFromFile : MonoBehaviour
{
    TransformData td; //TransformData.cs
   
    public string objPath = string.Empty;
    //string[] objPath = new string[50]; //파일 경로
    string error = string.Empty;
    public string ModelName = string.Empty;

    public static int m = 0; //model 번호 
    public GameObject[] loadedObject = new GameObject[100]; //인체모델 100개
    public static int q = 0; //loadedObject[q]
    double [,] tScale = new double[100, 3]; //100개 비율 데이터 저장 

    public Material mat;     // Materials 

    //Opacity
    public Material currentMat;
    public float alpha = 0.2f;
    

    public void OnGUI() //파일 임포트 UI
    {
        td = GameObject.Find("GameManager").GetComponent<TransformData>();
            
        objPath = GUI.TextField(new Rect(0, 0, 256, 32), objPath);
        GUI.Label(new Rect(0, 0, 256, 32), "Obj Path:");

        if(GUI.Button(new Rect(256, 32, 64, 32), "Load File"))
        {
            //file path
            if (!File.Exists(objPath)) //파일이 존재하지 않으면
            {
                error = "File doesn't exist.";
            }
            else
            { //파일이 존재하면 
                      
                loadedObject[q] = new OBJLoader().Load(objPath);
                error = string.Empty;
               ModelName =  loadedObject[q].name;
                //DontDestroyOnLoad(loadedObject[i]);


                SearchModel(); //모델 검색 
                ModelVal(); //모델 위치, 각도, 크기 함수

                //material 적용
                mat = Resources.Load<Material>("Materials/" + ModelName);
                loadedObject[q].transform.GetChild(0).GetComponent<MeshRenderer>().material = mat;

                //opacity
                loadedObject[q].transform.GetChild(0).gameObject.AddComponent<OpacityController>(); //모델 컴포넌트에 스크립트 추가 

                //toggle
                loadedObject[q].transform.GetChild(0).gameObject.AddComponent<ToggleController>(); //모델 컴포넌트에 스크립트 추가 


                //다음 모델 추가 
                if (loadedObject.Length > q)
                    q++;

            }

        if (!string.IsNullOrWhiteSpace(error))
        {
            GUI.color = Color.red;
            GUI.Box(new Rect(0, 64, 256 + 64, 32), error);
            GUI.color = Color.white;
        }

       
        }
    } //OnGUI


    public void SearchModel() //모델 검색 
    {
        //모델 검색 
        if (ModelName == "Skin" && objPath.Contains("Skin.obj") == true)
            m = 0;


        if (ModelName == "Heart" && objPath.Contains("Heart.obj") == true)
            m = 1;
    }

    public void ModelVal() //인체모델 위치, 각도, 크기
    {
        
        MakeRate(); //비율 구하는 메서드         
        loadedObject[q].transform.rotation = Quaternion.Euler(new Vector3(td.Rotation[m, 0], td.Rotation[m, 1], td.Rotation[m, 2])); //rotation
        loadedObject[q].transform.localScale = new Vector3((float)tScale[m, 0], (float)tScale[m, 2], (float)tScale[m, 1]); //scale
        loadedObject[q].transform.position = new Vector3((float)(td.Position[m, 0] * tScale[m, 0]), (float)(td.Position[m, 1] * tScale[m, 1]), (float)(td.Position[m, 2] * tScale[m, 2])); //position
    }



    public void MakeRate() //비율 구하는 메서드
    {

        int i;
        double[,] orgNum = new double[100, 3]; //100개의 모델 데이터
        double[,] mNum = new double[100, 3];  //100개의 모델 데이터


        for (i = 0; i < 3; i++)
        {
            //값이 음수인 경우 -> 양수로 변환 
            if (td.mScale[m, i] < 0)
                td.mScale[m, i] = -td.mScale[m, i];

            if (td.orgScale[m, i] < 0)
                td.orgScale[m, i] = -td.orgScale[m, i];

            //cnt : 몇번 나누어지는지 구하기 위해 
            int j;
            int cnt = 1;

            if (td.orgScale[m, i] >= td.mScale[m, i]) //num1이 num2보다 큰 경우 
            {
                for (j = 1; j <= td.orgScale[m, i]; j++)
                {
                    if (td.orgScale[m, i] % j == 0 && td.mScale[m, i] % j == 0)
                        cnt++;

                }
            }
            else
            {
                for (j = 1; j <= td.mScale[m, i]; j++)
                {
                    if (td.orgScale[m, i] % j == 0 && td.mScale[m, i] % j == 0)
                        cnt++;

                }
            }

            //비례식 구하기 
            if (cnt == 1)
            {
                orgNum[m, i] = td.orgScale[m, i];
                mNum[m, i] = td.mScale[m, i];
            }
            else
            {
                for (j = 1; j <= td.orgScale[m, i]; j++)
                {
                    if (td.orgScale[m, i] % j == 0 && td.mScale[m, i] % j == 0)
                    {
                        orgNum[m, i] = td.orgScale[m, i] / j;
                        mNum[m, i] = td.mScale[m, i] / j;
                    }

                }
            }

            //비율 구하기  
            tScale[m, i] = mNum[m, i] * 1 / orgNum[m, i];

        } //for문 i end



    }

}
