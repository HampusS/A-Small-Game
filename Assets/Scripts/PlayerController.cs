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
    float sensX = 5, sensY = 5;
    float x, y;
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
        float v = Input.GetAxis("Mouse Y");
        x += (h * sensX);
        y += (v * sensY);
        y = Mathf.Clamp(y, -60, 60);
        cam.localEulerAngles = Vector3.left * y;
        transform.Rotate(Vector3.up * h * sensX);

        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, direction * speed, ref smoothMove, .15f);

        if (grounded && Input.GetButtonDown("Jump"))
            rgdBody.AddForce(transform.up * jumpForce);
    }

    void FixedUpdate()
    {
        rgdBody.MovePosition(rgdBody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
        gravityBody.gravityMultiplier = startGravMult;

        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        grounded = false;
        if (Physics.Raycast(ray, out hit, 1.1f, groundedMask))
            grounded = true;

        if (!grounded)
        {
            if (rgdBody.velocity.y < 0)
            {
                gravityBody.gravityMultiplier = startGravMult * 2;
            }
            else if (rgdBody.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                gravityBody.gravityMultiplier = startGravMult * 3;
            }
        }
    }
}
