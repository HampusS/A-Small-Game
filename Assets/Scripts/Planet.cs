using UnityEngine;

public class Planet : MonoBehaviour
{
    public float gravity = -9.8f;
    public float sizeMultiplier = 1;
    public float smoothness = 1;
    MeshCollider meshCollider;

    void Start()
    {
        meshCollider = gameObject.AddComponent<MeshCollider>();
        SetTerrain();
    }

    void SetTerrain()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        float offsetX = Random.Range(0.0f, 1.0f);
        float offsetY = Random.Range(0.0f, 1.0f);
        
        for (int i = 0; i < vertices.Length; ++i)
        {
            float xCoord = vertices[i].x / smoothness + offsetX;
            float yCoord = vertices[i].y / smoothness + offsetY;
            vertices[i] = mesh.normals[i] * sizeMultiplier * Mathf.PerlinNoise(xCoord, yCoord);
        }
        mesh.vertices = vertices;
        DestroyImmediate(meshCollider);
        meshCollider = gameObject.AddComponent<MeshCollider>();
        mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 2000);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            SetTerrain();
    }

    public void Attract(Rigidbody target, float gravityMultiplier, Vector3 direction)
    {
        target.AddForce(direction * gravity * gravityMultiplier);
    }
}
