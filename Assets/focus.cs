using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class focus : MonoBehaviour {
	Transform target;
	float smoothing = 3.0f;
	float threshold = 0.5f;

	float waitTime = 0.1f;

	float smallHalo = 1.0f;
	float largeHalo = 2.0f;

	Component halo;

	bool selected;

	public float mergeRadius = 4.0f;

	public GameObject smallcoin;

	Vector3 split1 = new Vector3(0.0f,0.0f,1.0f);
	Vector3 split2 = new Vector3(Mathf.Cos(Mathf.PI/6),0.0f,-1*Mathf.Sin(Mathf.PI/6));
	Vector3 split3 = new Vector3(-1*Mathf.Cos(Mathf.PI/6),0.0f,-1*Mathf.Sin(Mathf.PI/6));

	coinMerger cm1;
	coinMerger cm2;
	//Viewer view;
	// Use this for initialization
	void Start () {
		mergeRadius = 5.0f;//4
		cm1 = GameObject.Find ("P1Merger").GetComponent<coinMerger> ();
		cm2 = GameObject.Find ("P2Merger").GetComponent<coinMerger> ();
		smallHalo = 1.0f;
		largeHalo = 1.4f;
		halo = GetComponent("Halo");
		halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
		selected = false;
		//view = GameObject.Find ("Main Camera").GetComponent<Viewer> ();
		target = GameObject.Find ("Main Camera").GetComponent<Viewer> ().target;
	}
		
	public void deselect(){
		selected = false;
		halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.D) && selected && gameObject.GetComponent<coinAndroidTest> ().type == "med") {
			multiply();		
		}
		if (Input.GetKeyDown (KeyCode.D) && selected && gameObject.GetComponent<coinAndroidTest> ().type == "small") {
			mergeAid();		
		}
		if (Input.GetKeyDown (KeyCode.S) && selected) {
			if (gameObject.GetComponent<coinAndroidTest> ().owner == 1){
				pipeTeleportAidP1();
			} else {
				pipeTeleportAidP2();
			}
		}
	}
	//void OnMouseDown(){
	public void handleSelection(){
		StopAllCoroutines ();
		//pan the camera to this coin
		StartCoroutine ("lerpCamera");
		if (gameObject.GetComponent<coinAndroidTest> ().locked) {
			return;
		}
		if (!selected) {

			//deselect all other coins 
			if (gameObject.tag == "p1"){
				//Debug.Log("called");
				foreach(GameObject c in GameObject.FindGameObjectsWithTag("p1"))
				{
					//Debug.Log('i');
					c.GetComponent<focus>().deselect();

				}
			} else {
				foreach(GameObject c in GameObject.FindGameObjectsWithTag("p2"))
				{
					c.GetComponent<focus>().deselect();
				}
			}

			selected = true;
			//multiply();

			halo.GetType ().GetProperty ("enabled").SetValue (halo, true, null);

			//now if small coin (well know it's unlocked) and 2 small coins already selected(in context of merging), triangle it
			if ((gameObject.GetComponent<coinAndroidTest>().type == "small")){
				if (gameObject.tag == "p1"){
					if(GameObject.Find("P1Merger").GetComponent<coinMerger>().state == 2){
						bool terminate = true;
						foreach(int i in cm1.reachableFromSecondCoin){
							if (i == gameObject.GetComponent<coinAndroidTest>().id){terminate = false;}
						}
						if (terminate) { 
							cm1.clearLines();//added
							return;}
						GameObject.Find("P1Merger").GetComponent<coinMerger>().handleThirdSelection(gameObject.GetComponent<coinAndroidTest>().id,gameObject.transform.position);
					}
				} else {
					if(GameObject.Find("P2Merger").GetComponent<coinMerger>().state == 2){
						bool terminate = true;
						foreach(int i in cm2.reachableFromSecondCoin){
							if (i == gameObject.GetComponent<coinAndroidTest>().id){terminate = false;}
						}
						if (terminate) { 
							cm2.clearLines();//added
							return;}
						GameObject.Find("P2Merger").GetComponent<coinMerger>().handleThirdSelection(gameObject.GetComponent<coinAndroidTest>().id,gameObject.transform.position);
					}
				}
			}

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

	//for large coins only, swap for 3 small coins.
	void multiply(){

		GameObject s1 = (GameObject)Instantiate(smallcoin,gameObject.transform.position + .8f*(split1 + .5f*Vector3.up),gameObject.transform.rotation);
		if (gameObject.tag == "p1") {
			GameObject.Find ("Game").GetComponent<testGame> ().addCoinToP1(s1);
		} else {
			GameObject.Find ("Game").GetComponent<testGame> ().addCoinToP2(s1);
		}
		coinAndroidTest s1info = s1.GetComponent<coinAndroidTest>();
		s1info.locked = true;
		s1info.toggleLocked(true);

		GameObject s2 = (GameObject)Instantiate(smallcoin,gameObject.transform.position + .8f*(split2 + .5f*Vector3.up),gameObject.transform.rotation);
		if (gameObject.tag == "p1") {
			GameObject.Find ("Game").GetComponent<testGame> ().addCoinToP1(s2);
		} else {
			GameObject.Find ("Game").GetComponent<testGame> ().addCoinToP2(s2);
		}
		coinAndroidTest s2info = s2.GetComponent<coinAndroidTest>();
		s2info.locked = true;
		s2info.toggleLocked(true);

		GameObject s3 = (GameObject)Instantiate(smallcoin,gameObject.transform.position + .8f*(split3 + .5f*Vector3.up),gameObject.transform.rotation);
		if (gameObject.tag == "p1") {
			GameObject.Find ("Game").GetComponent<testGame> ().addCoinToP1(s3);
		} else {
			GameObject.Find ("Game").GetComponent<testGame> ().addCoinToP2(s3);
		}
		coinAndroidTest s3info = s3.GetComponent<coinAndroidTest>();
		s3info.locked = true;
		s3info.toggleLocked(true);

		if (gameObject.GetComponent<coinAndroidTest>().fireball){
			s1info.fireball = true;
			s1.transform.GetComponentInChildren<Image>().color = new Color (1.0f, .6f, 0.0f);

			s2info.fireball = true;
			s2.transform.GetComponentInChildren<Image>().color = new Color (1.0f, .6f, 0.0f);

			s3info.fireball = true;
			s3.transform.GetComponentInChildren<Image>().color = new Color (1.0f, .6f, 0.0f);
		}

		if (gameObject.GetComponent<coinAndroidTest>().dynamite){
			s1info.dynamite = true;
			s1.transform.GetComponentInChildren<Image>().color = new Color (174.0f / 255.0f, 28.0f / 255.0f, 152.0f / 255.0f);
			
			s2info.dynamite = true;
			s2.transform.GetComponentInChildren<Image>().color = new Color (174.0f / 255.0f, 28.0f / 255.0f, 152.0f / 255.0f);
			
			s3info.dynamite = true;
			s3.transform.GetComponentInChildren<Image>().color = new Color (174.0f / 255.0f, 28.0f / 255.0f, 152.0f / 255.0f);
		}

		float health = gameObject.GetComponentInChildren<Image>().fillAmount;
		s1.GetComponentInChildren<Image>().fillAmount = health;
		s2.GetComponentInChildren<Image>().fillAmount = health;
		s3.GetComponentInChildren<Image>().fillAmount = health;

		Destroy(gameObject);

	}

	//to try to merge this coin
	void mergeAid() {
		Collider[] stuffHit = Physics.OverlapSphere(transform.position, mergeRadius);
		if (gameObject.tag == "p1") {
			if ((cm1.state == 0)||(cm1.state == 1)){

				List<int> adjCoinIds = new List<int>();

				foreach (Collider c in stuffHit) {
					GameObject adjacentCoin = c.gameObject;
					coinAndroidTest adjCoinInfo = adjacentCoin.GetComponent<coinAndroidTest>();
					if ((adjacentCoin.tag == "p1")){
						if (!(adjCoinInfo.id == gameObject.GetComponent<coinAndroidTest>().id) && (adjCoinInfo.type == "small")) {
							if(!(adjCoinInfo.locked)){
								adjCoinIds.Add(adjCoinInfo.id);
							}
						}
					}
				}

				if (cm1.state == 0) {
					cm1.handleFirst(adjCoinIds,gameObject.GetComponent<coinAndroidTest>().id,transform.position);
				} else {//so state=1, on second coin. Need to first check to make sure that this coin is actually reachable from 1st coin
					bool terminate = true;
					foreach(int i in cm1.reachableFromFirstCoin){
						if (i == gameObject.GetComponent<coinAndroidTest>().id){terminate = false;}
					}
					if (terminate) { 
						cm1.clearLines();//new
						return;}
					cm1.handleSecond(adjCoinIds,gameObject.GetComponent<coinAndroidTest>().id,transform.position);
				}

			} else {//do the combining!
				//for now just place on centroid, highest height + the .5, and average the healths of the 3 constituent coins
				cm1.merge();
			}

		} else {
			if ((cm2.state == 0)||(cm2.state == 1)){
				
				List<int> adjCoinIds = new List<int>();
				
				foreach (Collider c in stuffHit) {
					GameObject adjacentCoin = c.gameObject;
					coinAndroidTest adjCoinInfo = adjacentCoin.GetComponent<coinAndroidTest>();
					if ((adjacentCoin.tag == "p2")){
						if (!(adjCoinInfo.id == gameObject.GetComponent<coinAndroidTest>().id) && (adjCoinInfo.type == "small")) {
							if(!(adjCoinInfo.locked)){
								adjCoinIds.Add(adjCoinInfo.id);
							}
						}
					}
				}
				
				if (cm2.state == 0) {
					//foreach (int i in adjCoinIds){Debug.Log(i);}
					cm2.handleFirst(adjCoinIds,gameObject.GetComponent<coinAndroidTest>().id,transform.position);
				} else {//so state=1, on second coin. Need to first check to make sure that this coin is actually reachable from 1st coin
					bool terminate = true;
					foreach(int i in cm2.reachableFromFirstCoin){
						if (i == gameObject.GetComponent<coinAndroidTest>().id){terminate = false;}
					}
					if (terminate) { 
						cm2.clearLines();//new
						return;}
					cm2.handleSecond(adjCoinIds,gameObject.GetComponent<coinAndroidTest>().id,transform.position);
				}
				
			} else {//do the combining!
				//for now just place on centroid, highest height + the .5, and average the healths of the 3 constituent coins
				//ah bug here-need to make sure that there are 3 coins selected--just stick this in the merge

				cm2.merge();
			}



		}

	}
	
	

	void pipeTeleportAidP1(){
		storePipes.clearTeleportation ();
		//first is this coin touching any pipe segments?
		bool touching = false;

		List<SnapGrid> onTopOf = new List<SnapGrid>();
		foreach (SnapGrid sg in storePipes.sgs) {
			foreach(Collider c in sg.colliderList){
				if (c!=null){
				if (c.gameObject.GetComponent<coinAndroidTest>()!=null){
					if (c.gameObject.GetComponent<coinAndroidTest>().id == gameObject.GetComponent<coinAndroidTest>().id){
						touching = true;
						onTopOf.Add(sg);
					}
				}
				}// else {Debug.Log("fixed merge then sg missingreference in it's col List");}
			}
		}

		//quit if coin is not touching any pipe segments
		if (!touching) {

			return;
		}

		storePipes.signalTeleportation (gameObject);
		foreach (SnapGrid sg in onTopOf) {
			//sg.gameObject.GetComponent<Renderer> ().enabled = false;
			storePipes.selectReachablePipeSegs(sg);
		}


	}




	void pipeTeleportAidP2(){
		storePipes.clearTeleportationP2 ();
		//first is this coin touching any pipe segments?
		bool touching = false;
		
		List<SnapGrid> onTopOf = new List<SnapGrid>();
		foreach (SnapGrid sg in storePipes.sgsP2) {
			foreach(Collider c in sg.colliderList){
				if (c!=null){
					if (c.gameObject.GetComponent<coinAndroidTest>()!=null){
						if (c.gameObject.GetComponent<coinAndroidTest>().id == gameObject.GetComponent<coinAndroidTest>().id){
							touching = true;
							onTopOf.Add(sg);
						}
					}
				}// else {Debug.Log("fixed merge then sg missingreference in it's col List");}
			}
		}
		
		//quit if coin is not touching any pipe segments
		if (!touching) {
			
			return;
		}
		
		storePipes.signalTeleportationP2 (gameObject);
		foreach (SnapGrid sg in onTopOf) {
			//sg.gameObject.GetComponent<Renderer> ().enabled = false;
			storePipes.selectReachablePipeSegs(sg);
		}
		
		
	}








}
