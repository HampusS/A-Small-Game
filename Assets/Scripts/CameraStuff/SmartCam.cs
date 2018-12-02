using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartCam : MonoBehaviour {
    [SerializeField]
    Transform target, rotationPivot;
    [SerializeField]
    float verticalStrength = 0.1f;

    [SerializeField]
    Animator animator;
    PlayerController player;

    public bool Enabled { get; set; }

    public float height = 5;
    public float length = 10;
    float follow_strength = 4;



    public static SmartCam Instance;


    // Use this for initialization
    void Start () {

        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Enabled = true;
		if(target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            rotationPivot = target.GetChild(0).transform;
        }
        player = target.GetComponent<PlayerController>();
	}

    // Update is called once per frame
    void Update()
    {
        if (Enabled)
        {
            Vector3 camTarget = target.position + (-target.forward * length) + (target.up * height);
            float dist = Vector3.Distance(transform.position, camTarget);
            transform.position = Vector3.Lerp(transform.position, camTarget, Time.deltaTime * dist * follow_strength);
            float angle = Quaternion.Angle(transform.rotation, rotationPivot.rotation);
            float rotation_strength = (Time.deltaTime * angle) * verticalStrength;
            transform.rotation = Quaternion.Lerp(transform.rotation, rotationPivot.rotation, rotation_strength);
            //ROTATE UP AND DOWN //
            transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y") * 0.25f);

            if (player.shake_cam)
                ShakeCamera();
        }
    }

    public void ShakeCamera()
    {
        animator.SetTrigger("Shake");
    }

    public void ResetCamera()
    {
        animator.ResetTrigger("Shake");
        player.shake_cam = false;
    }
}
