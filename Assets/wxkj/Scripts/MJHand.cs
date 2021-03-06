﻿using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;

public enum AnimationAct
{
    Idle = 0,
    DragCard = 1,
    PutHandCard = 2,
    DropCard = 3,
    PutTable = 4,
    Hu = 5,
}
public class MJHand : MonoBehaviour
{
    public MJPlayer player;
    public GameObject hand;
    public Animation anim;
    public Transform handMJRoot;
    public HandAnima handAnima;
    public Transform dizePos;

    private GameObject baoEffect;
    public bool IsBusy = false;

    public void Clear()
    {
        while (handMJRoot.childCount > 0)
        {
            Transform trans = handMJRoot.GetChild(0);
            Game.PoolManager.CardPool.Despawn(trans.gameObject);
        }
    }

    private int position
    {
        get
        {
            return player.postion;
        }
    }

    public void PlayDropCard(int card, bool isMy)
    {
        IsBusy = true;
        MjData data = Game.MJMgr.MjData[position];

        Game.SoundManager.PlayCardSound(position, card);

        Transform dropCLTrans = player.dropCardLayout.transform;
        int childCount = dropCLTrans.childCount;

        if (isMy)
        {
            if (null == Game.MJMgr.MyDropMJEntity)
            {
                player.handCardLayout.DropCard(card);
            }
            else
            {
                player.handCardLayout.DropCard();
            }

            Game.MJMgr.MyDropMJEntity = null;
            //player.handCardLayout.Sort();
        }
        else
        {
            //int position = Game.MJMgr.GetPositionByIndex(player.index);

            //MjData data = Game.MJMgr.MjData[position];
            //if (data.player.ting)
            //{
            //    int Count = player.handCardLayout.HandCards.Count;
            //    player.handCardLayout.RemoveCardAt(Count);
            //}
            //else
            {
                int count = player.handCardLayout.HandCards.Count;
                if (data.player.ting)
                {
                    player.handCardLayout.RemoveCardAt(count - 1);
                }
                else
                {
                    int index = UnityEngine.Random.Range(2, count);
                    player.handCardLayout.RemoveCardAt(index);
                }
            }

            //player.HandCards.RemoveAt(index);
            // TODO  这里需要做细节
        }

        anim.gameObject.SetActive(true);

        Vector3 toPos = player.dropCardLayout.GetLocalPos(childCount);
        Vector3 endPos = dropCLTrans.TransformPoint(toPos);

        hand.transform.position = endPos;

        GameObject child = Game.PoolManager.CardPool.Spawn(card.ToString());
        Game.MJMgr.LastDropCard = child.GetComponent<MJEntity>();
        Game.MJMgr.LastDropCard.Card = card;

        Transform cardTrans = child.transform;
        cardTrans.SetParent(handMJRoot);
        cardTrans.localPosition = Vector3.zero;
        cardTrans.localRotation = Quaternion.identity;
        cardTrans.localScale = Vector3.one;

        if (UnityEngine.Random.Range(0f, 1f) < 0.8f)
        {
            anim.Play("Drop");
            handAnima.OnDropCallback = () =>
            {
                Game.SoundManager.PlayDropCard();
                Game.MJMgr.targetFlag.gameObject.SetActive(true);
                Game.MJMgr.targetFlag.position = hand.transform.position;

                player.dropCardLayout.AddCard(card, child);
                //hand.SetActive(false);
                player.handCardLayout.LineUp(isMy);
                IsBusy = false;
            };
            //Game.Delay(0.13f, () =>{
            //});
        }
        else
        {
            anim.Play("Drop1");
            handAnima.OnDropSoundCallback = () =>
            {
                Game.SoundManager.PlayDropCard();
            };

            handAnima.OnDropCallback = () =>
            {
                Game.MJMgr.targetFlag.gameObject.SetActive(true);
                Game.MJMgr.targetFlag.position = hand.transform.position;

                player.dropCardLayout.AddCard(card, child);
                //hand.SetActive(false);
                player.handCardLayout.LineUp(isMy);
                IsBusy = false;
            };
            //Game.Delay(0.25f, () => {

            //});
        }
    }

