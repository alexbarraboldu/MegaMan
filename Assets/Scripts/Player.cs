using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private Vector2 Movement;
	private Rigidbody2D rb2d;

	private float fixedDelta;

	[Header("Variables for player:")]
	[Space(5)]
	public float Speed = 0.15f;
	public bool TouchingMarging;

	public float health = 30.0f;
	//public bool IsDead = false;

	public float JumpRate;
	public float JumpTime;

	[Header("Variables for gun:")]
	[Space(10)]
	public GameObject bulletObject;
	private GameObject AuxBulletObject;
	private Rigidbody2D rb2dBullet;
	public Transform firePoint;
	public float BulletSpeed = 100.0f;

	public float fireRate = 5.0f;

	public float Counter;
	private float initBulletTime;

	[Header("Variables for AudioManager:")]
	[Space(10)]
   // public SoundManager SoundManager;
	private string ShootSound = "PlayerShoot";
	private string DeadSound = "PlayerDead";
	private string HittedSound = "PlayerHit";


   // public GameManager GameManager;
	private void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		rb2dBullet = null;
		Movement = Vector2.zero;
		//AuxSpeed = Speed;

		//SoundManager = FindObjectOfType<SoundManager>();
		//GameManager = FindObjectOfType<GameManager>();
	}

	private void Update()
	{
		if (health <= 0)
		{
			//GameManager.CheckPlayer();
			Destroy(gameObject);
			//SoundManager.Play(DeadSound);
		}
	}

	private void FixedUpdate()
	{
		fixedDelta = Time.fixedDeltaTime * 1000.0f;
		Counter = /*Time.time **/ fixedDelta;

		PlayerMovement();

		if (Counter >= initBulletTime && (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)))
		{
			Shooting();
			//SoundManager.Play(ShootSound);
			initBulletTime = Counter + fireRate;
		}
	}

	void PlayerMovement()
	{
		if (rb2d.velocity.x > Speed || rb2d.velocity.x < Speed) rb2d.velocity = new Vector2(0, Movement.y);

		Movement.x = Input.GetAxis("Horizontal");

		if (Counter >= JumpTime && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.RightControl)))
		{
			Movement = new Vector2(Movement.x, 2f);

			JumpTime = Counter + JumpRate;
		}
		else if (Counter >= JumpTime)
			{
				Movement = new Vector2(Movement.x, 0f);
				JumpTime = Counter + JumpRate;
			}

		rb2d.AddForce(Movement * Speed * fixedDelta, ForceMode2D.Impulse);
	}

	void Shooting()
	{
		//AuxBulletObject = Instantiate(bulletObject.gameObject, firePoint.position, firePoint.rotation);
		//AuxBulletObject.GetComponent<Bullet>().WhoShoot = true;
		//rb2dBullet = AuxBulletObject.GetComponent<Rigidbody2D>();
		//rb2dBullet.AddForce(firePoint.right * BulletSpeed * fixedDelta, ForceMode2D.Impulse);
		//Destroy(AuxBulletObject, 1f);
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Paredes" && TouchingMarging)
		{
			health = 0;
			//SoundManager.Play(DeadSound);
		}
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Bullet")
		{
			//if (!collision.gameObject.GetComponent<Bullet>().WhoShoot)
			//{
			//    health -= 10f;
			//    Destroy(collision.gameObject);
			//    SoundManager.Play(HittedSound);
			//}
		}
		if (collision.gameObject.tag == "Enemy")
		{
			health -= 40f;
			Destroy(collision.gameObject);
			//SoundManager.Play(HittedSound);
		}
	}
}
