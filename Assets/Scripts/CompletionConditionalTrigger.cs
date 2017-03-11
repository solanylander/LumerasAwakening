using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletionConditionalTrigger : MonoBehaviour {

    public List<GameObject> triggerObjects;
    public int displacement;
    public float displacementSpeed;
    [SerializeField]
    private List<BeamEventTrigger> completionConditionals;
    private Vector3 originalPosition;
    private Vector3 endPosition;

	// Use this for initialization
	void Start () {
        originalPosition = transform.position;
        endPosition = new Vector3(originalPosition.x, originalPosition.y + displacement, originalPosition.z);
        foreach (GameObject trigger in triggerObjects)
        {
            completionConditionals.Add(trigger.GetComponent<BeamEventTrigger>());
        }
    }

    // Update is called once per frame
    void Update() {
        for (int i = 0; i < completionConditionals.Count; i++)
        {
            if (completionConditionals[i].firstCompletion)
            {
                completionConditionals.Remove(completionConditionals[i]);
            }
        }
        if (completionConditionals.Count.Equals(0))
        {
            //Debug.Log("do stuff");
            transform.position = Vector3.MoveTowards(transform.position, endPosition, displacementSpeed);
        }
	}
}
