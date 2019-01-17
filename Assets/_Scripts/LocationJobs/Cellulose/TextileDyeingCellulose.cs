﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TextileDyeingCellulose : MonoBehaviour
{

    public bool isInput;
    public Transform cart;
    public Transform tree;
    public GameObject notification;
    public Animator anim;
    public Transform gear;
    public GameObject tutorialHand;
    public Image imgBG;
    public SpriteRenderer spPaper;
    public Sprite[] arrspPaper;
    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
    private Vector3 posCheckHand;
    private bool isTutorial;
    private bool isStop;
    private int randomPaper;

    public Transform tfStart;
    public Transform tfEnd;

    public void Start()
    {
        posCheck = transform.GetChild(0).position;
        posCheckHand = transform.GetChild(1).position;
    }

    private void OnEnable()
    {
        int randomBG = Random.Range(0, UIManager.Instance.spBG.Length);
        imgBG.sprite = UIManager.Instance.spBG[randomBG];
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;

        if (GameManager.Instance.lsLocation[ID].indexTypeRisk == -1)
        {
            isTutorial = true;
        }
        cart.localPosition = new Vector3(-1.5f, 0f, 0f);
        if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
        {
            notification.SetActive(false);
            tree.gameObject.SetActive(true);
            LoadInput();
        }
        else
        {
            isStop = true;
            tree.gameObject.SetActive(false);
            notification.SetActive(true);
        }
    }

    public void Update()
    {
        if (!isStop)
        {
            if (isRun)
            {
                if (Input.mousePosition.x > posDown.x)
                {
                    float dis = Input.mousePosition.x - posDown.x;
                    cart.localPosition += new Vector3(dis * 0.01f * Time.deltaTime, 0f, 0f);
                    gear.localEulerAngles += new Vector3(0f, 0f, dis * 5f * Time.deltaTime);
                }
                if (cart.position.x > posCheck.x)
                {
                    StartCoroutine(CompleteJob());
                }
            }
        }
        else
        {
            if (GameManager.Instance.lsLocation[GameManager.Instance.IDLocation]
           .lsWorking[GameManager.Instance.lsLocation[GameManager.Instance.IDLocation].indexType].input > 0)
            {
                notification.SetActive(false);
                tree.gameObject.SetActive(true);
                LoadInput();
                isStop = false;
            }
        }
    }

    public void TapDown()
    {
        int id = GameManager.Instance.IDLocation;
        if (isInput && GameManager.Instance.lsLocation[id].indexTypeRisk == -1)
        {
            anim.enabled = true;
            AudioManager.Instance.Play("Polish");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        anim.enabled = false;

        AudioManager.Instance.Stop("Polish");
        isRun = false;
    }

    public void LoadInput()
    {
        randomPaper = Random.Range(0, arrspPaper.Length);
        spPaper.sprite = arrspPaper[randomPaper];
        cart.DOLocalMove(Vector3.zero, 1f).OnComplete(() =>
        {
            if (isTutorial)
            {
                tutorialHand.SetActive(true);
                isTutorial = false;
            }
            isInput = true;
        });
    }

    public IEnumerator CompleteJob()
    {
        anim.enabled = false;
        isRun = false;
        isInput = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        yield return new WaitForSeconds(0.5f);
        
        tree.DOScale(Vector3.zero, 0.5f);
        tree.DOMove(tfEnd.position, 0.5f).OnComplete(() =>
        {
            GameManager.Instance.lsLocation[ID].JobComplete(IndexType);

            GameManager.Instance.AddOutPutMiniGame(IndexType);

            cart.localPosition = new Vector3(-1.5f, 0f, 0f);
            tree.localPosition = Vector3.zero;
            tree.localScale = new Vector3(0.82f, 0.82f, 1f);

            tutorialHand.SetActive(false);
            if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
            {
                LoadInput();
            }
            else
            {
                isStop = true;
                tree.gameObject.SetActive(false);
                notification.SetActive(true);
            }
        });
    }

    public void Help()
    {
        int id = GameManager.Instance.IDLocation;
        if (GameManager.Instance.lsLocation[id].indexTypeRisk != -1)
            return;
        tutorialHand.SetActive(true);
    }
}
