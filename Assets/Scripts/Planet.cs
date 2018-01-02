using UnityEngine;

public class Planet : MonoBehaviour
{
    public float gravity = -9.8f;
    public float sizeMultiplier = 1;
    public float smoothness = 1;
    MeshCollider meshCollider;

    private void Start()
    {
        meshCollider = gameObject.AddComponent<MeshCollider>();
        SetTerrain();
    }

    private void SetTerrain()
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
    }

    public void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            SetTerrain();
    }

    public void Attract(Transform target, float gravityMultiplier)
    {
        Vector3 normal = (target.position - transform.position).normalized;
        target.GetComponent<Rigidbody>().AddForce(normal * gravity * gravityMultiplier);
    }
}
