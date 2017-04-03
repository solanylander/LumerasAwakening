using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour {
    
    public float scrollSpeed = 0.2F;
    public GameObject player;
    public Renderer rend;
    Vector3 position;

    void Start()
    {
        rend = GetComponent<Renderer>();
        position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
    }
    void Update()
    {
        float offset = Time.time * scrollSpeed;
        rend.material.SetTextureOffset("_MainTex", new Vector2(0, -offset));
    }

    void OnTriggerEnter(Collider other)
    {
        player.transform.position = position;
        player.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
    void OnTriggerStay(Collider other)
    {
        player.transform.position = position;
        player.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
