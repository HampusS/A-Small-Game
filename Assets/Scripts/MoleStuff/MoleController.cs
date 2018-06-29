using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleController : MonoBehaviour {
    [SerializeField]
    float dig_time;
    [SerializeField]
    float bounty;
    Planet planet;

    bool ready;
    float timer;
    ScoreManager score_manager;

    // Use this for initialization
    void Start () {
        planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<Planet>();
        ready = false;
        timer = Random.Range(-dig_time, dig_time);
        score_manager = FindObjectOfType<ScoreManager>();
    }

    void Update () {
        DigUp();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (ready && other.transform.tag == "Hammer")
        {
            GotHit(other.transform.parent.gameObject);
        }
    }

    public void GotHit(GameObject player)
    {
        planet.PlaceObject(GetComponent<GravityBody>(), 0);
        ready = false;
        timer = Random.Range(-dig_time, dig_time * 0.5f);

        //START DIGGING ANIMATION
        PlayerController player_control = player.GetComponent<PlayerController>();
        player_control.Score += bounty + (player_control.ChainScore * bounty);
        player_control.ChainScores();
        score_manager.SetScore(player_control.Score.ToString());
        score_manager.SetChain(player_control.ChainScore.ToString());
    }

    void DigUp()
    {
        if (!ready)
        {
            timer += Time.deltaTime;
            if (timer >= dig_time)
            {
                timer = 0;
                ready = true;
            }
        }
    }

}
