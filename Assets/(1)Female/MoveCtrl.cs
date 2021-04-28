using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityStandardAssets.Utility;
using UnityEngine.SceneManagement;

public class MoveCtrl : MonoBehaviourPunCallbacks
{
    private float h, v;
    private Rigidbody rb;
    private Transform tr;

    public float speed = 10f;
    public float rotSpeed = 60.0f;
    public float curHp = 100f;
    public float maxHp = 100f;
    public Image hpBar;
    private GameObject Tape;
    private GameObject Potion;
    public GameObject Tape_;
    public GameObject Potion_;
    private GameObject knife;
    private GameObject HandGun;
    public int Change;
    private bool CanvasTape;
    private bool CanvasPotion;
    public Animator AN;
    public bool Move = true;
    public bool TapeTime = false;
    float time = 0;
    float timeLimit = 7f;

    public Transform potionDestroyPos;
    public GameObject Potion_Destroy_box;
    public Transform tapeDestroyPos;
    public GameObject Tape_Destroy_box;

    //Weapon Box
    public Transform HandGunPos;
    public GameObject HandGunBox;
    public Transform KnifePos;
    public GameObject KnifeBox;
    public bool Scene = false;

    //Enemy Move
    public Transform EnemyMoveTruePos;
    public GameObject EnemyMoveTrueBox;

    public GameObject MoveFalseZone;

    public GameObject bullet;
    AudioSource theAudio;

    public bool GameWin = false;

    void Start()
    {
        theAudio = GameObject.Find("Female(Clone)").GetComponent<Sound>().theAudio;
        MoveFalseZone = GameObject.Find("MoveFalse");
        MoveFalseZone.SetActive(false);

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -1.5f, 0); //player중심설정
        Tape = GameObject.Find("Tape");
        Tape_ = GameObject.Find("Tape_");
        Potion = GameObject.Find("Potion");
        Potion_ = GameObject.Find("Potion_");
        HandGun = GameObject.Find("HandGun");
        knife = GameObject.Find("knife");

        Tape_.SetActive(false);
        Potion_.SetActive(false);
        HandGun.SetActive(false);
        knife.SetActive(true);
        CanvasTape = false;
        CanvasPotion = false;

        tr = GetComponent<Transform>();
        if (photonView.IsMine)
        {
            Camera.main.GetComponent<SmoothFollow>().target = tr.Find("camera_follow").transform;
        }
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        Debug.Log("Female" + Move);

        if (Move == true)
        {
            v = Input.GetAxis("Vertical");
            h = Input.GetAxis("Horizontal");

            tr.Translate(Vector3.forward * v * speed * Time.deltaTime);
            tr.Rotate(Vector3.up * h * rotSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)
                || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                GetComponent<Animator>().Play("run");
                AN.SetBool("run", true);
            }

            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)
                || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                GetComponent<Animator>().Play("idle");
                AN.SetBool("run", false);
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                photonView.RPC("KnifeCreate", RpcTarget.Others, null);
                KnifeCreate();
                Change = 1;
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
                photonView.RPC("HandGunCreate", RpcTarget.Others, null);
                HandGunCreate();
                Change = 2;
            }

            if (Input.GetKeyDown(KeyCode.Q) && CanvasTape == true)
            {
                photonView.RPC("tapeDestroy", RpcTarget.Others, null);
                tapeDestroy();
            }

            if (Input.GetKeyDown(KeyCode.W) && CanvasPotion == true)
            {
                photonView.RPC("potionDestroy", RpcTarget.Others, null);
                potionDestroy();
            }

            if (TapeTime == true)
            {
                time += Time.deltaTime;
                Debug.Log(time);
                if (time > timeLimit)
                {
                    photonView.RPC("EnemyMoveTrue", RpcTarget.Others, null);
                    EnemyMoveTrue();
                    TapeTime = false;
                }
            }
        }
    }

    [PunRPC]
    void potionDestroy()
    {
        Instantiate(Potion_Destroy_box, potionDestroyPos.position, potionDestroyPos.rotation);
    }
    
    [PunRPC]
    void tapeDestroy()
    {
        Instantiate(Tape_Destroy_box, tapeDestroyPos.position, tapeDestroyPos.rotation);
    }

    //Weapon
    [PunRPC]
    void HandGunCreate()
    {
        Instantiate(HandGunBox, HandGunPos.position, HandGunPos.rotation);
    }
    [PunRPC]
    void KnifeCreate()
    {
        Instantiate(KnifeBox, KnifePos.position, KnifePos.rotation);
    }

    [PunRPC]
    void EnemyMoveTrue()
    {
        Instantiate(EnemyMoveTrueBox, EnemyMoveTruePos.position, EnemyMoveTruePos.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Debug.Log("Bullet Collision");
            curHp -= 10;
            hpBar.fillAmount = curHp / maxHp;
            if (curHp <= 0)
            {
                GetComponent<Animator>().Play("dying");
            }
        }

        if (other.gameObject.tag == "HammerAttackBox")
        {
            Debug.Log("Hammer Collision");
            curHp -= 10;
            hpBar.fillAmount = curHp / maxHp;
            if (curHp <= 0)
            {
                GetComponent<Animator>().Play("dying");
            }
        }


        if (other.gameObject.tag == "Tape")
        {
            GetComponent<Animator>().Play("pick");
            Debug.Log("Tape Collision");
            Destroy(Tape, 0.5f);
            Tape_.SetActive(true);
            CanvasTape = true; theAudio.clip = GameObject.Find("Female(Clone)").GetComponent<Sound>().clip[1];
            theAudio.Play();
        }

        if (other.gameObject.tag == "Potion")
        {
            GetComponent<Animator>().Play("pick");
            Debug.Log("Potion Collision");
            Destroy(Potion, 0.5f);
            curHp += 10;
            hpBar.fillAmount = curHp / maxHp;
            Potion_.SetActive(true);
            CanvasPotion = true;
            theAudio.clip = GameObject.Find("Female(Clone)").GetComponent<Sound>().clip[1];
            theAudio.Play();
        }

        if (other.gameObject.tag == "Potion_Destroy_box")
        {
            Potion_.SetActive(false);
            curHp += 10;
            hpBar.fillAmount = curHp / maxHp;
            CanvasTape = true; theAudio.clip = GameObject.Find("Female(Clone)").GetComponent<Sound>().clip[3];
            theAudio.Play();
        }

        if (other.gameObject.tag == "Tape_Destroy_box")
        {
            Tape_.SetActive(false);
            if (GameObject.Find("Male(Clone)"))
            {
                GameObject.Find("Male(Clone)").GetComponent<M_MoveCtrl>().Move = false;
                TapeTime = true;
                GameObject.Find("Male(Clone)").GetComponent<M_MoveCtrl>().M_MoveFalseZone.SetActive(true);
            }
        }

        if (other.gameObject.tag == "HandGunBox")
        {
            Debug.Log("HandGunBox");
            knife.SetActive(false);
            HandGun.SetActive(true);
        }

        if (other.gameObject.tag == "KnifeBox")
        {
            Debug.Log("KnifeBox");
            knife.SetActive(true);
            HandGun.SetActive(false);
        }

        if (other.gameObject.tag == "EnemyMoveTrue")
        {
            if (GameObject.Find("Male(Clone)"))
            {
                GameObject.Find("Male(Clone)").GetComponent<M_MoveCtrl>().Move = true;
                GameObject.Find("Male(Clone)").GetComponent<M_MoveCtrl>().M_MoveFalseZone.SetActive(false);
            }
        }
    }
}
