//MoveCar.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCar : MonoBehaviour
{
    [SerializeField] Vector3 displacement;
    [SerializeField] float angle;
    [SerializeField] AXIS rotationAxis;

    public GameObject wheel;

    Mesh mesh;
    Mesh wheelMesh1;
    Mesh wheelMesh2;
    Mesh wheelMesh3;
    Mesh wheelMesh4;
    Vector3[] baseVertices;
    Vector3[] baseVerticesW1;
    Vector3[] baseVerticesW2;
    Vector3[] baseVerticesW3;
    Vector3[] baseVerticesW4;

    Vector3[] newVertices;
    Vector3[] newVerticesW1;
    Vector3[] newVerticesW2;
    Vector3[] newVerticesW3;
    Vector3[] newVerticesW4;

    // game objects 
    GameObject wheel1;
    GameObject wheel2;
    GameObject wheel3;
    GameObject wheel4;
    // Start is called before the first frame update
    void Start()
    {
        wheel1 = Instantiate(wheel, new Vector3(1.0f, 0.5f, -1.4f), Quaternion.identity);
        wheel2 = Instantiate(wheel, new Vector3(1.0f, 0.5f, 1.4f), Quaternion.identity);
        wheel3 = Instantiate(wheel, new Vector3(-1.0f, 0.5f, 1.4f), Quaternion.identity);
        wheel4 = Instantiate(wheel, new Vector3(-1.0f, 0.5f, -1.4f), Quaternion.identity);

        // mesh
        mesh = GetComponentInChildren<MeshFilter>().mesh;
        wheelMesh1 = wheel1.GetComponentInChildren<MeshFilter>().mesh;
        wheelMesh2 = wheel2.GetComponentInChildren<MeshFilter>().mesh;
        wheelMesh3 = wheel3.GetComponentInChildren<MeshFilter>().mesh;
        wheelMesh4 = wheel4.GetComponentInChildren<MeshFilter>().mesh;


        // base Vertices
        baseVertices = mesh.vertices;
        baseVerticesW1 = wheelMesh1.vertices;
        baseVerticesW2 = wheelMesh2.vertices;
        baseVerticesW3 = wheelMesh3.vertices;
        baseVerticesW4 = wheelMesh4.vertices;

        // allocate memory for the copy
        newVertices = new Vector3[baseVertices.Length];
        newVerticesW1 = new Vector3[baseVerticesW1.Length];
        newVerticesW2 = new Vector3[baseVerticesW2.Length];
        newVerticesW3 = new Vector3[baseVerticesW3.Length];
        newVerticesW4 = new Vector3[baseVerticesW4.Length];

        // copy the coordinates
        for (int i = 0; i < baseVertices.Length; i++)
        {
            newVertices[i] = baseVertices[i];
            
        }

        // copy the coordinates of wheels
        for (int i = 0; i < baseVerticesW1.Length; i++)
        {
            newVerticesW1[i] = baseVerticesW1[i];
            newVerticesW2[i] = baseVerticesW2[i];
            newVerticesW3[i] = baseVerticesW3[i];
            newVerticesW4[i] = baseVerticesW4[i];

        }



    }

    // Update is called once per frame
    void Update()
    {
        DoTransform();
    }

    void DoTransform()
    {


        Matrix4x4 move = HW_Transforms.TranslationMat(displacement.x * Time.time, displacement.y * Time.time, displacement.z * Time.time);

        Matrix4x4 moveOrigin = HW_Transforms.TranslationMat(-displacement.x, -displacement.y, -displacement.z);

        Matrix4x4 moveObject = HW_Transforms.TranslationMat(displacement.x, displacement.y, displacement.z);

        Matrix4x4 rotate = HW_Transforms.RotateMat(Time.time * angle, rotationAxis);

        // el q mueve al pivote no tiene que usar time.time
      
        Matrix4x4 composite = move;
        for (int i = 0; i < newVertices.Length; i++)
        {
            Vector4 temp = new Vector4(baseVertices[i].x, baseVertices[i].y, baseVertices[i].z, 1);
            newVertices[i] = composite * temp;

        }
        Matrix4x4 wheelComposite = composite * rotate;

        for (int i = 0; i < newVerticesW1.Length; i++)
        {
            Vector4 temp = new Vector4(baseVerticesW1[i].x, baseVerticesW1[i].y, baseVerticesW1[i].z, 1);
            newVerticesW1[i] = wheelComposite * temp;
            newVerticesW2[i] = wheelComposite * temp;
            newVerticesW3[i] = wheelComposite * temp;
            newVerticesW4[i] = wheelComposite * temp;
        }

        // Assign the new vertices to the mesh
        mesh.vertices = newVertices;
        wheelMesh1.vertices = newVerticesW1;
        wheelMesh2.vertices = newVerticesW2;
        wheelMesh3.vertices = newVerticesW3;
        wheelMesh4.vertices = newVerticesW4;

        // recalculate normals
        mesh.RecalculateNormals();
    }
}
