using UnityEngine;

public class GravityBody : MonoBehaviour
{
    public Planet source;
    public float gravityMultiplier { get; set; }
    public Vector3 gravityDirection { get; set; }
    Rigidbody rgdbody;

    // Use this for initialization
    void Awake()
    {
        if (source == null)
            source = GameObject.FindGameObjectWithTag("Planet").GetComponent<Planet>();
        rgdbody = GetComponent<Rigidbody>();
        rgdbody.useGravity = false;
        rgdbody.constraints = RigidbodyConstraints.FreezeRotation;
        gravityMultiplier = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        source.Attract(rgdbody, gravityMultiplier, gravityDirection);
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void Rotate(Vector3 direction)
    {
        transform.rotation = Quaternion.FromToRotation(transform.up, direction) * transform.rotation;
    }

    public void Align(Vector3 direction, float amount)
    {
        transform.position += direction * (GetComponent<CapsuleCollider>().height * amount);
    }
}
