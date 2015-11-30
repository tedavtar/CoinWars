using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class coinMerger : MonoBehaviour {

	public int state; //how many small coins selected
	public GameObject dl;
	public GameObject sl;


	public List<int> reachableFromFirstCoin;
	Vector3 firstCoinPos = Vector3.zero;
	Vector3 secondCoinPos = Vector3.zero;
	public List<int> reachableFromSecondCoin;
	public int firstCoinId = 0;
	public int secondCoinId = 0;
	public int thirdCoinId = 0;
	testGame tg;

	public GameObject largeCoin;

	public int owner;

	void Start(){
		firstCoinId = 0;
		tg = GameObject.Find ("Game").GetComponent<testGame> ();
		state = 0;
		reachableFromFirstCoin = new List<int>();

	}

	//clear lines + reset state
	public void clearLines(){
		GameObject[] dls = GameObject.FindGameObjectsWithTag("dotted");
		foreach (GameObject dl in dls) {
			Destroy(dl);
		}
		GameObject[] sls = GameObject.FindGameObjectsWithTag("solid");
		foreach (GameObject sl in sls) {
			Destroy(sl);
		}
		state = 0;
	}

	public void clearDottedLines(){
		GameObject[] dls = GameObject.FindGameObjectsWithTag("dotted");
		foreach (GameObject dl in dls) {
			Destroy(dl);
		}
	}

	public void drawDottedLine(Vector3 a, Vector3 b){
		/*
		LineRenderer lr = new LineRenderer ();
		lr.material = new Material(Shader.Find("Particles/Additive"));
		lr.material.mainTexture = dotted;
		lr.SetPosition (0, a);
		lr.SetPosition (0, b);
		*/
		GameObject dottedLine = Instantiate (dl) as GameObject;
		dottedLine.GetComponent<LineRenderer> ().SetPosition (0, a);
		dottedLine.GetComponent<LineRenderer> ().SetPosition (1, b);
	}

	public void drawSolidLine(Vector3 a, Vector3 b){
		/*
		LineRenderer lr = new LineRenderer ();
		lr.material = new Material(Shader.Find("Particles/Additive"));
		lr.material.mainTexture = dotted;
		lr.SetPosition (0, a);
		lr.SetPosition (0, b);
		*/
		GameObject solidLine = Instantiate (sl) as GameObject;
		solidLine.GetComponent<LineRenderer> ().SetPosition (0, a);
		solidLine.GetComponent<LineRenderer> ().SetPosition (1, b);
	}

	public void handleFirst(List<int> reachableIDs, int c, Vector3 cpos){
		firstCoinId = c;
		firstCoinPos = cpos;
		reachableFromFirstCoin = reachableIDs;
		foreach (int id in reachableIDs) {
			Vector3 adjpos = Vector3.zero;
			if (owner == 1){
				foreach(GameObject c1 in tg.p1coins){
					if(c1){
						if (c1.GetComponent<coinAndroidTest>().id == id){
							adjpos = c1.transform.position;
						}
					}
				}
			} else {
				foreach(GameObject c1 in tg.p2coins){
					if(c1){
						if (c1.GetComponent<coinAndroidTest>().id == id){
							adjpos = c1.transform.position;
						}
					}
				}
			}
			//Debug.Log(adjpos);
			drawDottedLine(cpos,adjpos); 
		}
		state = 1;
	}

	public void handleSecond(List<int> reachableIDs, int c, Vector3 cpos){
		secondCoinId = c;
		secondCoinPos = cpos;
		reachableFromSecondCoin = reachableIDs;

		//first flush out all the previous dotted lines as result of first selection
		GameObject[] dls = GameObject.FindGameObjectsWithTag("dotted");
		foreach (GameObject dl in dls) {
			Destroy(dl);
		}

		//restore a solid connection between first and this second coin
		drawSolidLine(firstCoinPos,cpos);

		foreach (int id in reachableIDs) {
			if (id == firstCoinId){continue;}//because you already drew solid line, no need to add dotted on top of that
			Vector3 adjpos = Vector3.zero;
			if (owner == 1){
				foreach(GameObject c1 in tg.p1coins){
					if(c1){
						if (c1.GetComponent<coinAndroidTest>().id == id){
							adjpos = c1.transform.position;
						}
					}
				}
			} else {
				foreach(GameObject c1 in tg.p2coins){
					if(c1){
						if (c1.GetComponent<coinAndroidTest>().id == id){
							adjpos = c1.transform.position;
						}
					}
				}
			}
			drawDottedLine(cpos,adjpos);
		}
		state = 2;
	}

	public void handleThirdSelection(int c, Vector3 cpos){
		thirdCoinId = c;
		clearDottedLines();
		drawSolidLine(secondCoinPos,cpos);
		drawDottedLine(cpos,firstCoinPos);

	}

	public void merge(){

		if (!(firstCoinId > 0 && secondCoinId > 0 && thirdCoinId > 0)) {
			clearLines();//added
			return;}//need to make sure 3 coins selected (as in they're not trying to cheat by doing a third unreachable coin

		if (firstCoinId==secondCoinId||firstCoinId==thirdCoinId||secondCoinId==thirdCoinId) {
			clearLines();//just now: prevent coin from cheat-merging with itself
			return;}

		GameObject c1 = tg.findCoin (firstCoinId);
		GameObject c2 = tg.findCoin (secondCoinId);
		GameObject c3 = tg.findCoin (thirdCoinId);

		Vector3 v1 = c1.transform.position;
		Vector3 v2 = c2.transform.position;
		Vector3 v3 = c3.transform.position;

		Vector3 centroid = v1 + v2 + v3;
		centroid = 1.0f/3.0f * centroid;

		GameObject l = (GameObject)Instantiate (largeCoin, centroid + .4f*Vector3.up, c1.transform.rotation);
		if (l.tag == "p1") {
			GameObject.Find ("Game").GetComponent<testGame> ().addCoinToP1(l);
		} else {
			GameObject.Find ("Game").GetComponent<testGame> ().addCoinToP2(l);
		}

		//coinAndroidTest linfo = l.GetComponent<coinAndroidTest>();
		//linfo.locked = true;
		//linfo.toggleLocked(true);

		float h1 = c1.GetComponentInChildren<Image>().fillAmount;
		float h2 = c2.GetComponentInChildren<Image>().fillAmount;
		float h3 =c3.GetComponentInChildren<Image>().fillAmount;
		float health = 1.0f / 3.0f * (h1 + h2 + h3);
		l.GetComponentInChildren<Image>().fillAmount = health;

		Destroy(c1);
		Destroy(c2);
		Destroy(c3);


		//perform reset
		clearLines();
	}

}
