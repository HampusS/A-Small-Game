using UnityEngine;

public class GravityBody : MonoBehaviour
{
    public Planet source;
    public float gravityMultiplier { get; set; }
    public Vector3 gravitDirection { get; set; }
    Rigidbody rgdbody;
    // Use this for initialization
    void Awake()
    {
        source = GameObject.FindGameObjectWithTag("Planet").GetComponent<Planet>();
        rgdbody = GetComponent<Rigidbody>();
        rgdbody.useGravity = false;
        rgdbody.constraints = RigidbodyConstraints.FreezeRotation;
        gravityMultiplier = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        source.Attract(rgdbody, gravityMultiplier, gravitDirection);
    }
}
