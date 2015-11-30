using UnityEngine;
using System.Collections;

public class falldetect : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		GameObject died = other.gameObject;
		//Destroy(died);
		//GameObject.Find ("Game").GetComponent<webGame> ().photonView.RPC ("destroyCoin", PhotonTargets.All,died.GetComponent<coin>().id);
	}
}