    public void PlayMingGang(int card, bool isMy)
    {
        Game.SoundManager.PlayMingGang(position);

        player.tableCardLayout.AddCard(card);
        player.tableCardLayout.AddCard(card);
        player.tableCardLayout.AddCard(card);
        player.tableCardLayout.ChangeCard(card);

        if (isMy)
        {
            player.handCardLayout.RemoveCard(card);
            player.handCardLayout.RemoveCard(card);
            player.handCardLayout.RemoveCard(card);
        }
        else
        {
            int count = player.handCardLayout.HandCards.Count;
            int index = UnityEngine.Random.Range(0, count);
            int index2 = index - 1;
            if (index <= 0)
            {
                index2 = index + 1;
            }
            ///////////////////////////
            player.handCardLayout.RemoveCardAt(0);
            player.handCardLayout.RemoveCardAt(1);
            player.handCardLayout.RemoveCardAt(2);

            //////////////////////
  //          player.handCardLayout.RemoveCardAt(index);
   //         player.handCardLayout.RemoveCardAt(index2);

        }

        player.handCardLayout.LineUp();

        Game.MJMgr.targetFlag.gameObject.SetActive(false);
        Game.PoolManager.CardPool.Despawn(Game.MJMgr.LastDropCard.gameObject);
        //Game.MJMgr.LastDropCardPlayer.dropCardLayout.RemoveLast();

        EventDispatcher.DispatchEvent(MessageCommand.PlayEffect, position, "gangUI_EF");


        Transform tableCLTrans = player.tableCardLayout.transform;
        int childCount = tableCLTrans.childCount;
        Transform lastChild = tableCLTrans.GetChild(childCount - 1);
        Vector3 endPos = tableCLTrans.TransformPoint(lastChild.localPosition);
        hand.transform.position = endPos;
        anim.gameObject.SetActive(true);
/*
        Debug.Log("明杠====" + childCount);
        for (int a = 0; a < childCount; a++)
        {
            Debug.Log("tableCLTrans====name====" + tableCLTrans.GetChild(childCount - 1).name);
            Debug.Log("aaaaaa====name====" + tableCLTrans.GetChild(a).name);
            if (tableCLTrans.GetChild(childCount - 1).name == tableCLTrans.GetChild(a).name)
            {
                Transform trans2 = tableCLTrans.GetChild(childCount - 1);
                trans2.localPosition = new Vector3(tableCLTrans.GetChild(a + 1).localPosition.x, -0.02f, tableCLTrans.GetChild(a + 1).localPosition.z);
                trans2.localRotation = Quaternion.identity;
                trans2.localScale = Vector3.one;
                break;
            }
        }

*/

        GameObject eff = Game.PoolManager.EffectPool.Spawn("gangUI_EF");
        eff.transform.position = endPos;
        Game.PoolManager.EffectPool.Despawn(eff, 3);

        anim.Play("PutTable");
    }


    public void PlayBuGang(int card, bool isMy)
    {
        Game.SoundManager.PlayMingGang(position);

        player.tableCardLayout.ChangeCard(card);

        if (isMy)
        {
            player.handCardLayout.RemoveCard(card);
        }
        else
        {
            int count = player.handCardLayout.HandCards.Count;
            int index = UnityEngine.Random.Range(0, count);
            int index2 = index - 1;
            if (index <= 0)
            {
                index2 = index + 1;
            }
 //           player.handCardLayout.RemoveCardAt(index);
            player.handCardLayout.RemoveCardAt(0);
        }

        player.handCardLayout.LineUp();

        Game.MJMgr.targetFlag.gameObject.SetActive(false);
        Game.PoolManager.CardPool.Despawn(Game.MJMgr.LastDropCard.gameObject);
        //Game.MJMgr.LastDropCardPlayer.dropCardLayout.RemoveLast();

        EventDispatcher.DispatchEvent(MessageCommand.PlayEffect, position, "gangUI_EF");


        Transform tableCLTrans = player.tableCardLayout.transform;
        int childCount = tableCLTrans.childCount;
        Transform lastChild = tableCLTrans.GetChild(childCount - 1);
        Vector3 endPos = tableCLTrans.TransformPoint(lastChild.localPosition);
        hand.transform.position = endPos;
        anim.gameObject.SetActive(true);
/*
        Debug.Log("补杠====" + childCount);
        for (int a = 0; a < childCount; a++)
        {
            Debug.Log("tableCLTrans====name====" + tableCLTrans.GetChild(childCount - 1).name);
            Debug.Log("aaaaaa====name====" + tableCLTrans.GetChild(a).name);
            if (tableCLTrans.GetChild(childCount - 1).name == tableCLTrans.GetChild(a).name)
            {
                Transform trans2 = tableCLTrans.GetChild(childCount - 1);
                trans2.localPosition = new Vector3(tableCLTrans.GetChild(a + 1).localPosition.x, -0.02f, tableCLTrans.GetChild(a + 1).localPosition.z);
                trans2.localRotation = Quaternion.identity;
                trans2.localScale = Vector3.one;
                break;
            }
        }
*/


        GameObject eff = Game.PoolManager.EffectPool.Spawn("gangUI_EF");
        eff.transform.position = endPos;
        Game.PoolManager.EffectPool.Despawn(eff, 3);

        anim.Play("PutTable");
    }

