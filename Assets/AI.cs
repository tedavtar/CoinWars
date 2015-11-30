using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI: MonoBehaviour{

	public bool turnInProgress;
	public GameObject p1smallcoin;
	public GameObject p2smallcoin;
	// Use this for initialization
	void Start () {
		turnInProgress = false;
	}
	

	public void executeTurn() {
		turnInProgress = true;
		//StartCoroutine ("SetUpSimulation");
		//for now just reflex agents in this block!
		StartCoroutine ("MoveCoins");




		//
	}
	IEnumerator MoveCoins(){
		yield return new WaitForSeconds(1.0f);
		foreach(GameObject p2c in runHumanVsAIGame.p2coins)
		{
			if (p2c == null){
				continue;
			}
			float sqrdist = 100000;
			GameObject closestHumanCoin = null;
			foreach(GameObject p1c in runHumanVsAIGame.p1coins)
			{
				if (p1c == null){
					continue;
				}
				Vector3 testdisp = p2c.transform.position - p1c.transform.position;
				float testdist = Vector3.SqrMagnitude(testdisp);
				if (testdist < sqrdist){
					sqrdist = testdist;
					closestHumanCoin = p1c;
				}
			}
			//closest coin hit
			if (closestHumanCoin == null){
				yield return null;
			} else {
				coinProps info = p2c.GetComponent<coinProps> ();
				info.toggleLocked (true);
				info.locked = true;

				Vector3 dir = closestHumanCoin.transform.position - p2c.transform.position;
				float forceFactor = p2c.GetComponent<coinProps> ().forceFactor;
				Vector3 forceToAdd = dir.normalized * .85f * forceFactor;// why not .8 for now
				//Debug.Log(coin.name + forceToAdd);
				p2c.GetComponent<Rigidbody>().AddForceAtPosition (forceToAdd, p2c.transform.position);
				yield return new WaitForSeconds(1.0f);
			}
			
			
		}
		turnInProgress = false;
		yield return null;
	}

	IEnumerator SetUpSimulation(){
		//first need to wait until p1 coins come to standstill
		bool canProceed = false;


		while (!canProceed) {
			bool nowCanProceed = true;
			foreach(GameObject c in GameObject.FindGameObjectsWithTag("p1"))
			{
				if(c){
					if (c.GetComponent<Rigidbody>().velocity.sqrMagnitude > .005){
						nowCanProceed = false;
					}
				}
			}
			if (nowCanProceed){
				canProceed = true;
			}
			yield return null;
		}

		//now simulate
		foreach(GameObject c in GameObject.FindGameObjectsWithTag("p1"))
		{
			if(c){
				Vector3 shiftedPos = c.transform.position;
				shiftedPos.z += 20;
				GameObject cCopy = Instantiate(p1smallcoin,shiftedPos,c.GetComponent<Rigidbody>().rotation) as GameObject; 
			}
		}
		foreach(GameObject c in GameObject.FindGameObjectsWithTag("p2"))
		{
			if(c){
				Vector3 shiftedPos = c.transform.position;
				shiftedPos.z += 20;
				GameObject cCopy = Instantiate(p2smallcoin,shiftedPos,c.GetComponent<Rigidbody>().rotation) as GameObject; 
			}
		}
		yield return null;
	}



}
