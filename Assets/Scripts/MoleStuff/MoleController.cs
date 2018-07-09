using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleController : MonoBehaviour
{
    Planet planet;

    bool ready;
    float timer;
    GameManager game_manager;

    [SerializeField]
    Animator animator;
    CapsuleCollider collider;

    // Use this for initialization
    void Start()
    {
        planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<Planet>();
        ready = false;
        game_manager = FindObjectOfType<GameManager>();
        timer = Random.Range(-game_manager.SurfaceTime, game_manager.SurfaceTime);
        collider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        StayUp();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Hammer")
        {
            GotHit();
        }
    }

    public void InitializeBunny()
    {
        animator.ResetTrigger("Down");
        animator.ResetTrigger("Smashed");
        collider.enabled = true;
    }

    public void GotHit()
    {
        game_manager.AddHit();
        game_manager.IncreaseDifficulty();
        animator.SetTrigger("Smashed");
        collider.enabled = false;
    }

    public void ResetBunny()
    {
        planet.PlaceObject(GetComponent<GravityBody>(), 0);
        timer = Random.Range(game_manager.SurfaceTime * 0.25f, game_manager.SurfaceTime + 0.05f);
        animator.ResetTrigger("Down");
        animator.ResetTrigger("Smashed");
        ready = false;
    }

    void StayUp()
    {
        if (ready)
        {
            timer += Time.deltaTime;
            if (timer >= game_manager.SurfaceTime)
            {
                animator.SetTrigger("Down");
                timer = 0;
            }
        }
    }

    public void StartDigDown()
    {
        ready = true;
    }

}