    public void PlayAngGang(int card, bool isMy)
    {
        Game.SoundManager.PlayMingGang(position);

        player.tableCardLayout.AddCard(card);
        player.tableCardLayout.AddCard(card);
        player.tableCardLayout.AddCard(card);
        player.tableCardLayout.ChangeCard(card);

        if (isMy)
        {
            player.handCardLayout.RemoveCard(card);
            player.handCardLayout.RemoveCard(card);
            player.handCardLayout.RemoveCard(card);
            player.handCardLayout.RemoveCard(card);
        }
        else
        {
            int count = player.handCardLayout.HandCards.Count;
            int index = UnityEngine.Random.Range(0, count);
            int index2 = index - 1;
            if (index <= 0)
            {
                index2 = index + 1;
            }  
            /////////////////////
            player.handCardLayout.RemoveCardAt(0);
            player.handCardLayout.RemoveCardAt(1);
            player.handCardLayout.RemoveCardAt(2);
            player.handCardLayout.RemoveCardAt(3);

            //////////////////////////////////////////
            //           player.handCardLayout.RemoveCardAt(index);
            //           player.handCardLayout.RemoveCardAt(index2);

        }

        player.handCardLayout.LineUp();

        Game.MJMgr.targetFlag.gameObject.SetActive(false);
        Game.PoolManager.CardPool.Despawn(Game.MJMgr.LastDropCard.gameObject);
        //Game.MJMgr.LastDropCardPlayer.dropCardLayout.RemoveLast();

        EventDispatcher.DispatchEvent(MessageCommand.PlayEffect, position, "gangUI_EF");


        Transform tableCLTrans = player.tableCardLayout.transform;
        int childCount = tableCLTrans.childCount;
        Transform lastChild = tableCLTrans.GetChild(childCount - 1);
///////////////

 /*           Transform lastChild1 = tableCLTrans.GetChild(childCount - 1);
            lastChild1.localRotation = Quaternion.Euler(0, 0, 180);
            lastChild1.localScale = Vector3.one;

            Transform lastChild2 = tableCLTrans.GetChild(childCount - 2);
            lastChild2.localRotation = Quaternion.Euler(0, 0, 180);
            lastChild2.localScale = Vector3.one;


            Transform lastChild3 = tableCLTrans.GetChild(childCount - 3);
            lastChild3.localRotation = Quaternion.Euler(0, 0, 180);
            lastChild3.localScale = Vector3.one;
*/
///////////////
        Vector3 endPos = tableCLTrans.TransformPoint(lastChild.localPosition);
        hand.transform.position = endPos;
        anim.gameObject.SetActive(true);

 /*       if (isMy)
        {
   //         trans1.localPosition = new Vector3(tableCLTrans.GetChild(childCount - 3).localPosition.x, -0.02f, tableCLTrans.GetChild(childCount - 2).localPosition.z);

            Transform trans1 = tableCLTrans.GetChild(childCount - 1);
            trans1.localPosition = new Vector3(tableCLTrans.GetChild(childCount - 3).localPosition.x, -0.02f, tableCLTrans.GetChild(childCount - 2).localPosition.z);
            //          trans1.localEulerAngles = new Vector3(0, 0, 180);
            //         trans1.localScale = Vector3.one;
 
        }else {
            Transform trans1 = tableCLTrans.GetChild(childCount - 1);
            trans1.localPosition = new Vector3(tableCLTrans.GetChild(childCount - 3).localPosition.x, -0.02f, tableCLTrans.GetChild(childCount - 2).localPosition.z);
            trans1.localEulerAngles = new Vector3(0, 0, 180);
            trans1.localScale = Vector3.one;
 
        }

    */
        GameObject eff = Game.PoolManager.EffectPool.Spawn("gangUI_EF");
        eff.transform.position = endPos;
        Game.PoolManager.EffectPool.Despawn(eff, 3);

        anim.Play("PutTable");

    }

