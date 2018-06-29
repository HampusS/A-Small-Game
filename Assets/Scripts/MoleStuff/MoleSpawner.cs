using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleSpawner : MonoBehaviour {
    [SerializeField]
    GameObject mole;
    [SerializeField]
    int amount;
    [SerializeField]
    Planet planet;
    GameObject mole_holder;

    List<GameObject> mole_list;
    public List<GameObject> Moles() { return mole_list; }

	// Use this for initialization
	void Start () {
        mole_list = new List<GameObject>();
        mole_holder = new GameObject();
        mole_holder.name = "MoleHolder";

        for (int i = 0; i < amount; i++)
        {
            GameObject clone = Instantiate(mole, mole_holder.transform);
            planet.PlaceObject(clone.GetComponent<GravityBody>(), 0);
            mole_list.Add(clone);
        }
	}

}
