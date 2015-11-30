using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class focusSimple : MonoBehaviour {

	Transform target;
	float smoothing = 3.0f;
	float threshold = 0.5f;
	
	float waitTime = 0.1f;
	
	float smallHalo = 1.0f;
	float largeHalo = 2.0f;
	
	Component halo;
	
	bool selected;

	public GameObject smallcoin;


	// Use this for initialization
	void Start () {
		smallHalo = 1.0f;
		largeHalo = 1.4f;

		halo = GetComponent("Halo");
		halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
		selected = false;

		target = GameObject.Find ("Main Camera").GetComponent<Viewer> ().target;
	}

	public void deselect(){
		selected = false;
		halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void handleSelection(){
		StopAllCoroutines ();

		StartCoroutine ("lerpCamera");
		if (gameObject.GetComponent<coinProps> ().locked) {
			return;
		}
		if (!selected) {
			if (gameObject.tag == "p1"){
				foreach(GameObject c in GameObject.FindGameObjectsWithTag("p1"))
				{
					//Debug.Log('i');
					c.GetComponent<focusSimple>().deselect();
					
				}
			} else {
				foreach(GameObject c in GameObject.FindGameObjectsWithTag("p2"))
				{
					c.GetComponent<focusSimple>().deselect();
				}
			}
			selected = true;
			halo.GetType ().GetProperty ("enabled").SetValue (halo, true, null);

			
		} else {
			selected = false;
			halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
		}
	}


	IEnumerator lerpCamera(){ //lerps target (cam's target) to destination dest
		/*
		yield return new WaitForSeconds (waitTime);
		while (gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude > 0) {
			yield return null;
		}*/
		Vector3 dest = gameObject.transform.position;
		while(Vector3.Distance(target.position, dest) > threshold)
		{
			target.position = Vector3.Lerp(target.position, dest, smoothing * Time.deltaTime);
			
			yield return null;
		}
		yield return null;
	}




}
