using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMesh : MonoBehaviour
{
    void OnPreRender()
    {
        GL.wireframe = true;
    }
    void OnPostRender()
    {
        GL.wireframe = true;
    }
}
