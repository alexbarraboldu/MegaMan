using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public GameObject PlayerObject;
	private Player Player;
	public float CameraSpeed = 0.001f;

	private Camera Main_Camera;

	private float CameraWidth, CameraHeight;

	private float CurrentCameraX, CurrentCameraY;

	public Vector2 PlayerCameraBounds;

	public bool Marging;
	public bool BossSpot;

	void Start()
	{
		Main_Camera = Camera.main;

		CameraHeight = Main_Camera.orthographicSize;
		CameraWidth = CameraHeight * Main_Camera.aspect;

		Player = PlayerObject.gameObject.GetComponent<Player>();
		CameraSpeed = 0.035f;
	}

	void Update()
	{
		Main_Camera = Camera.main;

		CurrentCameraY = Main_Camera.transform.position.y;
		CurrentCameraX = Main_Camera.transform.position.x;

		if (!Player || !PlayerObject) return;
		PlayerLimits();
		CameraMovement();
	}

	void CameraMovement()
	{
		if (Main_Camera.transform.position.x >= 60.0f)
		{
			BossSpot = true;
			Main_Camera.transform.position = new Vector3(60.0f, 0f, -10f);
		}
	}

	void PlayerLimits()
	{
		if (BossSpot) return;
		if (PlayerObject.transform.position.x > CurrentCameraX + PlayerCameraBounds.x)
		{
			Debug.Log("Right Side;");
			Marging = true;
			Main_Camera.transform.position += new Vector3(CameraSpeed, 0f, 0f);
		}
		else Marging = false;


		if (PlayerObject.transform.position.x < CurrentCameraX - PlayerCameraBounds.x)
		{
			Debug.Log("Left Side;");
			//Player.transform.position = new Vector3(CurrentCameraX + (CameraWidth / 6 - CameraWidth), Player.transform.position.y);
			Marging = true;
			Main_Camera.transform.position -= new Vector3(CameraSpeed, 0f, 0f);
		}
		else Marging = false;

		//if (PlayerObject.transform.position.y > CurrentCameraY + (-PlayerCameraBounds.y))
		//{
		//	Debug.Log("Top Side;");
		//	//Player.transform.position = new Vector3(Player.transform.position.x, CurrentCameraY + (CameraHeight - 10f));
		//	Marging = true;
		//	Main_Camera.transform.position += new Vector3(0f, CameraSpeed, 0f);
		//}
		//else if (PlayerObject.transform.position.y < CurrentCameraY + PlayerCameraBounds.y)
		//{
		//	Debug.Log("Bottom Side;");
		//	//Player.transform.position = new Vector3(Player.transform.position.x, CurrentCameraY + (-CameraHeight + 10f));
		//	Marging = true;
		//	Main_Camera.transform.position -= new Vector3(0f, CameraSpeed, 0f);
		//}
		//else Marging = false;
	}
}
