using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AI;
using UnityEngine.AI;


public class BasicMovementAi : MonoBehaviour {

	public GameObject Target;
	public NavMeshAgent Age;
	public float Speed;
	public float Distance;
	public float SearchRange;

	public GameObject[] WayPoint;
	private int w =0;
	private bool CanSee;
	private float Timer;


	// Use this for initialization
	void Awake () {
		Age = GetComponent<NavMeshAgent> ();
		Age.speed = Speed;
		Age.stoppingDistance = Distance;
		WayPoint = GameObject.FindGameObjectsWithTag ("WPTag");

	}
	
	// Update is called once per frame
	void LateUpdate () {

		RaycastHit hit;

//		Collider[] hitColliders = Physics.OverlapSphere (transform.position, SearchRange);
//
//		for (int i = 0; i < hitColliders.Length; i++) {
//			if (hitColliders [i].gameObject.transform.tag == "Player") {
//				Target = hitColliders [i].gameObject;
//			
//				break;
//			}
//		}


		if (!Target) {
			print ("a");
			Search ();
		}

		if (Target) {
			
			transform.LookAt (Target.transform.position);
			Spot ();
		}


	}
	public void Search()
	{
		WayPointMove ();
		Age.Resume ();
		Collider[] hitColliders = Physics.OverlapSphere (transform.position, SearchRange);

		for (int i = 0; i < hitColliders.Length; i++) {
			if (hitColliders [i].gameObject.transform.tag == "Player") {
				Target = hitColliders [i].gameObject;

				break;
			}
		}
	}
	public void WayPointMove()
	{



		float Dist = Vector3.Distance (transform.position, WayPoint [w].transform.position);

		for (int i = 0; i <= WayPoint.Length; i++) {
			if (i == w) { 
				
				Age.SetDestination (WayPoint [w].transform.position);
			
				break;
			}
		}

		if (Dist <= Age.stoppingDistance || Dist <= 0) {
			w ++;

		}

		if (w > WayPoint.Length-1) {
			w = 0;

		}
	}


	public void Spot()
	{
		


		//Age.SetDestination (Target.transform.position);

		RaycastHit hit;
	//	Ray ray;
		Vector3 fwd = Target.transform.position - transform.position;

	


		if (Physics.Raycast (transform.position, fwd, out hit, SearchRange)) {
			if (hit.collider.gameObject.tag == "Player") {
			//	print ("i see you");
				Age.isStopped = true;
				CanSee = true;
				BroadCastPlayer (Target.transform.position);
				Timer = .6f;
			} else {
			//	print ("booo");
				CanSee = false;
			}
		}

		if (!CanSee) {
		//	print ("Cant see");
			Timer -= Time.deltaTime;
		}

		if (Timer <= 0) {
		//	print ("Targetnull");
		
			Target = null;
		}
	

	}

	void BroadCastPlayer(Vector3 TarPos)
	{
		GameObject.FindGameObjectWithTag ("Seeker").GetComponent<MainBodyScript> ().TargetLocal= TarPos;
	}
}
