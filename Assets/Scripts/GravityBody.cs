using UnityEngine;

public class GravityBody : MonoBehaviour
{
    public Planet source;
    Rigidbody rgdbody;
    bool grounded;
    RaycastHit hit;

    // Use this for initialization
    void Awake()
    {
        if (source == null)
            source = GameObject.FindGameObjectWithTag("Planet").GetComponent<Planet>();
        rgdbody = GetComponent<Rigidbody>();
        rgdbody.useGravity = false;
        rgdbody.constraints = RigidbodyConstraints.FreezeRotation;
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
