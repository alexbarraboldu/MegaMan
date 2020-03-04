using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject PlayerObject;
    private Player Player;
    public float CameraSpeed = 1.0f;

    private Camera Main_Camera;

    private float CameraWidth, CameraHeight;

    private float CurrentCameraX, CurrentCameraY;

    public bool Marging;

    void Start()
    {
        Main_Camera = Camera.main;

        CameraHeight = Main_Camera.orthographicSize;
        CameraWidth = CameraHeight * Main_Camera.aspect;

        Player = PlayerObject.gameObject.GetComponent<Player>();
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
        if (Main_Camera.transform.position.x >= 6000.0f)
        {
            Main_Camera.transform.position = new Vector3(6000.0f, 0f, 0f);
            return;
        }
        Main_Camera.transform.position += new Vector3(CameraSpeed, 0f, 0f);
    }

    void PlayerLimits()
    {
        if ((PlayerObject.transform.position.x >= CurrentCameraX + (CameraWidth - CameraWidth / 4)))
        {
            Debug.Log("Right Camera Side;");
            Player.transform.position = new Vector3(CurrentCameraX + (CameraWidth - CameraWidth / 4), Player.transform.position.y);
        }
        else if (PlayerObject.transform.position.x <= CurrentCameraX + (CameraWidth / 6 - CameraWidth))
        {
            Debug.Log("Left Camera Side;");
            Player.transform.position = new Vector3(CurrentCameraX + (CameraWidth / 6 - CameraWidth), Player.transform.position.y);
            Player.TouchingMarging = true;
        }
        else Player.TouchingMarging = false;

        if (PlayerObject.transform.position.y >= CurrentCameraY + (CameraHeight - 10f))
        {
            Debug.Log("Top Camera Side;");
            Player.transform.position = new Vector3(Player.transform.position.x, CurrentCameraY + (CameraHeight - 10f));
        }
        else if (PlayerObject.transform.position.y <= CurrentCameraY + (-CameraHeight + 10f))
        {
            Debug.Log("Bottom Camera Side;");
            Player.transform.position = new Vector3(Player.transform.position.x, CurrentCameraY + (-CameraHeight + 10f));
        }
    }
}
