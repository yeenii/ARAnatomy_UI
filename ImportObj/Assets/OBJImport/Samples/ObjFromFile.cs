using Dummiesman;
using System.IO;
using UnityEngine;

public class ObjFromFile : MonoBehaviour
{
    TransformData td;
   
    string objPath = string.Empty;
    //string[] objPath = new string[50]; //파일 경로
    string error = string.Empty;
    static int q = 0; //loadedObject[q]
    public GameObject[] loadedObject = new GameObject[100]; //인체모델 100개
    double [,] tScale = new double[100, 3]; //100개 비율 데이터 저장 
   

    public void OnGUI() //파일 임포트 UI
    {
        td = GameObject.Find("TransformManager").GetComponent<TransformData>();
            
        objPath = GUI.TextField(new Rect(0, 0, 256, 32), objPath);
        GUI.Label(new Rect(0, 0, 256, 32), "Obj Path:");

        if(GUI.Button(new Rect(256, 32, 64, 32), "Load File"))
        {
            //file path
            if (!File.Exists(objPath)) //파일이 존재하지 않으면
            {
                error = "File doesn't exist.";
            }
            else { //파일이 존재하면 
                      
                    loadedObject[q] = new OBJLoader().Load(objPath);
                    error = string.Empty;
                    //DontDestroyOnLoad(loadedObject[i]);

                    //인체모델 위치, 각도, 크기 
                    MakeRate(); //비율 구하는 메서드         
                    loadedObject[q].transform.rotation = Quaternion.Euler(new Vector3(td.Rotation[q, 0], td.Rotation[q, 1], td.Rotation[q, 2])); //rotation
                    loadedObject[q].transform.localScale = new Vector3((float)tScale[q, 0], (float)tScale[q, 2], (float)tScale[q, 1]); //scale
                    loadedObject[q].transform.position = new Vector3((float)(td.Position[q, 0] * tScale[q, 0]), (float)(td.Position[q, 1] * tScale[q, 1]), (float)(td.Position[q, 2] * tScale[q, 2])); //position
                  
                //다음 모델 추가 
                if (loadedObject.Length > q)
                    q++;




            }

        if(!string.IsNullOrWhiteSpace(error))
        {
            GUI.color = Color.red;
            GUI.Box(new Rect(0, 64, 256 + 64, 32), error);
            GUI.color = Color.white;
        }     
                
        }
    } //OnGUI


  
    public void MakeRate() //비율 구하는 메서드
    {

        int i;
        double[,] orgNum = new double[100, 3]; //100개의 모델 데이터
        double[,] mNum = new double[100, 3];  //100개의 모델 데이터


        for (i = 0; i < 3; i++)
            {
                //값이 음수인 경우 -> 양수로 변환 
                if (td.mScale[q, i] < 0)
                    td.mScale[q, i] = -td.mScale[q, i];

                if (td.orgScale[q, i] < 0)
                    td.orgScale[q, i] = -td.orgScale[q, i];

                //cnt : 몇번 나누어지는지 구하기 위해 
                int j;
                int cnt = 1;

                if (td.orgScale[q, i] >= td.mScale[q, i]) //num1이 num2보다 큰 경우 
                {
                    for (j = 1; j <= td.orgScale[q, i]; j++)
                    {
                        if (td.orgScale[q, i] % j == 0 && td.mScale[q, i] % j == 0)
                            cnt++;

                    }
                }
                else
                {
                    for (j = 1; j <= td.mScale[q, i]; j++)
                    {
                        if (td.orgScale[q, i] % j == 0 && td.mScale[q, i] % j == 0)
                            cnt++;

                    }
                }

                //비례식 구하기 
                if (cnt == 1)
                {
                    orgNum[q, i] = td.orgScale[q, i];
                    mNum[q, i] = td.mScale[q, i];
                }
                else
                {
                    for (j = 1; j <= td.orgScale[q, i]; j++)
                    {
                        if (td.orgScale[q, i] % j == 0 && td.mScale[q, i] % j == 0)
                        {
                            orgNum[q, i] = td.orgScale[q, i] / j;
                            mNum[q, i] = td.mScale[q, i] / j;
                        }

                    }
                }

                //비율 구하기  
                tScale[q, i] = mNum[q, i] * 1 / orgNum[q, i];

            } //for문 i end
       


    }


}
