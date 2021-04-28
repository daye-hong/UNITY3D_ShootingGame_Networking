using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityStandardAssets.Utility;
using UnityEngine.SceneManagement;

public class M_MoveCtrl : MonoBehaviourPunCallbacks
{
    private float h, v;
    private Rigidbody rb;
    private Transform tr;

    public float speed = 10f;
    public float rotSpeed = 60.0f;
    public float curHp = 100f;
    public float maxHp = 100f;
    public Image M_hpBar;
    private GameObject Tape;
    private GameObject Potion;
    public GameObject Tape_;
    public GameObject Potion_;
    private GameObject Hammer;
    private GameObject HandGun;
    private bool CanvasTape;
    private bool CanvasPotion;
    public Animator AN;
    public int Change;
    public bool Move = true;
    public bool TapeTime = false;
    float time = 0;
    float timeLimit = 7f;

    //Item Destroy Box
    public Transform potionDestroyPos;
    public GameObject Potion_Destroy_box;
    public Transform tapeDestroyPos;
    public GameObject Tape_Destroy_box;

    //Weapon Box
    public Transform HandGunPos;
    public GameObject HandGunBox;
    public Transform HammerPos;
    public GameObject HammerBox;

    //Enemy Move
    public Transform EnemyMoveTruePos;
    public GameObject EnemyMoveTrueBox;

    public GameObject M_MoveFalseZone;

    public bool Scene = false;

    AudioSource theAudio;

    public bool GameWin = false;

    void Start()
    {
        theAudio = GameObject.Find("Male(Clone)").GetComponent<M_Sound>().theAudio;
        M_MoveFalseZone = GameObject.Find("M_MoveFalse");
        M_MoveFalseZone.SetActive(false);

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -1.5f, 0); //player중심설정
        Tape = GameObject.Find("Tape");
        Tape_ = GameObject.Find("Tape_");
        Potion = GameObject.Find("Potion");
        Potion_ = GameObject.Find("Potion_");
        HandGun = GameObject.Find("M_HandGun");
        Hammer = GameObject.Find("Hammer");

        Tape_.SetActive(false);
        Potion_.SetActive(false);
        Hammer.SetActive(false);
        HandGun.SetActive(true);
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
                photonView.RPC("HammerCreate", RpcTarget.Others, null);
                HammerCreate();
                Change = 2;
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
                photonView.RPC("HandGunCreate", RpcTarget.Others, null);
                HandGunCreate();
                Change = 1;
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

    //Item
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
    void HammerCreate()
    {
        Instantiate(HammerBox, HammerPos.position, HammerPos.rotation);
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
            M_hpBar.fillAmount = curHp / maxHp;
            if (curHp <= 0)
            {
                GetComponent<Animator>().Play("dying");
            }
        }

        if (other.gameObject.tag == "KnifeAttackBox")
        {
            Debug.Log("knife Collision");
            curHp -= 10;
            M_hpBar.fillAmount = curHp / maxHp;
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
            CanvasTape = true;
            theAudio.clip = GameObject.Find("Male(Clone)").GetComponent<M_Sound>().clip[1];
            theAudio.Play();
        }

        if (other.gameObject.tag == "Potion")
        {
            GetComponent<Animator>().Play("pick");
            Debug.Log("Potion Collision");
            Destroy(Potion, 0.5f);
            Potion_.SetActive(true);
            CanvasPotion = true;
            theAudio.clip = GameObject.Find("Male(Clone)").GetComponent<M_Sound>().clip[1];
            theAudio.Play();
        }

        if (other.gameObject.tag == "Potion_Destroy_box")
        {
            Potion_.SetActive(false);
            curHp += 10;
            M_hpBar.fillAmount = curHp / maxHp;
            theAudio.clip = GameObject.Find("Male(Clone)").GetComponent<M_Sound>().clip[3];
            theAudio.Play();
        }

        if (other.gameObject.tag == "Tape_Destroy_box")
        {
            Tape_.SetActive(false);
            if (GameObject.Find("Female(Clone)"))
            {
                GameObject.Find("Female(Clone)").GetComponent<MoveCtrl>().Move = false;
                TapeTime = true;
                GameObject.Find("Female(Clone)").GetComponent<MoveCtrl>().MoveFalseZone.SetActive(true);
            }
        }

        if (other.gameObject.tag == "M_HandGunBox")
        {
            Debug.Log("HandGunBox");
            Hammer.SetActive(false);
            HandGun.SetActive(true);
        }
        if (other.gameObject.tag == "M_HammerBox")
        {
            Debug.Log("HammerBox");
            Hammer.SetActive(true);
            HandGun.SetActive(false);
        }

        if (other.gameObject.tag == "M_EnemyMoveTrue")
        {
            if (GameObject.Find("Female(Clone)"))
            {
                GameObject.Find("Female(Clone)").GetComponent<MoveCtrl>().Move = true;
                GameObject.Find("Female(Clone)").GetComponent<MoveCtrl>().MoveFalseZone.SetActive(false);
            }
        }
    }
}
