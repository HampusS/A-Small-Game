using UnityEngine;

public class GravityBody : MonoBehaviour
{
    public Planet source;
    public float gravityMultiplier { get; set; }

    // Use this for initialization
    void Awake()
    {
        source = GameObject.FindGameObjectWithTag("Planet").GetComponent<Planet>();
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        gravityMultiplier = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        source.Attract(transform, gravityMultiplier);
    }
}
