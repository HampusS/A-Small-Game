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
    [SerializeField]
    float moveFloatiness = .15f;
    float startGravMult;
    bool grounded;
    bool jump;
    Vector3 moveAmount;
    Vector3 smoothMove;
    Rigidbody rgdBody;
    GravityBody gravityBody;
    RaycastHit hit;

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
        jump = Input.GetButtonDown("Jump");
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * sensX);
        Vector3 strafe = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, strafe * speed, ref smoothMove, moveFloatiness);

        if (grounded)
            GetComponent<Renderer>().material.color = Color.green;
        else
            GetComponent<Renderer>().material.color = Color.red;

        if (grounded && jump)
        {
            gravityBody.Align(transform.up, 0.15f);
            rgdBody.AddForce(transform.up * jumpForce);
        }

        SurfaceContactInfo();
    }

    void FixedUpdate()
    {
        gravityBody.gravityMultiplier = startGravMult;

        //if (rgdBody.velocity.y < 0 || rgdBody.velocity.y > 0 && !Input.GetButton("Jump")) // NOT WORKING PROPERLY  -  UPSIDEDOWN NEEDS INVERTED NUMBERS
        //{
        //    gravityBody.gravityMultiplier = startGravMult * 4;
        //}

        rgdBody.MovePosition(rgdBody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    void SurfaceContactInfo()
    {
        grounded = false;
        Vector3 raydirect = (planet.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, raydirect);
        float rayLength = Vector3.Distance(transform.position, planet.position);
        if (Physics.Raycast(ray, out hit, rayLength, groundedMask))
        {
            float surfDist = Vector3.Distance(hit.point, transform.position);
            Debug.Log(hit.normal * ((GetComponent<CapsuleCollider>().height * 0.5f) - surfDist) + " " + (hit.normal * -surfDist) + " " + hit.normal * (GetComponent<CapsuleCollider>().height * 0.5f));
            if (!jump && surfDist < GetComponent<CapsuleCollider>().height * 0.6f)
            {
                transform.position += hit.normal * ((GetComponent<CapsuleCollider>().height * 0.5f) - surfDist);
                grounded = true;
                gravityBody.gravityMultiplier = 0;
                rgdBody.velocity = Vector3.zero;
                gravityBody.gravityDirection = hit.normal;
            }
            else if (surfDist > jumpForce * Time.fixedDeltaTime * 0.6f)
            {
                gravityBody.gravityDirection = -raydirect;
            }
        }

        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2);
    }

}
