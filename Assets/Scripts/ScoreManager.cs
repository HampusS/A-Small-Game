using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
    public Text score;
    public Text chain;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetScore(string input)
    {
        score.text = input;
    }

    public void SetChain(string input)
    {
        chain.text = input;
    }

}
