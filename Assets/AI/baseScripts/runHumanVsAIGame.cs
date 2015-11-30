using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class runHumanVsAIGame : MonoBehaviour {

	public int turn;

	public static List<GameObject> p1coins = new List<GameObject>();
	public static List<GameObject> p2coins = new List<GameObject>();

	Vector2 touchPos1 = Vector2.zero;
	Vector2 touchPos2 = Vector2.zero;

	public int clicks;


	Vector3 clickPos;

	public LineRenderer lr;
	public GameObject lrgo;

	public LayerMask lm1;//mask everything out but 1 (i think)
	public LayerMask lm2;

	public float forceFactor = 1.0f;

	AI ai;

	IEnumerator drawLine(Vector3 start, Vector3 end, float dur){
		lr.enabled = true;
		lr.SetPosition (0, start);
		lr.SetPosition (1, end);
		yield return new WaitForSeconds (dur);
		lr.enabled = false;
		yield return null;
	}

	void Awake () {
		lr = lrgo.GetComponent<LineRenderer> ();
		ai = gameObject.GetComponent<AI> ();
	}

	void Start(){
		clicks = 0;
		turn = 1;
		int coinId = 0;
		foreach(GameObject c in GameObject.FindGameObjectsWithTag("p1"))
		{
			coinId += 1;
			c.GetComponent<coinProps>().id = coinId;
			p1coins.Add(c);
			
		}
		foreach(GameObject c in GameObject.FindGameObjectsWithTag("p2"))
		{
			coinId += 1;
			c.GetComponent<coinProps>().id = coinId;
			p2coins.Add(c);
			c.GetComponent<coinProps>().toggleLocked(true);

		}
	}

	bool checkToReloadScene(){
		bool reloadLvl = true;
		foreach(GameObject c in p1coins){
			if(c){
				reloadLvl = false;
			}
		}
		//so this means that p1 out of coins so yeah, can reload level
		if (reloadLvl) {
			return reloadLvl;
		}
		//so p1 has some coins...but can still reload if p2 out of coins--basically just a repeat
		reloadLvl = true;
		foreach(GameObject c in p2coins){
			if(c){
				reloadLvl = false;
			}
		}
		return reloadLvl;
	}
	
	// Update is called once per frame
	void Update () {
		if (checkToReloadScene ()) {
			Application.LoadLevel(Application.loadedLevel);
		}
		handleTurn ();
		if (turn == 1) {
			if (Input.GetMouseButtonDown (0)) {
				CastRayToWorld (Input.mousePosition);
			}
		} else {
			if (!ai.turnInProgress){
				ai.executeTurn();
			}
		}
	}

	void handleTurn(){
		bool canChangeTurn = true;//so start off saying yes can change turn from px but then go through all of px's coins and if it is the case that at least one remains unlocked, then nope, can't switch
		if (turn == 1) {
			foreach(GameObject c in p1coins){
				if(c){
					coinProps info = c.GetComponent<coinProps>();
					//Debug.Log(info.locked);
					if (!info.locked){
						
						canChangeTurn = false;
					}
				}
			}
		} else {
			foreach(GameObject c in p2coins){
				if(c){
					coinProps info = c.GetComponent<coinProps>();
					if (!info.locked){
						canChangeTurn = false;
					}
				}
			}
		}
		
		if (canChangeTurn) {
			
			if (turn == 1){
				
				foreach(GameObject c in p2coins){
					if(c){
						coinProps info = c.GetComponent<coinProps>();
						info.toggleLocked(false);
						info.locked = false;
					}
				}
				turn = 2;

				
			} else {
				
				foreach(GameObject c in p1coins){
					if(c){
						coinProps info = c.GetComponent<coinProps>();
						info.toggleLocked(false);
						info.locked = false;
					}
				}
				turn = 1;

			}
		}
	}









	public static void deselectAllCoins(){
		foreach(GameObject c in GameObject.FindGameObjectsWithTag("p1"))
		{
			c.GetComponent<focusSimple>().deselect();	
		}
		foreach(GameObject c in GameObject.FindGameObjectsWithTag("p2"))
		{
			c.GetComponent<focusSimple>().deselect();
		}
	}


	//s


	void CastRayToWorld(Vector2 pos) {
		//Debug.Log ("initially: " + clicks);

			Ray r = Camera.main.ScreenPointToRay (pos);
			//Vector3 p = r.origin + r.direction*(10 - (-.17f));
			Vector3 p = Vector3.zero;
			RaycastHit test;
			LayerMask lm; //our layermask for raycasting to ignore coins not in turn
			lm = lm2;
			lm = ~lm;
		
			bool hitCoin = false;
			GameObject coinHit;
	
			if (Physics.Raycast (r.origin, r.direction, out test, Mathf.Infinity, lm)) {


				p = test.point;
			
				GameObject gotHit = test.collider.gameObject;

			
				if (!gotHit.name.StartsWith ("Terrain")) {
				
					if (clicks == 0) {
						focusSimple coinF = gotHit.GetComponent<focusSimple> ();
						if (!coinF) {
							return;
						}
						coinF.handleSelection ();
						return;
					}

					Vector3 adjust = test.normal;
					adjust.Normalize ();
			
					p += -.1f * adjust;

				} else {

					deselectAllCoins ();
				
					Vector3 adjust = test.normal;
					adjust.Normalize ();
				
					p += .1f * adjust;
							
				}
			
			}
		
		
			if (clicks == 0) {
				clickPos = p;
				clicks += 1;
				//Debug.Log("h " + clicks);//added
			} else {
			
				clicks = 0;
				Vector3 dir = p - clickPos;
			
				//k so here rather than debug dray ray do a line renderer so viewer can see in scene
			
				StartCoroutine (drawLine (clickPos, p, 1.0f));
			
				Debug.DrawRay (clickPos, dir, Color.blue, 5.0f);
				RaycastHit hit;
				float dist = dir.magnitude;
				if (Physics.Raycast (clickPos, dir, out hit, dist, lm)) {
					GameObject target = hit.collider.gameObject;
				
				
					coinProps info = target.GetComponent<coinProps> ();
					//if (info.owner != turn){return;} // so exit if WAIT NO use raycast layber bitmask and here just check for locked/set it if false to true and increment a var to decide if turn over
					if (!info) {
						return;
					}
					if (info.locked) {//so this coin is locked meaning have already launched it
						return;
					} 
					info.toggleLocked (true);
					info.locked = true; //so we're going to launch this so need to lock it so can't double launch in same turn
				
				
					Rigidbody coin = target.GetComponent<Rigidbody> ();
				
					float distToHit = Vector3.Distance (hit.point, clickPos);
				
					float ratio = distToHit / dist;
					//Debug.Log("N/D: " + ratio);
				
				
					Vector3 offset = hit.point - coin.position;
				
					float offsetsize = offset.magnitude;
					//Debug.Log(offsetsize); offset = useless for coins--always = .5. For pencilwars (original) it had meaning...
				
					forceFactor = target.GetComponent<coinProps> ().forceFactor;
				
					Vector3 forceToAdd = dir.normalized * ratio * forceFactor;// * Mathf.Pow (offsetsize,spower);
					//Debug.Log(coin.name + forceToAdd);
					coin.AddForceAtPosition (forceToAdd, hit.point);
				
				}
			
			}
		
			//Debug.Log ("at the end: " + clicks);
		
	}
	

	
	//e



}