    public void PlayPeng(int card, bool isMy)
    {
        Game.SoundManager.PlayPeng(position);

        player.tableCardLayout.AddCard(card);
        player.tableCardLayout.AddCard(card);
        player.tableCardLayout.AddCard(card);

        if (isMy)
        {
            player.handCardLayout.RemoveCard(card);
            player.handCardLayout.RemoveCard(card);
        }
        else
        {
            int count = player.handCardLayout.HandCards.Count;
            int index = UnityEngine.Random.Range(0, count);
            int index2 = index - 1;
            if (index <= 0)
            {
                index2 = index + 1;
            }
            player.handCardLayout.RemoveCardAt(index);
            player.handCardLayout.RemoveCardAt(index2);
        }

        player.handCardLayout.LineUp();

        Game.MJMgr.targetFlag.gameObject.SetActive(false);
        Game.PoolManager.CardPool.Despawn(Game.MJMgr.LastDropCard.gameObject);
        //Game.MJMgr.LastDropCardPlayer.dropCardLayout.RemoveLast();

        EventDispatcher.DispatchEvent(MessageCommand.PlayEffect, position, "pengUI_EF");


        Transform tableCLTrans = player.tableCardLayout.transform;
        int childCount = tableCLTrans.childCount;
        Transform lastChild = tableCLTrans.GetChild(childCount - 1);

        Vector3 endPos = tableCLTrans.TransformPoint(lastChild.localPosition);
        hand.transform.position = endPos;
        anim.gameObject.SetActive(true);

        GameObject eff = Game.PoolManager.EffectPool.Spawn("peng_EF");
        eff.transform.position = endPos;
        Game.PoolManager.EffectPool.Despawn(eff, 2);

        anim.Play("PutTable");
    }

    internal void PlayChi(int card0, int card1, bool isMy)
    {
        Game.SoundManager.PlayChi(position);

        int card2 = Game.MJMgr.LastDropCard.Card;

        if (isMy)
        {
            player.handCardLayout.RemoveCard(card0);
            player.handCardLayout.RemoveCard(card1);
        }
        else
        {
            int count = player.handCardLayout.HandCards.Count;
            int index = UnityEngine.Random.Range(0, count);
            int index2 = index - 1;
            if (index <= 0)
            {
                index2 = index + 1;
            }
            player.handCardLayout.RemoveCardAt(index);
            player.handCardLayout.RemoveCardAt(index2);
        }

        player.handCardLayout.LineUp();

        player.tableCardLayout.AddCard(card0);
        player.tableCardLayout.AddCard(card2);
        player.tableCardLayout.AddCard(card1);

        Game.MJMgr.targetFlag.gameObject.SetActive(false);
        Game.PoolManager.CardPool.Despawn(Game.MJMgr.LastDropCard.gameObject);
        //Game.MJMgr.LastDropCardPlayer.dropCardLayout.RemoveLast();
        EventDispatcher.DispatchEvent(MessageCommand.PlayEffect, position, "chiUI_EF");

        Transform tableCLTrans = player.tableCardLayout.transform;
        int childCount = tableCLTrans.childCount;
        Transform lastChild = tableCLTrans.GetChild(childCount - 1);

        Vector3 endPos = tableCLTrans.TransformPoint(lastChild.localPosition);
        hand.transform.position = endPos;
        anim.gameObject.SetActive(true);

        GameObject eff = Game.PoolManager.EffectPool.Spawn("peng_EF");
        eff.transform.position = endPos;
        Game.PoolManager.EffectPool.Despawn(eff, 2);

        anim.Play("PutTable");
    }

