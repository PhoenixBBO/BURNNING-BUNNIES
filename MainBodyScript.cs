using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AI;
using UnityEngine.AI;

public class MainBodyScript : MonoBehaviour {


	//// Eyeballs find Player
	/// MainBody Goes to the last seen Location
	/// 


	public Vector3 TargetLocal;
	public NavMeshAgent BriAge;
	public float AttackRange;
	public int Damage;
	public bool Attacking;
	void Awake()
	{
		BriAge = this.GetComponent<NavMeshAgent> ();
		TargetLocal = transform.position;
	}
	void Update()
	{
		
	
		float Dis = Vector3.Distance (transform.position, TargetLocal);
		if (Dis <= AttackRange) {

			if (Attacking == false) {
				print ("TriggerOnce!!!!");
				StartCoroutine ("OverReact");
			}
		} else {
			Attacking = false;
		}



	}

	void OnDrawGizmos(){
		Gizmos.color = Color.white;
		Gizmos.DrawSphere (transform.position,AttackRange);
	}

	void LateUpdate()
	{
	


		BriAge.SetDestination (TargetLocal);

	}

	IEnumerator OverReact()
	{
		Attacking = true;
		Collider[] hits = Physics.OverlapSphere (transform.position, AttackRange);

		for (int i = 0; i < hits.Length; i++) {
			if (hits [i].gameObject.tag == "Player") {
				hits [i].gameObject.GetComponent<PlayerStats> ().Health_ -= Damage;
			}
		}

		print ("TAKING A SWING BRO");

		yield return new WaitForSeconds (3);

		if (Attacking == true) {

			StartCoroutine ("OverReact");
		}

		if (Attacking == false) {
			yield return null;
		}
	}



}
