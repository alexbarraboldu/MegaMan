using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Vector2 Movement;
    private Rigidbody2D rb2d;

    private float fixedDelta;

    [Header("Variables for enemy:")]
    [Space(5)]
    public float Speed = 1.0f;
    public float AuxSpeed;

    public float health = 30.0f;

    [Header("Variables for gun:")]
    [Space(10)]
    public GameObject bulletObject;
    private GameObject AuxBulletObject;
    private Rigidbody2D rb2dBullet;
    public Transform firePoint;
    public float BulletSpeed = 0.25f;

    public float fireRate = 5.0f;

    private float ShootCounter;
    private float initBulletTime;

    //[Header("Variables for AudioManager:")]
    //[Space(10)]
    //public SoundManager SoundManager;
    //private string ShootSound = "EnemyShoot";
    //private string DeadSound = "EnemyDead";
    //private string HittedSound = "EnemyHit";

    //[Header("Variables for GameManager:")]
    //[Space(10)]
    //public GameManager GameManager;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2dBullet = null;
        Movement = Vector2.zero;
        AuxSpeed = Speed;
        //SoundManager = FindObjectOfType<SoundManager>();
        //GameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            //SoundManager.Play(DeadSound);
        }
        EnemiesOutOfFrame();
    }

    private void FixedUpdate()
    {
        fixedDelta = Time.fixedDeltaTime * 1000.0f;
        ShootCounter = Time.time * fixedDelta;

        if (ShootCounter >= initBulletTime + 20f)
        {
            Shooting();
            //SoundManager.Play(ShootSound);
            initBulletTime = ShootCounter + fireRate;
        }
    }

    void Shooting()
    {
        AuxBulletObject = Instantiate(bulletObject.gameObject, firePoint.position, firePoint.rotation);
        AuxBulletObject.GetComponent<Bullet>().WhoShoot = false;
        rb2dBullet = AuxBulletObject.GetComponent<Rigidbody2D>();
        rb2dBullet.AddForce(firePoint.right * BulletSpeed * fixedDelta, ForceMode2D.Impulse);
        Destroy(AuxBulletObject, 1f);
    }

    void EnemiesOutOfFrame()
    {
        //if (transform.position.x <= GameManager.Player.transform.position.x - 10)
        //{
        //    Destroy(gameObject);
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (collision.gameObject.GetComponent<Bullet>().WhoShoot)
            {
                Destroy(gameObject);
                Destroy(collision.gameObject);
                GameManager.Instance.addPoints(10);

                //SoundManager.Play(HittedSound);
            }
        }

    }
}
