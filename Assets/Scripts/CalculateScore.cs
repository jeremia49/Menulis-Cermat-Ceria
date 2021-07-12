using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class CalculateScore : MonoBehaviour
{
    public string LEVEL;


    public Color color;
    public GameObject HintParent;
    public GameObject TargetParent;
    public List<GameObject> createdLines;
    public int lineIndex = 0;
    public Material material;
    public int pointIndex = 0;
    public int stationCount = 0;
    

    public TextMeshProUGUI LineLeftText;

    public float Score = 0;

    public int LineLimit;
    public int LineLeft;

    private void Awake()
    {

    }

    void Start()
    {
        PlayerPrefs.SetString("level", LEVEL);
        createdLines = new List<GameObject>();
        LineLeft = LineLimit;
        LineLeftText.text = "Percobaan Tersisa : " + LineLeft;
    }

    void Update()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 worldpoint =  Camera.main.ScreenToWorldPoint(touch.position);
            Vector3 touchpos = new Vector3(worldpoint.x, worldpoint.y, 1f);
            

            if (touch.phase == TouchPhase.Began)
            {
               GameObject lineObject = new GameObject();
                lineObject.GetComponent<Transform>().SetParent(TargetParent.GetComponent<Transform>());
                lineObject.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, 1f);
                lineObject.GetComponent<Transform>().localScale = new Vector3(100f, 100f, 1f);
                lineObject.AddComponent<LineRenderer>();

                LineRenderer line = lineObject.GetComponent<LineRenderer>();
                line.material = material;
                line.startWidth = 0.01f;
                line.endWidth = 0.01f;

                line.SetPosition(0, touchpos);
                line.SetPosition(1, touchpos);

                createdLines.Add(lineObject);

                LineLeft--;

            }else if(touch.phase == TouchPhase.Moved)
            {
                LineRenderer targetLine = createdLines[lineIndex].GetComponent<LineRenderer>();
                pointIndex++;
                targetLine.positionCount++;
                targetLine.SetPosition(pointIndex, touchpos);
                targetLine.SetPosition(pointIndex+1, touchpos);
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                
                LineRenderer targetLine = createdLines[lineIndex].GetComponent<LineRenderer>();

              
                MeshCollider meshCollider = createdLines[lineIndex].AddComponent<MeshCollider>();

                Mesh mesh = new Mesh();
                targetLine.BakeMesh(mesh, false);
                meshCollider.sharedMesh = mesh;

                pointIndex = 0;
                lineIndex++;

                if(LineLeft <= 0)
                {
                    SceneManager.LoadSceneAsync("Result");
                }

                
            }


        }
        
        updatePoints();
        LineLeftText.text = "Percobaan Tersisa : " + LineLeft;
    }

    float proc(float a,float b){
        float c=0;
        if (a == b){
            c=0f;
        }else if(a < b){
            c = 0.001f;
        }else{
            c = -0.001f;
        }
        return c;
    } 

    bool check(float a, float b){
        if(a > b + 0.001f){
            return true;
        }else if(a < b - 0.001f){
            return true;
        }else{
            return false;
        }
    }



    private void updatePoints()
    {
        var Benar = 0;
        foreach (Transform childTransforms in HintParent.GetComponent<Transform>())
        {
            // int total = childTransforms.GetComponent<LineRenderer>().positionCount;
            int correct = 0;

            float x1 = childTransforms.GetComponent<LineRenderer>().GetPosition(0).x;
            float y1 = childTransforms.GetComponent<LineRenderer>().GetPosition(0).y;
            float x2 = childTransforms.GetComponent<LineRenderer>().GetPosition(1).x;
            float y2 = childTransforms.GetComponent<LineRenderer>().GetPosition(1).y;

            float koefx = y1-y2;
            float koefy = x2-x1;
            float koef = (x1*y2) - (x2*y1);

            float subx(float x){
                return (((-1*koef)-(koefx * x))/ (koefy));
            }
            // float suby(float y){
            //     return (((-1*koef)-(koefy * y))/ (koefx));
            // }

            float x=x1;
            float y=y1;

            List<float> xarr =  new List<float>();
            List<float> yarr =  new List<float>();

            int n = 0;
            while (true){
                if ( (float)Math.Round(x1, 2)  == (float)Math.Round(x2, 2)){
                    y += proc(y1,y2);
                    xarr.Add(x);
                    yarr.Add(y);
                    if(!check(y,y2)){
                        break;
                    }    
                }else if((float)Math.Round(y1, 2)==(float)Math.Round(y2, 2)){
                    x += proc(x1,x2);
                    xarr.Add(x);
                    yarr.Add(y);
                    if(!check(x,x2)){
                        break;
                    }    
                }else{
                    x += proc(x1,x2);
                    
                    xarr.Add(x);
                    yarr.Add(subx(x));
                    
                    if(!check(x,x2)){
                        break;
                    }    
                }
                n++;        
            }
            
            int total = xarr.Count;
                                    
            for (int i = 0; i < total ; i++)
            {
                // Vector3 pos = childTransforms.GetComponent<LineRenderer>().GetPosition(i);
                Vector3 pos = new Vector3(xarr[i],yarr[i],0f);
                RaycastHit hit;
                
                Debug.DrawRay(pos, (Vector3.forward) * 100, Color.blue);

                if (Physics.Raycast(pos, Vector3.forward, out hit, Mathf.Infinity))
                {
                    Debug.DrawRay(pos, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

                    //Debug.Log(hit.collider.gameObject.name);

                    correct++;
                    //Debug.Log(correct);
                }

            }
            
            if(correct >= 1)
            {
                LineRenderer sp = childTransforms.GetComponent<Transform>().gameObject.GetComponent<LineRenderer>();

                float alpha = 1.0f;
                Gradient gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.green, 1.0f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 0.0f) }
                );
                sp.colorGradient = gradient;

                Benar++;
            }
            else
            {
                LineRenderer sp = childTransforms.GetComponent<Transform>().gameObject.GetComponent<LineRenderer>();
                float alpha = 1.0f;
                Gradient gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 0.0f) }
                );
                sp.colorGradient = gradient;
            }

        }

        Score = (100 / HintParent.GetComponent<Transform>().childCount) * Benar ;
        PlayerPrefs.SetFloat("score", Score);
    }



    public void RestartScene()
    {
        foreach (Transform child in TargetParent.GetComponent<Transform>())
        {
            Destroy(child.gameObject);
        }
        createdLines = new List<GameObject>();
        lineIndex = 0;
        pointIndex = 0;
        Score = 0f;
        LineLeft = LineLimit;
    }
}

