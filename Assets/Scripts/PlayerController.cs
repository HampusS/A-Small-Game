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
    Transform planet;
    [SerializeField]
    float sensX;
    [SerializeField]
    float moveFloatiness = .1f;
    Vector3 moveAmount;
    Vector3 smoothMove;
    Rigidbody rgdBody;
    GravityBody gravityBody;
    RaycastHit hit;
    [SerializeField]
    float surface_align_speed;
    float height;

    bool stop_moving = false;
    [SerializeField]
    float cooldown, swing_time;
    float swing_timer;
    bool swinging_hammer;
    bool cooldown_swing;

    [SerializeField]
    Collider hammer;

    public float Score { get; set; }
    public float ChainScore { get; set; }
    float chain_timer, chain_limit = 6;
    
    void Start()
    {
        rgdBody = GetComponent<Rigidbody>();
        gravityBody = GetComponent<GravityBody>();
        height = GetComponent<CapsuleCollider>().height;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop_moving)
        {
            float angle = Input.GetAxis("Mouse X") * Time.deltaTime;
            transform.Rotate(Vector3.up, angle * 200);
            Vector3 strafe = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            strafe = new Vector3(strafe.x * 0.5f, 0, strafe.z);
            if (strafe.z < 0)
                strafe *= 0.25f;
            moveAmount = Vector3.SmoothDamp(moveAmount, strafe * speed, ref smoothMove, moveFloatiness);
            if (Input.GetKeyDown(KeyCode.Space))
                rgdBody.AddForce(transform.up * 200);
        }
        Debug.Log(rgdBody.velocity);

        SurfaceContactInfo();
        SwingHammer();
        chain_timer += Time.deltaTime;
        if (chain_timer > chain_limit)
        {
            ChainScore = 0;
            FindObjectOfType<ScoreManager>().SetChain("0");
        }
    }

    void FixedUpdate()
    {
        rgdBody.MovePosition(rgdBody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    void SurfaceContactInfo()
    {
        Vector3 raydirect = (planet.position - transform.position).normalized;
        gravityBody.gravityDirection = -raydirect;

        Ray ray = new Ray(transform.position, raydirect);
        float rayLength = Vector3.Distance(transform.position, planet.position);
        if (Physics.Raycast(ray, out hit, rayLength, groundedMask))
        {
            float surfDist = Vector3.Distance(hit.point, transform.position);
            if (surfDist < height * 0.6f)
            {
                transform.position += hit.normal * ((height * 0.5f) - surfDist);
                rgdBody.velocity = Vector3.zero;
                gravityBody.gravityDirection = hit.normal;
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                transform.position += hit.normal * ((height * 0.65f) - surfDist);
                rgdBody.AddForce((transform.up - transform.forward * 1.5f).normalized * 500);
            }
        }

        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * surface_align_speed);
    }

    void SwingHammer()
    {
        if (!swinging_hammer && !cooldown_swing && Input.GetMouseButtonDown(0))
        {
            swinging_hammer = true;
            hammer.gameObject.SetActive(true);
            stop_moving = true;
            moveAmount = Vector3.zero;
        }
        else if (swinging_hammer)
        {
            swing_timer += Time.deltaTime;
            if (swing_timer >= cooldown)
            {
                swing_timer = 0;
                cooldown_swing = true;
                swinging_hammer = false;
                stop_moving = false;
                hammer.gameObject.SetActive(false);
            }
        }
        else if (cooldown_swing)
        {
            swing_time += Time.deltaTime;
            if (swing_time >= cooldown)
            {
                cooldown_swing = false;
                swing_time = 0;
            }
        }


    }

    public void ChainScores()
    {
        ChainScore++;
        chain_timer = 0;
    }

}
