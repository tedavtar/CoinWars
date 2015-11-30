using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class coinProps : MonoBehaviour {

	public int id;
	public bool locked;
	public int owner;


	public Color p1unlockedColor;
	public Color p2unlockedColor;
	public Color p2lockedColor;
	public Color p1lockedColor;
	public float colorChangeTime;
	Renderer myRen;

	public float forceFactor;
	public float baseSmall = 4700; //4700
	public float largeToSmall = 1.5f;
	//public float mobility = .9f;
	public float extraBoostForMed = 1.3f;//1.3

	Vector3 myVel;
	Rigidbody myR;
	public float stationaryThreshold = 3;
	float refVel = 25.0f;
	public float maxDamage = .1f;//so about 1/10 damage (relative to initial) can be taken at worst
	float damageFactor = 1.0f;


	IEnumerator colorLerp(Color fr, Color to, float duration)
	{
		float t = 0;
		while(t < 1) {
			t += Time.smoothDeltaTime / duration;
			myRen.material.color = Color.Lerp (fr, to, t);
			yield return null;
		}
	}


	public void toggleLocked(bool lockme){//later migrate all logic into this--for now just change color
		if (lockme) {
			if (owner == 1){
				//Debug.Log("locking p1");
				StartCoroutine(colorLerp(p1unlockedColor,p1lockedColor,colorChangeTime));
				//myRen.material.color = p1lockedColor;
			} else {
				StartCoroutine(colorLerp(p2unlockedColor,p2lockedColor,colorChangeTime));
				myRen.material.color = p2lockedColor;
			}
		} else {
			if (owner == 1){
				StartCoroutine(colorLerp(p1lockedColor,p1unlockedColor,colorChangeTime));
				//myRen.material.color = p1unlockedColor;
			} else {
				StartCoroutine(colorLerp(p2lockedColor,p2unlockedColor,colorChangeTime));
				//myRen.material.color = p2unlockedColor;
			}
		}
	}


	void Awake(){
		myRen = GetComponent<Renderer> ();
		p1unlockedColor = new Color (231.0f / 255.0f, 207.0f / 255.0f, 15.0f / 255.0f);
		p2unlockedColor = new Color (244.0f / 255.0f, 0.0f, 0.0f);
		p1lockedColor = new Color (100.0f/255.0f,100.0f/255.0f,0.0f/255.0f);
		p2lockedColor = new Color (100.0f/255.0f,0.0f,0.0f);

		colorChangeTime = 0.7f;
		extraBoostForMed = 1.13f;//1.1 PREV
		baseSmall = 4700; //4700 PREV

		gameObject.GetComponent<Rigidbody>().mass = 5;
		forceFactor = baseSmall;

		locked = false;

		myR = GetComponent<Rigidbody> ();
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		myVel = myR.velocity;
	}

	void OnCollisionEnter(Collision c) {
		if (myVel.magnitude < stationaryThreshold) { // so if we're not really moving fast enough, then we're being hit no can't use our fireball
			return;
		}
		//now test to decrease my points //note fireball won't affect this as i use attacking coins velocity at moment of impact--the fireball actually doesn't change this (just adds impulse to other coin)
		if ((c.rigidbody)  && (myVel.magnitude > stationaryThreshold)){
			//Debug.Log (myVel.magnitude); //ok so now need to use this value (inflicting/attacking coins velocity) to damage opponent
			//c.gameObject.transform.GetChild(0).GetComponent<Image>().fillAmount -= .1f;
			float damage;
			damage = myVel.magnitude/refVel * maxDamage;//so hitting with velocity >= refVel means you inflict max Damage--else inflict proportionate fraction
			damage = Mathf.Clamp(damage, 0, maxDamage);
			c.gameObject.transform.GetComponentInChildren<Image>().fillAmount -= damage * damageFactor;
			
			if (c.gameObject.transform.GetComponentInChildren<Image>().fillAmount <=0){
				Destroy(c.gameObject);//fatality
			}
		}
	
	}


}
