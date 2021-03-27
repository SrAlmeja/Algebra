using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCreator : MonoBehaviour
{
    public Material material;

    Mesh mesh;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    Vector3[] vertex;



    int[] triangles;

    List<int> triangles_2;
    // Start is called before the first frame update
    void Start()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();

        meshRenderer = gameObject.AddComponent<MeshRenderer>();

        meshRenderer.material = material;

        meshFilter.mesh = mesh;

        vertex = new[]
        {
            new Vector3(0,0,0), //0
            new Vector3(0,1,0), //1
            new Vector3(1,0,0), //2
            new Vector3(1,1,0), //3

            new Vector3(1,0,1), //4
            new Vector3(1,1,1), //5

            new Vector3(0,0,1), //6

            new Vector3(0,1,1), //7
        };

        /*vertex = new Vector3[3];
        vertex[0] = new Vector3(0, 0, 0);
        vertex[1] = new Vector3(0, 1, 0);
        vertex[2] = new Vector3(1, 0, 0);*/

        meshFilter.mesh.vertices = vertex;

        triangles = new[]
        {
            0,1,2,  // T_1
            1,3,2,  // T_2
            2,3,4,  // T_3
            5,4,3,  // T_4  
            0,2,4,  // T_5
            6,0,4,  // T_6
            4,5,6,  // T_7
            5,7,6,  // T_8
            6,7,1,  // T_9
            1,0,6,  // T_10
            1,7,3,  // T_11
            3,7,5,  // T_12


        };

        meshFilter.mesh.triangles = triangles;

        /*
        triangle = new int[4];

        triangles_2 = new List<int>();

        triangle[0] = 1;

        triangles_2[0] = 1;

        triangles_2 
        */
    }

    
}
