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
    RaycastHit hit, hammerHit;
    [SerializeField]
    float surface_align_speed;
    float height;

    bool stop_moving = false;
    [SerializeField]
    float cooldown;
    float swing_timer;
    bool swinging_hammer;
    bool cooldown_swing;
    public bool shake_cam { get; set; }

    [SerializeField]
    Collider hammer;

    [SerializeField]
    Animator animator;
    [SerializeField]
    GameObject dust;
    [SerializeField]
    Transform ImpactPoint;

    GameController game_manager;

    public bool Enabled { get; set; }



    public static PlayerController Instance;



    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        rgdBody = GetComponent<Rigidbody>();
        height = GetComponent<CapsuleCollider>().height;
        Cursor.lockState = CursorLockMode.Locked;
        game_manager = FindObjectOfType<GameController>();
        Enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Enabled)
        {
            if (!stop_moving)
            {
                float angle = Input.GetAxis("Mouse X") * sensX;
                transform.Rotate(Vector3.up, angle);
                Vector3 strafe = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
                if (strafe != Vector3.zero)
                    animator.SetTrigger("Moving");
                else
                    animator.ResetTrigger("Moving");

                moveAmount = Vector3.SmoothDamp(moveAmount, strafe * speed, ref smoothMove, moveFloatiness);

            }

            SurfaceContactInfo();
            SwingHammer();
        }
        transform.position += transform.TransformDirection(moveAmount) * Time.deltaTime;
    }

    void SurfaceContactInfo()
    {
        Vector3 raydirect = (planet.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, raydirect);
        float rayLength = Vector3.Distance(transform.position, planet.position);
        if (Physics.Raycast(ray, out hit, rayLength, groundedMask))
        {
            float surfDist = Vector3.Distance(hit.point, transform.position);
            Vector3 offset = Vector3.zero;
            if (surfDist > 1)
                offset = hit.normal * ((height * 0.5f) - surfDist);

            transform.position = Vector3.Lerp(transform.position, transform.position + offset, Time.deltaTime * 6);

            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * surface_align_speed);
        }
    }

    void SwingHammer()
    {
        if (!swinging_hammer && Input.GetMouseButtonDown(0))
        {
            swinging_hammer = true;
            hammer.gameObject.SetActive(true);
            stop_moving = true;
            moveAmount = Vector3.zero;
            animator.SetTrigger("Swing");
        }
        else if (swinging_hammer)
        {
            swing_timer += Time.deltaTime;
            if (swing_timer >= cooldown)
            {
                swing_timer = 0;
                swinging_hammer = false;
                stop_moving = false;
                hammer.gameObject.SetActive(false);
                animator.SetTrigger("Idle");
                game_manager.PauseTimer = false;
            }
        }
    }

    public void HammerImpact()
    {
        shake_cam = true;
        AudioManager.instance.PlaySound(1);

        float rayLength = Vector3.Distance(ImpactPoint.position, planet.position);
        Vector3 raydirect = (planet.position - ImpactPoint.position).normalized;
        Ray ray = new Ray(ImpactPoint.position, raydirect);
        Vector3 impactPos = Vector3.zero;
        Quaternion impactRotation = Quaternion.identity;

        if (Physics.Raycast(ray, out hammerHit, rayLength, groundedMask))
        {
            impactPos = hammerHit.point;
            impactRotation = Quaternion.LookRotation(hammerHit.normal);
        }

        GameObject clone = Instantiate(dust, impactPos, impactRotation);
        Destroy(clone, 3);
    }

}
