﻿using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Limbing : MonoBehaviour
{
    public bool isInput;
    public Transform cart;
    public Transform tree;
    public GameObject notification;
    public Animator anim;
    public ParticleSystem particleEmissions;
    public ParticleSystem particleLimbing;

    public GameObject tutorialHand;
    public Image imgBG;

    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
    private bool isTutorial;
    private bool isStop;

    public Transform tfStart;
    public Transform tfEnd;
    public Sprite iconOutPut;

    public void Start()
    {
        posCheck = transform.GetChild(0).position;
    }

    private void OnEnable()
    {
        int randomBG = Random.Range(0, UIManager.Instance.spBG.Length);
        imgBG.sprite = UIManager.Instance.spBG[randomBG];

        tree.localScale = Vector3.one;

        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        if (GameManager.Instance.lsLocation[ID].indexTypeRisk == -1)
        {
            isTutorial = true;
        }
        else
        {
            isTutorial = false;
            tutorialHand.SetActive(false);
        }
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
                if (Input.mousePosition.y > posDown.y)
                {
                    float dis = Input.mousePosition.y - posDown.y;
                    cart.position += new Vector3(0f, dis * 0.01f * Time.deltaTime, 0f);
                }
                if (cart.position.y > posCheck.y)
                {
                    StartCoroutine(CompleteJob());
                }
            }

            if (GameManager.Instance.lsLocation[GameManager.Instance.IDLocation].indexTypeRisk != -1)
            {
                notification.SetActive(false);
            }
           
        }
        else
        {
            if (GameManager.Instance.lsLocation[GameManager.Instance.IDLocation].indexTypeRisk != -1)
            {
                notification.SetActive(false);
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
    }

    public void TapDown()
    {
        int id = GameManager.Instance.IDLocation;
        if (isInput && GameManager.Instance.lsLocation[id].indexTypeRisk == -1)
        {
            anim.enabled = true;
            particleLimbing.Play();
            particleEmissions.Play();
            AudioManager.Instance.Play("Debarking");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        anim.enabled = false;
        particleLimbing.Stop();
        particleEmissions.Stop();
        AudioManager.Instance.Stop("Debarking");
        isRun = false;
    }

    public void LoadInput()
    {

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
        isInput = false;

        anim.enabled = false;
        particleLimbing.Stop();
        particleEmissions.Stop();
        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        yield return new WaitForSeconds(0.5f);

        tree.DOScale(Vector3.zero, 0.5f);
        tree.DOMove(tfEnd.position, 0.5f).OnComplete(() =>
        {
            GameManager.Instance.lsLocation[ID].JobComplete(IndexType);

            GameManager.Instance.AddOutPutMiniGame(IndexType);

            cart.localPosition = new Vector3(-4f, 0f, 0f);
            tree.localPosition = Vector3.zero;
            tree.localScale = Vector3.one;
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
