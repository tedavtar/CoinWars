using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class coinAndroidTest : MonoBehaviour {
	
	public GameObject p1coin;
	public GameObject p2coin;
	float spawnRadius = 2.0f;
	
	public string type;
	public float forceFactor;
	public float baseSmall = 4700; //4700
	public float largeToSmall = 1.5f;
	//public float mobility = .9f;
	public float extraBoostForMed = 1.3f;//1.3
	
	public int id;
	
	public int owner;
	public bool locked; //means this coin has been launched, can't launch for rest of turn
	
	Vector3 myVel;
	Rigidbody myR;
	Renderer myRen;
	
	//items
	public bool dynamite;
	public bool fireball;
	public float fireFactor;
	public float dynamiteFactor; //so .5 = halve the hit points of coin you hit
	
	public float stationaryThreshold = 3;

	public float velHitThresh;
	float refVel = 25.0f;//20
	public float maxDamage = .1f;//so about 1/10 damage (relative to initial) can be taken at worst
	float damageFactor = 1.0f;
	
	public Color firecolor;
	public Color regularcolor;
	public Color dynamitecolor;

	public Color p1unlockedColor;
	public Color p2unlockedColor;
	public Color p2lockedColor;
	public Color p1lockedColor;
	public float colorChangeTime;
	
	
	// Use this for initialization
	void Awake () {
		//id = GameObject.Find ("Game").GetComponent<webGame> ().coinIDcount; //change these two lines
		//GameObject.Find ("Game").GetComponent<webGame> ().coinIDcount = id + 1;//and this one when web to android build
		colorChangeTime = 0.7f;



		extraBoostForMed = 1.13f;//1.13 PREV



		baseSmall = 4700; //4700 PREV
		myR = GetComponent<Rigidbody> ();

		switch (type) {
		case "small":
			gameObject.GetComponent<Rigidbody>().mass = 5;
			forceFactor = baseSmall;
			break;
		case "med":
			gameObject.GetComponent<Rigidbody>().mass = 5 * largeToSmall;// * mobility;
			forceFactor = baseSmall * largeToSmall * extraBoostForMed;
			damageFactor = 1.0f * extraBoostForMed + 1.0f;



			myR.mass = 8.2f; //PREV 5



			break;
		}
		dynamite = false;
		fireball = false;
		fireFactor = 4;
		dynamiteFactor = .7f; //.5 prev
		locked = false;// false; //and back to false from true

		myRen = GetComponent<Renderer> ();
		//regularcolor = gameObject.transform.GetComponentInChildren<Image> ().color;
		//regularcolor = new Color(122.0f / 255.0f, 111.0f / 255.0f, 204.0f / 255.0f);
		regularcolor = new Color(0.0f / 255.0f, 0.0f / 0.0f, 255.0f / 255.0f);
		firecolor = new Color (1.0f, .6f, 0.0f);
		dynamitecolor = new Color (174.0f / 255.0f, 28.0f / 255.0f, 152.0f / 255.0f);

		p1unlockedColor = new Color (231.0f / 255.0f, 207.0f / 255.0f, 15.0f / 255.0f);
		p2unlockedColor = new Color (244.0f / 255.0f, 0.0f, 0.0f);
		p1lockedColor = new Color (100.0f/255.0f,100.0f/255.0f,0.0f/255.0f);
		p2lockedColor = new Color (100.0f/255.0f,0.0f,0.0f);
	}
	
	void Update(){
		myVel = myR.velocity;
		
	}

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

	void OnCollisionEnter(Collision c) {
		
		//Debug.Log("my speed: " + myVel.magnitude + " my owner: " + owner + "who collided: " + c.gameObject.name);
		if (myVel.magnitude < stationaryThreshold) { // so if we're not really moving fast enough, then we're being hit no can't use our fireball
			
			return;
		}
		if ((c.rigidbody) && (fireball)) {
			fireball = false;//used up fireball
			gameObject.transform.GetComponentInChildren<Image>().color = regularcolor;

			c.rigidbody.AddForce (myVel * fireFactor,ForceMode.Impulse);
			
		}

		if ((c.rigidbody) && (dynamite)) {
			dynamite = false;//used up
			gameObject.transform.GetComponentInChildren<Image>().color = regularcolor;
			
			c.gameObject.transform.GetComponentInChildren<Image>().fillAmount *= dynamiteFactor;
			
			if (c.gameObject.transform.GetComponentInChildren<Image>().fillAmount <=0){
				Destroy(c.gameObject);//fatality
			}
			
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
	
	void OnTriggerEnter(Collider other) {
		GameObject powerUp = other.gameObject;
		if (powerUp.tag == "fireball") {
			powerUp.SetActive(false);
			dynamite = false;
			fireball = true;
			gameObject.transform.GetComponentInChildren<Image>().color = firecolor;
			//Debug.Log("got you!");
		}
		if (powerUp.tag == "dynamite") {
			powerUp.SetActive(false);
			fireball = false;
			dynamite = true;
			gameObject.transform.GetComponentInChildren<Image>().color = dynamitecolor;
		}
		if (powerUp.tag == "mirror") {
			if (owner == 1){
				powerUp.SetActive(false);
				//Debug.Log(myR.velocity);

				GameObject p1clone = (GameObject)Instantiate(p1coin,gameObject.transform.position - spawnRadius*myR.velocity.normalized,myR.rotation);
				GameObject.Find ("Game").GetComponent<testGame> ().addCoinToP1(p1clone);
				p1clone.GetComponent<Rigidbody>().velocity = myR.velocity;
				coinAndroidTest p1cloneinfo = p1clone.GetComponent<coinAndroidTest>();
				p1cloneinfo.locked = true;//locked
				//give the clone my state
				if (dynamite){
					p1cloneinfo.dynamite = true;
					p1cloneinfo.transform.GetComponentInChildren<Image>().color = dynamitecolor;
				}
				if (fireball){
					p1cloneinfo.fireball = true;
					p1cloneinfo.transform.GetComponentInChildren<Image>().color = firecolor;
				}
				//Debug.Log("p1 (yellow) hit me");
			} else {
				powerUp.SetActive(false);
				//Debug.Log(myR.velocity);
				
				GameObject p2clone = (GameObject)Instantiate(p2coin,gameObject.transform.position - spawnRadius*myR.velocity.normalized,myR.rotation);
				GameObject.Find ("Game").GetComponent<testGame> ().addCoinToP2(p2clone);
				p2clone.GetComponent<Rigidbody>().velocity = myR.velocity;
				coinAndroidTest p2cloneinfo = p2clone.GetComponent<coinAndroidTest>();
				//p2cloneinfo.locked = true;//locked

				//give the clone my state
				p2cloneinfo.locked = locked;
				if (locked){
					p2cloneinfo.toggleLocked(true);
				}
				if (dynamite){
					p2cloneinfo.dynamite = true;
					p2cloneinfo.transform.GetComponentInChildren<Image>().color = dynamitecolor;
				}
				if (fireball){
					p2cloneinfo.fireball = true;
					p2cloneinfo.transform.GetComponentInChildren<Image>().color = firecolor;
				}
				//Debug.Log("p2 (red) hit me");
			}
		}
	}
	
}
