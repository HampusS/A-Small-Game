using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    LayerMask groundedMask;
    [SerializeField]
    float speed = 10;
    [SerializeField]
    float jumpForce = 300;
    [SerializeField]
    Transform planet;
    [SerializeField]
    float sensX = 0.005f;
    float startGravMult;

    bool grounded;
    Vector3 moveAmount;
    Vector3 smoothMove;
    Rigidbody rgdBody;
    GravityBody gravityBody;

    // Use this for initialization
    void Start()
    {
        rgdBody = GetComponent<Rigidbody>();
        gravityBody = GetComponent<GravityBody>();
        startGravMult = gravityBody.gravityMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * sensX);
        Vector3 strafe = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, strafe * speed, ref smoothMove, .15f);


        GetComponent<Renderer>().material.color = Color.red;

        if (grounded && Input.GetButtonDown("Jump"))
            rgdBody.AddForce(transform.up * jumpForce);
        else if (grounded)
            GetComponent<Renderer>().material.color = Color.green;

        SurfaceContactInfo();
    }

    void FixedUpdate()
    {
        gravityBody.gravityMultiplier = startGravMult;

        if (rgdBody.velocity.y < 0)
        {
            gravityBody.gravityMultiplier = startGravMult * 2;
        }
        else if (rgdBody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            gravityBody.gravityMultiplier = startGravMult * 3;
        }
        rgdBody.MovePosition(rgdBody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    private void SurfaceContactInfo()
    {
        Vector3 raydirect = (planet.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, raydirect);
        RaycastHit hit;
        grounded = false;
        float rayLength = Vector3.Distance(transform.position, planet.position);
        if (Physics.Raycast(ray, out hit, rayLength, groundedMask))
        {
            float surfDist = Vector3.Distance(hit.point, transform.position);
            if (surfDist < GetComponent<CapsuleCollider>().height * 0.8f)
            {
                rgdBody.position = (hit.normal * (hit.point.magnitude + (GetComponent<CapsuleCollider>().height * 0.5f)));
                grounded = true;
                gravityBody.gravityMultiplier = 0;
                rgdBody.velocity = Vector3.zero;
            }
        }

        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2);
        gravityBody.gravityDirection = hit.normal;
    }

}
