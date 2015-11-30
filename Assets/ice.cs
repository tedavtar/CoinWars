using UnityEngine;
using System.Collections;

public class ice : MonoBehaviour {

	float originalDynamicFriction;
	float originalDrag;
	public float iceFriction;
	public float iceDrag;

	void Awake(){
		originalDynamicFriction = 0.4f;
		originalDrag = 0.5f;
		iceFriction = 0.0f;
		iceDrag = 0.00f;
		
	}

	void OnTriggerEnter (Collider col)
	{
		//Debug.Log ("hi");
		//Debug.Log (col.gameObject.GetComponent<Rigidbody> ().velocity);
		//Debug.Log (col.material.dynamicFriction);
		//Debug.Log (col.gameObject.GetComponent<Rigidbody> ().drag);
		col.material.dynamicFriction = iceFriction;
		col.gameObject.GetComponent<Rigidbody> ().drag = iceDrag;
	}

	void OnTriggerExit (Collider col)
	{
		//Debug.Log ("bye");
		col.material.dynamicFriction = originalDynamicFriction;
		col.gameObject.GetComponent<Rigidbody> ().drag = originalDrag;
	}
}
