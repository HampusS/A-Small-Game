using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyShake : MonoBehaviour {

    public ShakeProperties testProperties;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            FindObjectOfType<CameraShake>().StartShake(testProperties);
        }
    }
}
