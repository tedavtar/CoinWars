using UnityEngine;
using System.Collections;


//thanks to unity forum 827834 click-drag-camera question (he self answered)
public class panCamera : MonoBehaviour {

	public float xFactor = 1.0f;
	public float yFactor = 1.0f;

	public float speed = 5.0f;//4

	public bool dragging = false;
	public Vector3 pos1;
	public Vector3 panOri;

	//mobile speeds
	public float orthoZoomSpeed = 0.000000000001f;//0.0000000001
	public float panSpeed = 0.000000000000001f;

	public float dispThreshold = 1.8f;//1.75

	// Update is called once per frame
	void Update () {

		 //COMMENT THIS BACK IN FOR WEB BUILDS AND COMMENT OUT FOR ANDROID
		if (Input.GetMouseButtonDown(1)){
			pos1 = transform.position;
			dragging = true;
			panOri = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		}
		if (Input.GetMouseButtonUp(1)){
			dragging = false;
		}
		if (dragging){
			Vector3 pos2 = Camera.main.ScreenToViewportPoint(Input.mousePosition) - panOri;
			Vector3 temp = pos2;
			pos2.z = pos2.y;
			transform.position = pos1 - pos2*speed;
			Vector3 temp2 = transform.position;
			temp2.y = 10;
			transform.position = temp2;
			
		}


		//s
		// If there are two touches on the device...
		if (Input.touchCount == 2) {
			// Store both touches.
			Touch touchZero = Input.GetTouch (0);
			Touch touchOne = Input.GetTouch (1);
			
			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
			
			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
			
			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
			//GameObject.Find("Game").GetComponent<webGame>().disp(deltaMagnitudeDiff.ToString());
			Debug.Log (deltaMagnitudeDiff);
			
			if (Mathf.Abs(deltaMagnitudeDiff) > dispThreshold) {  //so do the zoom
				Camera.main.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
				
				// Make sure the orthographic size never drops below zero.
				Camera.main.orthographicSize = Mathf.Max (Camera.main.orthographicSize, 0.1f);
			} else {//do the panning
				/*
				Vector2 avgMov = touchZero.deltaPosition + touchOne.deltaPosition;
				avgMov = avgMov/2;
				transform.Translate(-avgMov.x * speed, -avgMov.y * speed, 0);*/
				switch (touchZero.phase){
					
				case TouchPhase.Began:
					pos1 = transform.position;
					dragging = true;
					panOri = Camera.main.ScreenToViewportPoint(Input.mousePosition);
					break;
			
					
				case TouchPhase.Ended:
					dragging = false;
					//Debug.Log("reached here");
					break;
				}
				if (dragging){
					Vector3 pos2 = Camera.main.ScreenToViewportPoint(Input.mousePosition) - panOri;
					Vector3 temp = pos2;
					pos2.z = pos2.y;
					transform.position = pos1 - pos2*speed;
					Vector3 temp2 = transform.position;
					temp2.y = 10;
					transform.position = temp2;
					
				}
			}



		}
		//e
	}


}
