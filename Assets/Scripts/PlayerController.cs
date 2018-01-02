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
    float sensX = 5, sensY = 5;
    float startGravMult;
    Transform cam;

    bool grounded;
    Vector3 moveAmount;
    Vector3 smoothMove;
    Rigidbody rgdBody;
    GravityBody gravityBody;

    // Use this for initialization
    void Start()
    {
        cam = Camera.main.transform;
        rgdBody = GetComponent<Rigidbody>();
        gravityBody = GetComponent<GravityBody>();
        startGravMult = gravityBody.gravityMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * h * sensX);

        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, direction * speed, ref smoothMove, .15f);

        if (grounded && Input.GetButtonDown("Jump"))
            rgdBody.AddForce(transform.up * jumpForce);
    }

    void FixedUpdate()
    {
        SurfaceContactInfo();

        gravityBody.gravityMultiplier = startGravMult;
        GetComponent<Renderer>().material.color = Color.green;
        if (!grounded)
        {
            GetComponent<Renderer>().material.color = Color.red;

            if (rgdBody.velocity.y < 0)
            {
                gravityBody.gravityMultiplier = startGravMult * 2;
            }
            else if (rgdBody.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                gravityBody.gravityMultiplier = startGravMult * 3;
            }
        }

        rgdBody.MovePosition(rgdBody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    private void SurfaceContactInfo()
    {
        Vector3 raydirect = (planet.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, raydirect);
        RaycastHit hit;
        grounded = false;
        float dist = Vector3.Distance(transform.position, planet.position);
        if (Physics.Raycast(ray, out hit, dist, groundedMask))
        {
            float surfDist = Vector3.Distance(hit.point, transform.position);
            if (surfDist < GetComponent<CapsuleCollider>().height * 0.7f)
            {
                grounded = true;
            }
        }
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2);
        gravityBody.gravitDirection = hit.normal;
    }

}
