using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleController : MonoBehaviour
{
    Planet planet;

    bool ready;
    float timer;
    GameController game_manager;

    [SerializeField]
    Animator animator;
    CapsuleCollider capsCollider;

    [SerializeField]
    GameObject normalBunny;
    [SerializeField]
    GameObject smashedBunny;

    bool gotHit;

    // Use this for initialization
    void Start()
    {
        planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<Planet>();
        ready = false;
        game_manager = FindObjectOfType<GameController>();
        timer = Random.Range(-game_manager.SurfaceTime, game_manager.SurfaceTime);
        capsCollider = GetComponent<CapsuleCollider>();
        gotHit = false;
    }

    void Update()
    {
        StayUp();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gotHit && other.transform.tag == "Hammer")
        {
            GotHit();
        }
    }

    public void InitializeBunny()
    {
        capsCollider.enabled = true;
        smashedBunny.SetActive(false);
        normalBunny.SetActive(true);
        gotHit = false;
    }

    public void GotHit()
    {
        game_manager.AddHit();
        game_manager.IncreaseDifficulty();
        capsCollider.enabled = false;
        smashedBunny.SetActive(true);
        normalBunny.SetActive(false);
        animator.SetTrigger("Smashed");
        ready = true;
        int random = Random.Range(5, 8);
        AudioManager.instance.PlaySound(random);
        timer = 0;
        gotHit = true;
    }

    public void ResetBunny()
    {
        planet.PlaceObject(GetComponent<GravityBody>(), 0);
        timer = Random.Range(game_manager.SurfaceTime * 0.25f, game_manager.SurfaceTime + 0.05f);
        ready = false;
    }

    void StayUp()
    {
        if (ready)
        {
            timer += Time.deltaTime;
            if (timer >= game_manager.SurfaceTime + 0.5f)
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

    public void Escaped()
    {
        capsCollider.enabled = false;
    }

}
