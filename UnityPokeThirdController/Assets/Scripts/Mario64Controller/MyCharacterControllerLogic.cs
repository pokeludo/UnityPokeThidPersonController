  using UnityEngine;
using System.Collections;

public class MyCharacterControllerLogic : MonoBehaviour {


	[SerializeField]
	private Animator animator;
	[SerializeField]
	private MyThirdPersonCamera gameCam;
	[SerializeField]
	private float directionDampTime = 0.25f;
	[SerializeField]
	private float directionSpeed = 0.25f;
	[SerializeField]
	private float rotationDegreePerSecond = 120f;

	private float speed = 0f;
	private float direction = 0f;
	private float horizontal = 0f;
	private float vertical = 0f;
	private AnimatorStateInfo stateInfo;

	private int m_LocomotionId = 0;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>(); 
		if(animator.layerCount >= 2) {
			animator.SetLayerWeight(1, 1);
		}		

		m_LocomotionId = Animator.StringToHash("Base Layer.Locomotion"); //IDにする
	}
	
	// Update is called once per frame
	void Update () {
	
		if(animator) {
			horizontal = Input.GetAxis("Horizontal"); //入力
			vertical = Input.GetAxis("Vertical"); //入力

			StickToWorldspace(this.transform, gameCam.transform, ref direction, ref speed);

			animator.SetFloat("Speed", speed);
			animator.SetFloat("Direction", horizontal, directionDampTime, Time.deltaTime);
		}

	}


	void FixedUpdate() { // 入力による、キャラクターの回転(丸い軌道にそる走る為に)
		if(IsInLocomotion() && (direction >= 0 && horizontal >= 0) || (direction < 0 && horizontal < 0)) { 
			Vector3 rotationAmount = Vector3.Lerp (Vector3.zero, new Vector3(0f, rotationDegreePerSecond * (horizontal < 0f ? -1f : 1f), 0f), Mathf.Abs(horizontal));
			Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime); //回転について、システム依存度を0にする
			this.transform.rotation = (this.transform.rotation * deltaRotation); //２つの回転の足し算
		}
	}


	public void StickToWorldspace(Transform root, Transform camera, ref float directionOut, ref float speedOut)
	{
		Vector3 rootDirection = root.forward;
		
		Vector3 stickDirection = new Vector3(horizontal, 0, vertical);
		
		speedOut = stickDirection.sqrMagnitude;		
		
		// カメラの回転
		Vector3 CameraDirection = camera.forward;
		CameraDirection.y = 0.0f; // 強制的に
		Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, CameraDirection);
		
		// 入力情報をWorldSpace座標への換算
		Vector3 moveDirection = referentialShift * stickDirection;
		Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);
		
		//Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), moveDirection, Color.green);
		//Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), rootDirection, Color.magenta);
		//Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), stickDirection, Color.blue);
		//Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2.5f, root.position.z), axisSign, Color.red);
		
		float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);
		//if (!isPivoting) {
		//	angleOut = angleRootToMove;
		//}
		angleRootToMove /= 180f;
		
		directionOut = angleRootToMove * directionSpeed;
	}	

	private bool IsInLocomotion() {
		return stateInfo.nameHash == m_LocomotionId;
	}
	
	
	
	
	
}
