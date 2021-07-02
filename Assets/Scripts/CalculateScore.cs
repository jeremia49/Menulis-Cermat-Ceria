using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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

    // Start is called before the first frame update
    private void Awake()
    {

    }

    void Start()
    {
        PlayerPrefs.SetString("level", LEVEL);
        createdLines = new List<GameObject>();
        LineLeft = LineLimit;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 worldpoint =  Camera.main.ScreenToWorldPoint(touch.position);
            Vector3 touchpos = new Vector3(worldpoint.x, worldpoint.y, 1f);
            

            if (touch.phase == TouchPhase.Began)
            {
                //Debug.Log("Phase Began");
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
                //Debug.Log("Phase Moved")
                LineRenderer targetLine = createdLines[lineIndex].GetComponent<LineRenderer>();
                pointIndex++;
                targetLine.positionCount++;
                targetLine.SetPosition(pointIndex, touchpos);
                targetLine.SetPosition(pointIndex+1, touchpos);
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                
                LineRenderer targetLine = createdLines[lineIndex].GetComponent<LineRenderer>();

                //Vector3[] collPoints = new Vector3[targetLine.positionCount];
                
                MeshCollider meshCollider = createdLines[lineIndex].AddComponent<MeshCollider>();

                /*for (int i = 0; i < targetLine.positionCount; i++)
                {
                    Vector3 linepos = targetLine.GetPosition(i);
                    Vector3 pos = meshCollider.transform.InverseTransformPoint(linepos);
                    collPoints[i] = new Vector3(pos.x, pos.y, pos.z);
                }*/


                Mesh mesh = new Mesh();
                targetLine.BakeMesh(mesh, false);
                meshCollider.sharedMesh = mesh;


                //EdgeCollider2D edgeColl = createdLines[lineIndex].AddComponent<EdgeCollider2D>();

                //edgeColl.points = collPoints;*/

                pointIndex = 0;
                lineIndex++;

                if(LineLeft <= 0)
                {
                    SceneManager.LoadSceneAsync("Result");
                }

            }


        }
        updatePoints();

        LineLeftText.text = "Garis Tersisa : " + LineLeft;
    }

    private void updatePoints()
    {
        var Benar = 0;
        foreach (Transform childTransforms in HintParent.GetComponent<Transform>())
        {
            int total = childTransforms.GetComponent<LineRenderer>().positionCount;
            int correct = 0;
            for (int i = 0; i < total ; i++)
            {
                Vector3 pos = childTransforms.GetComponent<LineRenderer>().GetPosition(i);
                RaycastHit hit;
                
                Debug.DrawRay(pos, (Vector3.forward) * 100, Color.blue);

                if (Physics.Raycast(pos, Vector3.forward, out hit, Mathf.Infinity))
                {
                    Debug.DrawRay(pos, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

                    Debug.Log(hit.collider.gameObject.name);

                    correct++;
                    Debug.Log(correct);
                }


            }
            
            if(correct == total)
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




/*Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
RaycastHit2D hit = Physics2D.Raycast(touchPos, -Vector2.up);

if (hit.collider != null)
{
    LineRenderer sp = hit.collider.gameObject.GetComponent<LineRenderer>();

    float alpha = 1.0f;
    Gradient gradient = new Gradient();
    gradient.SetKeys(
        new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.red, 1.0f) },
        new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
    );
    sp.colorGradient = gradient;

}*/