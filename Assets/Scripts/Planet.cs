using UnityEngine;

public class Planet : MonoBehaviour
{
    public float gravity = -9.8f;
    public float sizeMultiplier = 1;
    [Range(0, 1)]
    public float smoothMultiplier = 0.5f;
    float smoothness = 1;
    MeshCollider meshCollider;
    Mesh mesh;

    [SerializeField]
    GravityBody[] worldBodies;

    void Awake()
    {
        meshCollider = gameObject.AddComponent<MeshCollider>();
        SetTerrain();
    }

    void SetTerrain()
    {
        smoothness = sizeMultiplier * (smoothMultiplier + 1);
        mesh = GetComponent<MeshFilter>().mesh;
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

        foreach (GravityBody body in worldBodies)
        {
            PlaceObject(body, 0.5f);
        }
    }

    public void PlaceObject(GravityBody body, float distance)
    {
        int index = Random.Range(0, mesh.vertices.Length);
        Vector3 vect = mesh.vertices[index];
        body.SetPosition(vect);
        body.Rotate(vect - transform.position);
        body.Align(mesh.normals[index], distance);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SetTerrain();

            MoleSpawner find = FindObjectOfType<MoleSpawner>();
            foreach (GameObject mole in find.Moles())
            {
                PlaceObject(mole.GetComponent<GravityBody>(), 0);
            }
        }
    }

    public void Attract(Rigidbody target, float gravityMultiplier, Vector3 direction)
    {
        target.AddForce(direction * gravity * gravityMultiplier);
    }
}
