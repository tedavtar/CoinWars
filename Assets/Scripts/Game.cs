using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	Vector2 touchPos1 = Vector2.zero;
	Vector2 touchPos2 = Vector2.zero;
	float startT = 0.0f;
	float endT = 0.0f;

	public int clicks;

	public int turn; //val of 1 means p1 turn, 2 means p2 turn

	Vector3 clickPos;

	public float forceFactor = 1.0f;

	public float velocityThreshold = 0f;

	public float spower = 0.0f;



	public Vector3 initialPosP2 = new Vector3(1.33f,.98f,-9.79f);
	public Vector3 initialPosP1 = new Vector3(-1.33f,.98f,-9.79f);



	public List<GameObject> p1coins = new List<GameObject>();
	public List<GameObject> p2coins = new List<GameObject>();

	public LayerMask lm1;//mask everything out but 1 (i think)
	public LayerMask lm2;

	public float maxSpeed = 1500.0f;//1000

	void Start () {
		clicks = 0;
		turn = 1;

		foreach(GameObject c in GameObject.FindGameObjectsWithTag("p1"))
		{

			p1coins.Add(c);
		}
		foreach(GameObject c in GameObject.FindGameObjectsWithTag("p2"))
		{
			
			p2coins.Add(c);
		}


	}
	

	void Update () {
		handleTurn ();


		//android
		if (Input.touchCount == 1) {
		
				Touch t = Input.GetTouch(0);
				switch (t.phase){
					
				case TouchPhase.Began:
					startT = Time.time;
					touchPos1 = t.position;
					
					//disp(touchPos1.ToString());
					break;
					
					
				case TouchPhase.Ended:
					endT = Time.time;
					touchPos2 = t.position;
					//disp(touchPos1.ToString() + " " + touchPos2.ToString()); seems to be correct
					//disp(debugText.text + " " + touchPos2.ToString());
					float elapsed = endT - startT;
					//disp(debugText.text + " " + touchPos1.ToString());
					//CastRayToWorld (touchPos1);
					//CastRayToWorld (touchPos2);
					CastRayToWorldAndroid(touchPos1,touchPos2,elapsed);
					//clicks = 0;
					touchPos1 = Vector2.zero;
					touchPos2 = Vector2.zero;//?resets
					
					
					break;
					
				}
			return;
		}
			
			
		
		//must be web if reacher here
		if (Input.GetMouseButtonDown (0)) {
			CastRayToWorld (Input.mousePosition);
		}

	}

	void handleTurn(){
		bool canChangeTurn = true;//so start off saying yes can change turn from px but then go through all of px's coins and if it is the case that at least one remains unlocked, then nope, can't switch
		if (turn == 1) {
			foreach(GameObject c in p1coins){
				if(c){
					coinAndroidTest info = c.GetComponent<coinAndroidTest>();
					//Debug.Log(info.locked);
					if (!info.locked){

						canChangeTurn = false;
					}
				}
			}
		} else {
			foreach(GameObject c in p2coins){
				if(c){
					coinAndroidTest info = c.GetComponent<coinAndroidTest>();
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
						coinAndroidTest info = c.GetComponent<coinAndroidTest>();
						info.locked = false;
					}
				}
				turn = 2;

			} else {

				foreach(GameObject c in p1coins){
					if(c){
						coinAndroidTest info = c.GetComponent<coinAndroidTest>();
						info.locked = false;
					}
				}
				turn = 1;

			}
		}
	}
	/*
	void CastRayToWorldAndroidFlick(Vector3 touchPos1,Vector3 touchPos2,float elapsed){
		//k so now I'm going to do the whole flick thing for android.

		//step1 need to make sure that touchPos1--which is in screen coords gets to hit our object
		Ray r = Camera.main.ScreenPointToRay(touchPos1);
		RaycastHit test;
		LayerMask lm; //our layermask for raycasting to ignore coins not in turn
		if (turn == 1) {//so p1's turn want to mask out p2 coins. so mask out layer p2
			lm = lm2;//so lm2 is only layer p2. Then at end invert this.
		} else {
			lm = lm1;
		}
		lm = ~lm;
		if (Physics.Raycast (r.origin, r.direction, out test, Mathf.Infinity, lm)) {

			GameObject target = test.collider.gameObject;
			Debug.Log(target.name);
			
			coinAndroidTest info = target.GetComponent<coinAndroidTest>();
			//if (info.owner != turn){return;} // so exit if WAIT NO use raycast layber bitmask and here just check for locked/set it if false to true and increment a var to decide if turn over
			
			if(info.locked){//so this coin is locked meaning have already launched it
				return;
			} 
			info.locked = true; //so we're going to launch this so need to lock it so can't double launch in same turn
		}

	}*/

	void CastRayToWorldAndroid(Vector3 touchPos1,Vector3 touchPos2,float elapsed){
		/*
		Vector3 dir = touchPos2 - touchPos1;
		float dist = dir.magnitude;
		//Debug.Log(dist + " " + elapsed); most excellent*/
		Vector3 p1 = Vector3.zero;
		Vector3 p2 = Vector3.zero;
		Ray r1 = Camera.main.ScreenPointToRay(touchPos1);
		Ray r2 = Camera.main.ScreenPointToRay(touchPos2);
		RaycastHit test1;
		RaycastHit test2;
		LayerMask lm; //our layermask for raycasting to ignore coins not in turn
		if (turn == 1) {//so p1's turn want to mask out p2 coins. so mask out layer p2
			lm = lm2;//so lm2 is only layer p2. Then at end invert this.
		} else {
			lm = lm1;
		}
		lm = ~lm;
		if (Physics.Raycast (r1.origin, r1.direction, out test1, Mathf.Infinity, lm)) {
			//so need to check if hit a coin. else lower p.y else raise p.y (because if it's a coin then we're going to miss it if we incrase the ray's height..
			/*namely we know from that default unity cylinder primitive = 2 units high
			this means due to .1 scale our coins are .2 units high so .2/2 = .1, the half height of interest.
			 */
			p1 = test1.point;
			GameObject gotHit = test1.collider.gameObject;
			//Vector3 adjust = gotHit.transform.up;//so the normal of the surface that was hit
			//so need to make adjust to be terrain normal now..
			
			
			
			if (!gotHit.name.StartsWith("Terrain")){
				//p.y -= .1f;//
				//Vector3 adjust = gotHit.transform.up; //try something else b/c when coin upside down, we get 0 -1 0 rather than 0 1 0.
				Vector3 adjust = test1.normal;
				adjust.Normalize();
				
				
				//Debug.Log ("p prev: " + p);
				p1 += -.1f*adjust; //AHA! So that's why. Adjust is being negative b/c coin is flipped over.
				//Debug.Log(adjust);
				//Debug.Log ("p adjusted: " + p + " and adjustment used: " + adjust);
				//Debug.Log("On a coin");
			} else {//so need the Terrain normal - credit to Eric on unity forumns
				//p.y += .1f;
				Terrain t = gotHit.GetComponent<Terrain>();
				Vector3 relPos = p1 - t.transform.position;
				Vector2 nPos = new Vector2(Mathf.InverseLerp(0.0f, t.terrainData.size.x, relPos.x), Mathf.InverseLerp(0.0f, t.terrainData.size.z, relPos.z));
				
				Vector3 adjust = t.terrainData.GetInterpolatedNormal(nPos.x,nPos.y);
				
				//adjust = adjust.normalized;
				adjust.Normalize();
				
				p1 += .1f*adjust;
				//Debug.Log("On the floor");
				//Debug.Log(adjust);
				//Debug.Log(gotHit.transform.up); aha, so transform.up gives the normal. need to use this for adjustment so that will work also on inclined surfaces,not just flat ones w/ normal of 0 1 0			
			}
			
		}

		if (Physics.Raycast (r2.origin, r2.direction, out test2, Mathf.Infinity, lm)) {
			//so need to check if hit a coin. else lower p.y else raise p.y (because if it's a coin then we're going to miss it if we incrase the ray's height..
			/*namely we know from that default unity cylinder primitive = 2 units high
			this means due to .1 scale our coins are .2 units high so .2/2 = .1, the half height of interest.
			 */
			p2 = test2.point;
			GameObject gotHit = test2.collider.gameObject;
			//Vector3 adjust = gotHit.transform.up;//so the normal of the surface that was hit
			//so need to make adjust to be terrain normal now..
			
			
			
			if (!gotHit.name.StartsWith("Terrain")){
				//p.y -= .1f;//
				//Vector3 adjust = gotHit.transform.up; //try something else b/c when coin upside down, we get 0 -1 0 rather than 0 1 0.
				Vector3 adjust = test2.normal;
				adjust.Normalize();
				
				
				//Debug.Log ("p prev: " + p);
				p2 += -.1f*adjust; //AHA! So that's why. Adjust is being negative b/c coin is flipped over.
				//Debug.Log(adjust);
				//Debug.Log ("p adjusted: " + p + " and adjustment used: " + adjust);
				//Debug.Log("On a coin");
			} else {//so need the Terrain normal - credit to Eric on unity forumns
				//p.y += .1f;
				Terrain t = gotHit.GetComponent<Terrain>();
				Vector3 relPos = p2 - t.transform.position;
				Vector2 nPos = new Vector2(Mathf.InverseLerp(0.0f, t.terrainData.size.x, relPos.x), Mathf.InverseLerp(0.0f, t.terrainData.size.z, relPos.z));
				
				Vector3 adjust = t.terrainData.GetInterpolatedNormal(nPos.x,nPos.y);
				
				//adjust = adjust.normalized;
				adjust.Normalize();
				
				p2 += .1f*adjust;
				//Debug.Log("On the floor");
				//Debug.Log(adjust);
				//Debug.Log(gotHit.transform.up); aha, so transform.up gives the normal. need to use this for adjustment so that will work also on inclined surfaces,not just flat ones w/ normal of 0 1 0			
			}
			
		}

		//s

		Vector3 dir = p2 - p1;
		
		Debug.DrawRay(p1,dir,Color.blue,5.0f);
		RaycastHit hit;
		float dist = dir.magnitude;
		if (Physics.Raycast(p1,dir,out hit, dist,lm)){
			GameObject target = hit.collider.gameObject;
			
			
			coinAndroidTest info = target.GetComponent<coinAndroidTest>();
			//if (info.owner != turn){return;} // so exit if WAIT NO use raycast layber bitmask and here just check for locked/set it if false to true and increment a var to decide if turn over
			
			if(info.locked){//so this coin is locked meaning have already launched it
				return;
			} 
			info.locked = true; //so we're going to launch this so need to lock it so can't double launch in same turn
			
			
			Rigidbody coin = target.GetComponent<Rigidbody>();
			
			float distToHit = Vector3.Distance(hit.point,clickPos);
			
			float ratio = distToHit/dist;
			
			
			
			Vector3 offset = hit.point - coin.position;
			
			float offsetsize = offset.magnitude;
			//Debug.Log(offsetsize); offset = useless for coins--always = .5. For pencilwars (original) it had meaning...
			//Debug.Log(forceFactor);
			forceFactor = target.GetComponent<coinAndroidTest>().forceFactor;

			//so now to try out flicking replace below line with something that takes into account elapsed time and magnitude/that is a measure of velocity of finger
			//Vector3 forceToAdd = dir.normalized * ratio * forceFactor;// * Mathf.Pow (offsetsize,spower);
			float ratio1 = 0.0f;

			Vector3 dirScreen = touchPos2 - touchPos1;
			float distScreen = dirScreen.magnitude;
			float velScreen = distScreen/elapsed;
		
			Debug.Log (velScreen);
			/*so now just need to use velScreen to compute ratio1-testing method I'll emply is as follows:
			 * 1) Try out a bunch of velScreens-seen printed out and settle on a max one
			 * 2)set ratio1 = it/max
			 * 3)profit
			 * */
			ratio1 = velScreen/maxSpeed;
			Vector3 forceToAdd = dir.normalized * ratio1 * forceFactor;


			coin.AddForceAtPosition(forceToAdd,hit.point);
			
		}
		//e
		
	}

	void CastRayToWorld(Vector2 pos) {
		Ray r = Camera.main.ScreenPointToRay(pos);
		//Vector3 p = r.origin + r.direction*(10 - (-.17f));
		Vector3 p = Vector3.zero;
		RaycastHit test;
		LayerMask lm; //our layermask for raycasting to ignore coins not in turn
		if (turn == 1) {//so p1's turn want to mask out p2 coins. so mask out layer p2
			lm = lm2;//so lm2 is only layer p2. Then at end invert this.
		} else {
			lm = lm1;
		}
		lm = ~lm;
		//Debug.Log (lm.value);
		//Debug.Log (r.direction);
		if (Physics.Raycast (r.origin, r.direction, out test, Mathf.Infinity, lm)) {
			//so need to check if hit a coin. else lower p.y else raise p.y (because if it's a coin then we're going to miss it if we incrase the ray's height..
			/*namely we know from that default unity cylinder primitive = 2 units high
			this means due to .1 scale our coins are .2 units high so .2/2 = .1, the half height of interest.
			 */
			p = test.point;
			GameObject gotHit = test.collider.gameObject;
			//Vector3 adjust = gotHit.transform.up;//so the normal of the surface that was hit
			//so need to make adjust to be terrain normal now..



			if (!gotHit.name.StartsWith("Terrain")){
				//p.y -= .1f;//
				//Vector3 adjust = gotHit.transform.up; //try something else b/c when coin upside down, we get 0 -1 0 rather than 0 1 0.
				Vector3 adjust = test.normal;
				adjust.Normalize();
				 

				//Debug.Log ("p prev: " + p);
				p += -.1f*adjust; //AHA! So that's why. Adjust is being negative b/c coin is flipped over.
				//Debug.Log(adjust);
				//Debug.Log ("p adjusted: " + p + " and adjustment used: " + adjust);
				//Debug.Log("On a coin");
			} else {//so need the Terrain normal - credit to Eric on unity forumns
				//p.y += .1f;
				Terrain t = gotHit.GetComponent<Terrain>();
				Vector3 relPos = p - t.transform.position;
				Vector2 nPos = new Vector2(Mathf.InverseLerp(0.0f, t.terrainData.size.x, relPos.x), Mathf.InverseLerp(0.0f, t.terrainData.size.z, relPos.z));
				
				Vector3 adjust = t.terrainData.GetInterpolatedNormal(nPos.x,nPos.y);
				
				//adjust = adjust.normalized;
				adjust.Normalize();

				p += .1f*adjust;
				//Debug.Log("On the floor");
				//Debug.Log(adjust);
				//Debug.Log(gotHit.transform.up); aha, so transform.up gives the normal. need to use this for adjustment so that will work also on inclined surfaces,not just flat ones w/ normal of 0 1 0			
			}

		}


		if (clicks == 0) {
			clickPos = p;
			clicks += 1;
		} else {

			clicks = 0;
			Vector3 dir = p - clickPos;

			Debug.DrawRay(clickPos,dir,Color.blue,5.0f);
			RaycastHit hit;
			float dist = dir.magnitude;
			if (Physics.Raycast(clickPos,dir,out hit, dist,lm)){
				GameObject target = hit.collider.gameObject;


				coinAndroidTest info = target.GetComponent<coinAndroidTest>();
				//if (info.owner != turn){return;} // so exit if WAIT NO use raycast layber bitmask and here just check for locked/set it if false to true and increment a var to decide if turn over

				if(info.locked){//so this coin is locked meaning have already launched it
					return;
				} 
				info.locked = true; //so we're going to launch this so need to lock it so can't double launch in same turn


				Rigidbody coin = target.GetComponent<Rigidbody>();

				float distToHit = Vector3.Distance(hit.point,clickPos);

				float ratio = distToHit/dist;



				Vector3 offset = hit.point - coin.position;

				float offsetsize = offset.magnitude;
				//Debug.Log(offsetsize); offset = useless for coins--always = .5. For pencilwars (original) it had meaning...
				//Debug.Log(forceFactor);
				forceFactor = target.GetComponent<coinAndroidTest>().forceFactor;
				Vector3 forceToAdd = dir.normalized * ratio * forceFactor;// * Mathf.Pow (offsetsize,spower);

				coin.AddForceAtPosition(forceToAdd,hit.point);

			}

		}
	
	
	}




}
