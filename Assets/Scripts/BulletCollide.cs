using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollide : MonoBehaviour {

    public GameObject target;
    private ResourceManager targetResources;
    public int attackDamage = 5;

    // Use this for initialization
    void Start () {
        target = GameObject.FindGameObjectWithTag("Player");
        targetResources = target.GetComponentInChildren<ResourceManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
            if (collision.transform.tag == "Player")
        {
            if (targetResources != null)
            {
                targetResources.damaged = true;
                targetResources.decrementResource(attackDamage);
                GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>().decrementScore(attackDamage);
            }
        }
    }
}
