using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartCam : MonoBehaviour {
    [SerializeField]
    Transform target, rotationPivot;
    [SerializeField]
    float verticalStrength = 0.1f;

    // Use this for initialization
    void Start () {
		if(target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            rotationPivot = target.GetChild(0).transform;
        }
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 camTarget = target.position + (-target.forward * 10) + (target.up * 4);
        float dist = Vector3.Distance(transform.position, camTarget);
        transform.position = Vector3.Lerp(transform.position, camTarget, Time.deltaTime * dist * 2);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationPivot.rotation, Time.deltaTime * Quaternion.Angle(transform.rotation, rotationPivot.rotation) * verticalStrength);
        transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y"));
    }
}
