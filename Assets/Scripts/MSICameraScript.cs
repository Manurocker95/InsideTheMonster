using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSICameraScript : MonoBehaviour
{
    public Transform target;
    public float fixedY;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = new Vector3(transform.position.x, target.position.y + fixedY, transform.position.z);
    }
}