    internal void PlayTing(bool isMy)
    {
        Game.SoundManager.PlayTing(position);
        Game.SoundManager.PlayTingSound();
        Game.MJMgr.MjData[position].player.ting = true;
        EventDispatcher.DispatchEvent(MessageCommand.MJ_UpdatePlayPage);
        EventDispatcher.DispatchEvent(MessageCommand.PlayEffect, position, "tingUI_EF");
    }

    public void Bao(int dice, int oldBao)
    {
        IsBusy = true;
        MJPlayer player = Game.MJMgr.GetPlayerByPosition(position);
        //Time.timeScale = 0.1f;
        bool hasOldBao = (oldBao != -1);
        if (hasOldBao)
        {
            HideBaoEffect();
            Game.SoundManager.PlayHuanBao(player.postion);
            int card = oldBao;
            //Game.SoundManager.PlayCardSound(card);
            if (player.baoRoot.childCount > 0)
            {
                Transform old = player.baoRoot.GetChild(0);
                Game.PoolManager.CardPool.Despawn(old.gameObject);
            }

            Transform dropCLTrans = player.dropCardLayout.transform;
            int childCount = dropCLTrans.childCount;

            anim.gameObject.SetActive(true);

            Vector3 toPos = player.dropCardLayout.GetLocalPos(childCount);
            Vector3 endPos = dropCLTrans.TransformPoint(toPos);

            hand.transform.position = endPos;

            GameObject child = Game.PoolManager.CardPool.Spawn(card.ToString());
            //Game.MJMgr.LastDropCard = child.GetComponent<MJEntity>();
            //Game.MJMgr.LastDropCard.Card = card;
            Transform cardTrans = child.transform;
            cardTrans.SetParent(handMJRoot);
            cardTrans.localPosition = Vector3.zero;
            cardTrans.localRotation = Quaternion.identity;
            cardTrans.localScale = Vector3.one;

            anim.Play("Drop");
            handAnima.OnDropCallback = () =>
            {
                Game.SoundManager.PlayDropCard();
                player.dropCardLayout.AddCard(card, child);
                MJCardGroup.TryDragCard(true);
                //MJCardGroup.DragBaoCard(dice);
                Game.Delay(0.5f, () => { PutBao(); });
            };
        }
        else
        {
            MJCardGroup.TryDragCard(true);
            //MJCardGroup.DragBaoCard();
            PutBao();
        }
    }

    void PutBao()
    {
        anim.gameObject.SetActive(true);
        Vector3 endPos = player.baoRoot.position;
        hand.transform.position = endPos;

        GameObject child = Game.PoolManager.CardPool.Spawn("Dragon_Blank");
        Transform cardTrans = child.transform;
        cardTrans.SetParent(handMJRoot);
        cardTrans.localPosition = Vector3.zero;
        cardTrans.localRotation = Quaternion.Euler(0, 0, 180);
        cardTrans.localScale = Vector3.one;

        anim.Play("PutBao");
        handAnima.OnPutBaoCallback = () =>
        {
            Game.SoundManager.PlayDropCard();
            child.transform.SetParent(player.baoRoot);
            child.transform.localPosition = Vector3.zero;
            child.transform.localRotation = Quaternion.identity;
            child.transform.localScale = Vector3.one;
            IsBusy = false;

            ShowBaoEffect();
        };
    }

    public void HideBaoEffect()
    {
        if (null != baoEffect)
        {
            baoEffect.SetActive(false);
        }
    }

    public void ShowBaoEffect()
    {
        if (null == baoEffect)
        {
            baoEffect = Game.PoolManager.EffectPool.Spawn("BaoEffect");
        }
        baoEffect.transform.position = player.baoRoot.position;
        baoEffect.SetActive(true);
    }

    public void PlayDize(int dice1, int dice2)
    {
        Game.MJTable.HideCountdown();

        anim.gameObject.SetActive(true);

        hand.transform.position = dizePos.position;
        anim.Play("Dize");
        handAnima.OnDiceCallback = () =>
        {
            Game.SoundManager.PlayDizeSound();
            Game.MJTable.Dice(dice1, dice2);
        };

    }
}