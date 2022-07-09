using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class OceanRipple : MonoBehaviour
{
    [Header("Ripple Parameters")]
    [SerializeField] private float rippleAmplitude;
    [SerializeField] private float rippleSpeed;
    [SerializeField] private float rippleFrequency; 

    [Header("Ocean Parameters")]
    [SerializeField] private int oceanSize;
    [SerializeField] private int frameCount;

    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector3> normals = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uvs = new List<Vector2>();

    private Vector3 center;
    private MeshFilter meshFilter;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    private void Start()
    {
        center = new Vector3(oceanSize / 2f, 0, oceanSize / 2f);
        CreateOcean();
    }

    private void Update()
    {
        RippleMove();
        UpdateMesh();
    }

    private void CreateOcean()
    {
        float frame = oceanSize / frameCount;
        for (int i = 0; i < frameCount + 1; i++)
        {
            for (int j = 0; j < frameCount + 1; j++)
            {
                vertices.Add(new Vector3(j * frame, 0, i * frame));
                normals.Add(Vector3.up);
                uvs.Add(new Vector2(i / (float)frameCount, j / (float)frameCount));
            }
        }


        for (int i = 0; i < frameCount; i++)
        {
            for (int j = 0; j < frameCount; j++)
            {
                int currentIndex = (i * frameCount) + i + j;

                //Triangle 1
                triangles.Add(currentIndex);
                triangles.Add(currentIndex + frameCount + 1);
                triangles.Add(currentIndex + frameCount + 2);
                //Triangle 2
                triangles.Add(currentIndex);
                triangles.Add(currentIndex + frameCount + 2);
                triangles.Add(currentIndex + 1);
            }
        }
    }

    private void RippleMove()
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 currentVertex = vertices[i];
            float distance_to_Center = Vector3.Distance(center, currentVertex);
            currentVertex.y = rippleAmplitude * Mathf.Sin(Time.timeSinceLevelLoad * rippleSpeed + rippleFrequency * distance_to_Center);
            vertices[i] = currentVertex;
        }
    }

    private void UpdateMesh()
    {
        meshFilter.mesh.SetVertices(vertices);
        meshFilter.mesh.SetTriangles(triangles, 0);
        meshFilter.mesh.SetNormals(normals);
        meshFilter.mesh.SetUVs(0, uvs);
    }
}
