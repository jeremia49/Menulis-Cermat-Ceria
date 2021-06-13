using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CalculateScore : MonoBehaviour
{
    public Color color;
    public GameObject HintParent;
    public GameObject TargetParent;
    public List<GameObject> createdLines;
    public int lineIndex = 0;
    public Material material;
    public int pointIndex = 0;
    public bool stationary = false;
    public int stationCount = 0;

    public float Score = 0;

    public int LineLimit;
    public int LineLeft;

    // Start is called before the first frame update
    private void Awake()
    {
        /*foreach (Transform childTransforms in HintParent.GetComponent<Transform>())
        {
            LineRenderer targetLine = childTransforms.gameObject.GetComponent<LineRenderer>();
            Vector2 startPos = targetLine.GetPosition(0);
            Vector2 endPos = targetLine.GetPosition(1);

            EdgeCollider2D edgeColl = childTransforms.gameObject.AddComponent<EdgeCollider2D>();
            edgeColl.points = new Vector2[2] { new Vector2(startPos.x, startPos.y), new Vector2(endPos.x, endPos.y) };
        }*/
    }

    void Start()
    {
        createdLines = new List<GameObject>();
        LineLeft = LineLimit;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            
            if(LineLeft == 0)
            {
                return;
            }
            
            Touch touch = Input.GetTouch(0);
            Vector3 worldpoint =  Camera.main.ScreenToWorldPoint(touch.position);
            Vector3 touchpos = new Vector3(worldpoint.x, worldpoint.y, 0f);
            

            if (touch.phase == TouchPhase.Began)
            {

                //Debug.Log("Phase Began");
                GameObject lineObject = new GameObject();
                lineObject.GetComponent<Transform>().SetParent(TargetParent.GetComponent<Transform>());
                lineObject.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, 0f);
                lineObject.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
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
                //Debug.Log("Phase Moved");
                stationary = false;
                LineRenderer targetLine = createdLines[lineIndex].GetComponent<LineRenderer>();
                if(pointIndex == 0)
                {
                    targetLine.SetPosition(1, touchpos);
                    return;
                }
                //Debug.LogError("Point Index :" + pointIndex);
                //targetLine.SetPosition(pointIndex, touchpos);
                targetLine.SetPosition(pointIndex, touchpos);
                targetLine.SetPosition(pointIndex+1, touchpos);
            }
            else if(touch.phase == TouchPhase.Stationary)
            {
                //Debug.Log("Phase Stationary :"+ stationCount.ToString());
                stationCount++;

                if (!stationary && stationCount >= 2)
                {
                    pointIndex++;
                    LineRenderer targetLine = createdLines[lineIndex].GetComponent<LineRenderer>();
                    targetLine.positionCount++;
                    targetLine.SetPosition(pointIndex, touchpos);
                    targetLine.SetPosition(pointIndex + 1, touchpos);
                    stationary = true;
                    stationCount = 0;
                }
            }else if(touch.phase == TouchPhase.Ended)
            {
                //Debug.Log("Phase Ended");
                LineRenderer targetLine = createdLines[lineIndex].GetComponent<LineRenderer>();
                //targetLine.SetPosition(pointIndex, touchpos);

                EdgeCollider2D edgeColl = createdLines[lineIndex].AddComponent<EdgeCollider2D>();

                Vector2[] collPoints = new Vector2[targetLine.positionCount];
               
                for (int i = 0; i < targetLine.positionCount; i++)
                {
                    Vector3 linepos = targetLine.GetPosition(i);
                    Vector2 pos = edgeColl.transform.InverseTransformPoint(linepos);

                    collPoints[i] = new Vector2(pos.x, pos.y);
                }

                edgeColl.points = collPoints;


                //Mesh mesh = new Mesh();
                //targetLine.BakeMesh(mesh, false);
                //meshcoll.sharedMesh = mesh;


                pointIndex = 0;
                lineIndex++;

                //createdLines[lineIndex].AddComponent<Rigidbody2D>();
            }


        }
        updatePoints();
    }

    private void updatePoints()
    {

        foreach (Transform childTransforms in HintParent.GetComponent<Transform>())
        {

            Vector2 startChildPos = childTransforms.GetComponent<LineRenderer>().GetPosition(0);
            Vector2 endChildPos = childTransforms.GetComponent<LineRenderer>().GetPosition(1);


            Vector2 hintPos = new Vector2((startChildPos.x + endChildPos.x) / 2, ( (startChildPos.y + endChildPos.y) / 2) + 0.05f );
            Vector2 hintPos1 = new Vector2((startChildPos.x), ((startChildPos.y + endChildPos.y) / 2));
            //Debug.Log(hintPos);

            RaycastHit2D hit1 = Physics2D.Raycast(hintPos, Vector2.down, 0.1f);
            RaycastHit2D hit2 = Physics2D.Raycast(hintPos1, Vector2.left, 0.1f);

            if (hit1.collider != null || hit2.collider !=null)
            {
                //Debug.Log(childTransforms.name + " Hit " + hit1.collider.gameObject.GetComponent<Transform>().name);
                    
                LineRenderer sp = childTransforms.GetComponent<Transform>().gameObject.GetComponent<LineRenderer>();

                float alpha = 1.0f;
                Gradient gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.green, 1.0f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 0.0f) }
                );
                sp.colorGradient = gradient;

                continue;
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