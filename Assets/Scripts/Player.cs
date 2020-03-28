using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private Vector2 Movement;
	private Rigidbody2D rb2d;

	private BoxCollider2D box2D;

	private float fixedDelta;
	public float Counter;

	[Header("Variables for player:")]
	[Space(5)]
	public float Speed = 0.125f;
	public float VSpeed = 1.5f;
	public bool TouchingMarging;

	public float health = 30.0f;
	//public bool IsDead = false;

	public float JumpRate = 10f;
	public float JumpTime;

	public bool Jumping = false;

	[Header("Variables for gun:")]
	[Space(10)]
	public GameObject bulletObject;
	private GameObject AuxBulletObject;
	private Rigidbody2D rb2dBullet;
	public Transform firePoint;
	public float BulletSpeed = 0.25f;
	public float fireRate = 5.0f;
	private float initBulletTime;

	//[Header("Variables for AudioManager:")]
	//[Space(10)]
	//  // public SoundManager SoundManager;
	//private string ShootSound = "PlayerShoot";
	//private string DeadSound = "PlayerDead";
	//private string HittedSound = "PlayerHit";

	private void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		box2D = GetComponent<BoxCollider2D>();
		
		rb2dBullet = null;
		Movement = Vector2.zero;

		//SoundManager = FindObjectOfType<SoundManager>();
	}

	private void Update()
	{
		if (health <= 0)
		{
			GameManager.Instance.PlayerIsDead = true;
			//SoundManager.Play(DeadSound);
			Destroy(gameObject);
			GameManager.Instance.CheckPlayer();
		}
	}

	private void FixedUpdate()
	{
		fixedDelta = Time.fixedDeltaTime * 1000.0f;
		Counter = Time.time * fixedDelta;

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
		if (rb2d.velocity.x > Speed || rb2d.velocity.x < Speed) rb2d.velocity = new Vector2(0f, Movement.y);

		Movement.x = Input.GetAxis("Horizontal");

		if ((Jumping == false && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))))
		{
			Movement = new Vector2(Movement.x, VSpeed);
			JumpTime = Counter + JumpRate;
		}
		else if (Counter >= JumpTime - 15f && Jumping == true)
		{
			Movement = new Vector2(Movement.x, 0f);
		}

		rb2d.AddForce(Movement * Speed * fixedDelta, ForceMode2D.Impulse);
	}

	void Shooting()
	{
		AuxBulletObject = Instantiate(bulletObject.gameObject, firePoint.position, firePoint.rotation);
		AuxBulletObject.GetComponent<Bullet>().WhoShoot = true;
		rb2dBullet = AuxBulletObject.GetComponent<Rigidbody2D>();
		rb2dBullet.AddForce(firePoint.right * BulletSpeed * fixedDelta, ForceMode2D.Impulse);
		Destroy(AuxBulletObject, 1f);
	}

	private bool checkRaycastWithScenario(RaycastHit2D[] hits)
	{
		foreach (RaycastHit2D hit in hits)
		{
			if (hit.collider != null)
			{
				if (hit.collider.gameObject.tag == "Grid") return true;
			}
		}
		return false;
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Grid")
		{
			if (Jumping)
			{
				bool col1 = false;
				bool col2 = false;
				bool col3 = false;
				float center_x = (box2D.bounds.min.x + box2D.bounds.max.x) / 2;
				Vector2 centerPosition = new Vector2(center_x, box2D.bounds.min.y) / 2;
				Vector2 leftPosition = new Vector2(box2D.bounds.min.x, box2D.bounds.min.y) / 2;
				Vector2 rightPosition = new Vector2(box2D.bounds.max.x, box2D.bounds.min.y) / 2;

				RaycastHit2D[] hits = Physics2D.RaycastAll(centerPosition, -Vector2.up, 2);
				if (checkRaycastWithScenario(hits)) { col1 = true; }

				hits = Physics2D.RaycastAll(leftPosition, -Vector2.up, 2);
				if (checkRaycastWithScenario(hits)) { col2 = true; }

				hits = Physics2D.RaycastAll(rightPosition, -Vector2.up, 2);
				if (checkRaycastWithScenario(hits)) { col3 = true; }

				if (col1 || col2 || col3) { Jumping = false; }
			}
		}
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Bullet")
		{
			if (!collision.gameObject.GetComponent<Bullet>().WhoShoot)
			{
				health -= 10f;
				Destroy(collision.gameObject);
				//SoundManager.Play(HittedSound);
			}
		}
		if (collision.gameObject.tag == "Enemy")
		{
			health -= 40f;
			Destroy(collision.gameObject);
			//SoundManager.Play(HittedSound);
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Grid")
		{
			Jumping = true;
		}
	}
}
