using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState{
	public List<Coin> p1coins = new List<Coin>();
	public List<Coin> p2coins = new List<Coin>();

	public GameState(List<Coin> p1c, List<Coin> p2c){
		p1coins = p1c;
		p2coins = p2c;
	}
	public void print(){
		Debug.Log ("P1 coins:");
		foreach (Coin c in p1coins) {
			Debug.Log("Coin: " + c.id + " at position: " + c.pos);
		}
		Debug.Log ("P2 coins:");
		foreach (Coin c in p2coins) {
			Debug.Log("Coin: " + c.id + " at position: " + c.pos);
		}
	}
	//finds coin with coinID and updates it's pos to newPos
	public void update(float coinID, Vector3 newPos){
		foreach (Coin c in p1coins) {
			if (c.id == coinID) {
				c.pos = newPos;
			}
		}

		foreach (Coin c in p2coins) {
			if (c.id == coinID) {
				c.pos = newPos;
			}
		}
	}
}
