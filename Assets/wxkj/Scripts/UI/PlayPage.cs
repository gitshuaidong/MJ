﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using packet.mj;
using System;

public class PlayPage : PlayPageBase {
    private List<PlayerSub> players = new List<PlayerSub>();
    
    public override void InitializeScene ()
	{
		base.InitializeScene ();

        players.Add(detail.PlayerSub0_PlayerSub);
        players.Add(detail.PlayerSub1_PlayerSub);
        players.Add(detail.PlayerSub2_PlayerSub);
        players.Add(detail.PlayerSub3_PlayerSub);
        
        detail.ExitButton_Button.onClick.AddListener (OnBackPressed);
        detail.DismissButton_Button.onClick.AddListener(OnClickDismissBtn);
        detail.SettingButton_Button.onClick.AddListener(OnClickSettingBtn);
        detail.GameRoundButton_Button.onClick.AddListener(OnClickRecord);

        detail.PopupButton_Button.onClick.AddListener (OnClickOpenPopupMenu);
		detail.ClosePopButton_Button.onClick.AddListener (OnClickClosePopupMenu);
		//detail.DropButton_Button.onClick.AddListener (OnClickDropBtn);

		detail.PassButton_Button.onClick.AddListener (OnClickPassBtn);
		detail.ChiButton_Button.onClick.AddListener (OnClickChiBtn);
		detail.PengButton_Button.onClick.AddListener (OnClickPengBtn);
        detail.TingButton_Button.onClick.AddListener(OnClickTingBtn);
        detail.TingChiButton_Button.onClick.AddListener(OnClickTingChiBtn);
        detail.TingPengButton_Button.onClick.AddListener(OnClickTingPengBtn);
        detail.ZhiduiButton_Button.onClick.AddListener(OnClickZhiduiBtn);

        detail.CancelButton_Button.onClick.AddListener (OnClickCancelBtn);
        detail.CancelHangUpBtn_Button.onClick.AddListener(OnClickCancelHangUpBtn);
        detail.DumpBtn_Button.onClick.AddListener(OnClickDumpBtn);
        detail.GangButton_Button.onClick.AddListener (OnClickGangBtn);
        //detail.HuButton_Button.onClick.AddListener(OnClickHuBtn);
        //detail.StartButton_Button.onClick.AddListener (OnClickStartBtn);
        //detail.ReadyButton_Button.onClick.AddListener(OnClickReadyBtn);
        //detail.ReadyCancelButton_Button.onClick.AddListener(OnClickReadyCancelBtn);

        detail.ChatButton_Button.onClick.AddListener(OnClickChat);
        detail.HostedButton_Button.onClick.AddListener(OnClickHostedBtn);

        EventTriggerListener.Get(detail.VoiceButton_Button.gameObject).onDown += OnVoiceButtonDown;
        EventTriggerListener.Get(detail.VoiceButton_Button.gameObject).onUp += OnVoiceButtonUp;

        detail.RecodState_UIItem.gameObject.SetActive(false);
        detail.WXButton_Button.gameObject.SetActive(false);
        detail.WXButton_Button.onClick.AddListener(OnClickWXBtn);
        detail.DropButton_Button.gameObject.SetActive(false);
        detail.SelectPanel_UIItem.gameObject.SetActive(false);
    }

    private void OnClickRecord()
    {
        Game.SoundManager.PlayClick();
        Game.SocketHall.DoRoomResult(0, (response) =>
        {
            Game.UIMgr.PushScene(UIPage.TotalRecrodPage, response);
        });
    }

    private void OnClickDumpBtn()
    {
        Game.SocketGame.DoDump();
    }

    private void OnClickCancelHangUpBtn()
    {
        Game.SoundManager.PlayClick();

        Game.SocketGame.DoHangUpRequest(false);
    }

    private void OnClickChat()
    {
        //Game.SoundManager.PlayClick();
        Game.UIMgr.PushScene(UIPage.MoodPage);
    }

    public override void OnSceneOpened(params object[] sceneData)
    {
        base.OnSceneOpened(sceneData);

        EventDispatcher.AddEventListener(MessageCommand.MJ_UpdatePlayPage, SetupUI);
        //EventDispatcher.AddEventListener(MessageCommand.OnEnterRoom, SetupUI);
        EventDispatcher.AddEventListener(MessageCommand.MJ_UpdateCtrlPanel, OnUpdateCtrlPanel);
        EventDispatcher.AddEventListener(MessageCommand.Chat, OnChat);
        EventDispatcher.AddEventListener(MessageCommand.PlayEffect, OnPlayEffect);
    }

    public override void OnSceneClosed()
    {
        base.OnSceneClosed();
        EventDispatcher.RemoveEventListener(MessageCommand.MJ_UpdatePlayPage, SetupUI);
        //EventDispatcher.RemoveEventListener(MessageCommand.OnEnterRoom, SetupUI);
        EventDispatcher.RemoveEventListener(MessageCommand.MJ_UpdateCtrlPanel, OnUpdateCtrlPanel);
        EventDispatcher.RemoveEventListener(MessageCommand.Chat, OnChat);
        EventDispatcher.RemoveEventListener(MessageCommand.PlayEffect, OnPlayEffect);
    }

    public override void OnBackPressed()
    {
        Game.SoundManager.PlayClick();
        if (RoomMgr.IsSingeRoom())
        {
            Game.SocketGame.DoExitGameRequest();
        }
        else if(RoomMgr.IsVipRoom())
        {
            if (Game.Instance.state == GameState.Playing)
            {
                Action<bool> callback = (isOk) =>
                {
                    if (isOk)
                    {
                   //     Game.SocketGame.DoAwayGameRequest();
						Game.SocketGame.DoExitGameRequest();
						Debug.LogError("****************************************************" );
                        //Game.UIMgr.PushScene(UIPage.MainPage);
                    }
                };
                Game.DialogMgr.PushDialog(UIDialog.DoubleBtnDialog, "已经开局！确定要退出吗？", "提示", callback);
            }
            else
            {
                Action<bool> callback = (isOk) =>
                {
                    if (isOk)
                    {
                        Game.SocketGame.DoExitGameRequest();
                    }
                    else
                    {
                        Game.SocketGame.DoAwayGameRequest();
                        //Game.UIMgr.PushScene(UIPage.MainPage);
                    }
                };
                Game.DialogMgr.PushDialog(UIDialog.DoubleBtnDialog, "你即将返回大厅，需要同时离开桌子吗？", "提示", callback);
            }
        }
        else
        {
            if (Game.Instance.state == GameState.Playing)
            {
                Action<bool> callback = (isOk) =>
                {
                    if (isOk)
                    {
                        Game.SocketGame.DoExitGameRequest();
                    }
                };
                Game.DialogMgr.PushDialog(UIDialog.DoubleBtnDialog, "已经开局，退出将自动开启托管模式！确定要退出吗？", "提示", callback);
            }
            else
            {
                Action<bool> callback = (isOk) =>
                {
                    if (isOk)
                    {
                        Game.SocketGame.DoExitGameRequest();
                    }
                    else
                    {
                        Game.SocketGame.DoAwayGameRequest();
                        //Game.UIMgr.PushScene(UIPage.MainPage);
                    }
                };
                Game.DialogMgr.PushDialog(UIDialog.DoubleBtnDialog, "你即将返回大厅，需要同时离开桌子吗？", "提示", callback);
            }
        }
    }

    private void OnClickDismissBtn()
    {
        Game.SoundManager.PlayClick();
        if (RoomMgr.IsVipRoom())
        {
            Action<bool> callback = (ok) => {
                if (ok)
                {
                    Game.SocketGame.DoDissmissVoteSyn(true);
                }
            };
            Game.DialogMgr.PushDialog(UIDialog.DoubleBtnDialog, "确定申请解散房间吗？", "提示", callback);
        }
    }

    void OnClickSettingBtn()
    {
        Game.SoundManager.PlayClick();
        Game.UIMgr.PushScene(UIPage.SettingPage);
    }

    void OnClickOpenPopupMenu(){
        Game.SoundManager.PlayClick();
        detail.PopupMenu_UIItem.gameObject.SetActive (true);
		detail.PopupButton_Button.gameObject.SetActive (false);
	}

	void OnClickClosePopupMenu(){
        Game.SoundManager.PlayClick();
        detail.PopupMenu_UIItem.gameObject.SetActive (false);
		detail.PopupButton_Button.gameObject.SetActive (true);
	}

	public override void OnSceneActivated (params object[] sceneData)
	{
		base.OnSceneActivated (sceneData);

        //Game.Instance.state = GameState.Playing;

        detail.PopupMenu_UIItem.gameObject.SetActive(false);
        detail.PopupButton_Button.gameObject.SetActive(true);

        SetupUI();
    }

    void SetupUI(params object[] args)
    {
        detail.HangUp_UIItem.gameObject.SetActive(Game.MJMgr.HangUp);
        detail.VoiceButton_Button.gameObject.SetActive(RoomMgr.IsVipRoom());
        detail.HostedButton_Button.gameObject.SetActive(RoomMgr.IsNormalRoom());

        if (RoomMgr.IsVipRoom())
        {
            detail.DismissButton_Button.gameObject.SetActive(true);
            detail.GameRoundButton_Button.gameObject.SetActive(true);
            int quanNum = RoomMgr.GetQuanNum();
            int totalQuan = RoomMgr.GetTotalQuan();
            detail.GameRoundText_Text.text = string.Format("{0}/{1}{2}",quanNum,totalQuan, RoomMgr.IsVip2Room()?"局":"圈");

            bool isWaitting = Game.Instance.state == GameState.Waitting;
            detail.WXButton_Button.gameObject.SetActive(isWaitting);
        }
        else
        {
            detail.GameRoundButton_Button.gameObject.SetActive(false);
            detail.DismissButton_Button.gameObject.SetActive(false);
            detail.WXButton_Button.gameObject.SetActive(false);
        }

        OnUpdateCtrlPanel();

        foreach (PlayerSub sub in players)
        {
            sub.gameObject.SetActive(false);
        }

        for (int i = 0; i < Game.MJMgr.MjData.Length; i++)
        {
            int position = i;
            MjData data = Game.MJMgr.MjData[position];
            if(null != data.player)
            {
                int index = Game.MJMgr.GetIndexByPosition(position);
                players[index].SetValue(data);
            }
        }

        //MJPlayer self = Game.MJMgr.MyPlayer;
        //MjData selfData = Game.MJMgr.MjData[self.postion];
        //Player selfPlayer = selfData.player;
        //bool isReady = null != selfPlayer && selfPlayer.isReady;
        //detail.StartButton_Button.gameObject.SetActive(!isReady);
        //detail.StartButton_Button.gameObject.SetActive(false);
        //detail.ReadyCancelButton_Button.gameObject.SetActive(false);
        //detail.ReadyButton_Button.gameObject.SetActive(false);
    }

    //void OnClickStartBtn()
    //{
    //    Game.SocketGame.DoREADYL();
    //    //Game.StateMachine.SetNext(Game.StateMachine.PlayState);

    //    //detail.StartButton_Button.gameObject.SetActive(false);
    //}

    //void OnClickReadyBtn()
    //{
    //    Game.SocketGame.DoREADYL();
    //}

    //void OnClickReadyCancelBtn()
    //{

    //}
    //这里处理杠按钮的点击事件
    void OnClickGangBtn()
    {
        Game.MJMgr.MyPlayer.Gang();
        detail.CtrlPanel_UIItem.gameObject.SetActive(false);
    }

    //void OnClickHuBtn()
    //{
    //    Game.MJMgr.MyPlayer.Hu();
    //    detail.CtrlPanel_UIItem.gameObject.SetActive(false);

    //    //		Game.SoundManager.PlayHu ();
    //}
    
    void OnUpdateCtrlPanel(params object[] args)
    {
        bool chu = MJUtils.DropCard();
        bool chi = MJUtils.Chi();
        bool peng = MJUtils.Peng();
        //bool hu = false;// MJUtils.CanChi(actions);
        bool minggang = MJUtils.MinGang();
        bool bugang = MJUtils.BuGang();
        bool angang = MJUtils.AnGang();
        bool gang = angang || minggang || bugang;

        bool ting = MJUtils.Ting();
        bool tingChi = MJUtils.TingChi();
        bool tingPeng = MJUtils.TingPeng();
        bool tingZhidui = MJUtils.TingZhidui();

        //bool showPanel = (!Game.Instance.Ting)&& (chi || peng || ting || tingChi || tingPeng || tingZhidui);
        bool showPanel = (chi || peng || ting || tingChi || tingPeng || tingZhidui || gang);
  /*      Debug.LogError("minggang:" + minggang);
        Debug.LogError("angang:" + angang);
        Debug.LogError("(!Game.Instance.Ting)&& (chi || peng || ting || tingChi || tingPeng || tingZhidui):" + ((!Game.Instance.Ting) && (chi || peng || ting || tingChi || tingPeng || tingZhidui)));
        Debug.LogError("showPanel:" + showPanel);
        Debug.LogError("gang:" + gang);
*/
        detail.CtrlPanel_UIItem.gameObject.SetActive(showPanel);
        detail.ChiButton_Button.gameObject.SetActive(chi);
        detail.PengButton_Button.gameObject.SetActive(peng);
        detail.GangButton_Button.gameObject.SetActive(gang);
        detail.DropButton_Button.gameObject.SetActive(chu/* || ting*/);
        detail.TingButton_Button.gameObject.SetActive(ting);
        detail.HuButton_Button.gameObject.SetActive(false);
        detail.TingChiButton_Button.gameObject.SetActive(tingChi);
        detail.TingPengButton_Button.gameObject.SetActive(tingPeng);
        detail.ZhiduiButton_Button.gameObject.SetActive(tingZhidui);

        if (chu)
        {
            Game.MaterialManager.TurnOffHandCard();
        }
    }

    void OnClickPassBtn(){
        Game.SoundManager.PlayClick();
        //EventDispatcher.DispatchEvent (MessageCommand.MJ_Pass);
        detail.CtrlPanel_UIItem.gameObject.SetActive(false);
        detail.SelectPanel_UIItem.gameObject.SetActive(false);
        Game.SocketGame.DoPass();
        Game.MaterialManager.TurnOnHandCard();
        //detail.DropButton_Button.gameObject.SetActive(true);
    }

	void OnClickChiBtn(){
        Game.SoundManager.PlayClick();

        if (MJUtils.Chi() || MJUtils.TingChi())
        {
            List<GameOperChiArg> list = RoomMgr.actionNotify.chiArg;
			//List<int[]> list = Game.MJMgr.MyPlayer.GetChiList (Game.MJMgr.lastDropCard);
			if (null != list && list.Count > 0) {
				detail.CtrlPanel_UIItem.gameObject.SetActive (false);

				if (list.Count == 1) {
                    GameOperChiArg chiArg = list[0];

                    Game.MJMgr.MyPlayer.Chi (chiArg);
					//Game.SoundManager.PlayChi ();
                    //detail.DropButton_Button.gameObject.SetActive(true);
                } else {
                    ClearSelectPanel();
                    detail.SelectPanel_UIItem.gameObject.SetActive (true);
					List<int> sortTemp = new List<int> ();
                    for (int i = list.Count - 1; i >= 0; i--)
                    //for (int i = 0; i < list.Count; i++) 
                    {
                        GameOperChiArg chiArg = list[i];

                        GameObject selectGroup = PrefabUtils.AddChild (detail.SelectRoot_HorizontalLayoutGroup, detail.GroupButton_Button.gameObject);
						selectGroup.SetActive (true);

						sortTemp.Clear ();
						sortTemp.Add (chiArg.myCard1);
						sortTemp.Add (chiArg.myCard2);
						sortTemp.Add (chiArg.targetCard);
						sortTemp.Sort ();

						foreach (int card in sortTemp) {
							GameObject card0 = Game.PoolManager.MjPool.Spawn (card.ToString ());
							card0.transform.SetParent (selectGroup.transform);
							card0.transform.localScale = Vector3.one;
							card0.transform.localRotation = Quaternion.identity;
						}

						Button btn = selectGroup.GetComponent<Button> ();
						btn.onClick.AddListener (() => {
							Game.MJMgr.MyPlayer.Chi (chiArg);
                            detail.SelectPanel_UIItem.gameObject.SetActive(false);
                            //detail.DropButton_Button.gameObject.SetActive(true);
                        });
					}
				}
			}
		}
	}

	void ClearSelectPanel(){
		for (int a = 0; a < detail.SelectRoot_HorizontalLayoutGroup.transform.childCount; a++) {
			Transform sGroup = detail.SelectRoot_HorizontalLayoutGroup.transform.GetChild (a);
			for (int b = 0; b < sGroup.childCount; b++) {
				Transform sCard = sGroup.GetChild (b);
				Game.PoolManager.MjPool.Despawn (sCard);
			}
		}

		PrefabUtils.ClearChild (detail.SelectRoot_HorizontalLayoutGroup);
        detail.SelectPanel_UIItem.gameObject.SetActive(false);
    }

	void OnClickPengBtn(){
        Game.SoundManager.PlayClick();

        Game.MJMgr.MyPlayer.Peng ();
		detail.CtrlPanel_UIItem.gameObject.SetActive (false);

        //detail.DropButton_Button.gameObject.SetActive(true);
    }

    void OnClickTingBtn()
    {
        Game.SoundManager.PlayClick();

        bool ting = MJUtils.Ting();
        //bool tingChi = MJUtils.TingChi();
        //bool tingPeng = MJUtils.TingPeng();
        //bool tingZhidui = MJUtils.TingZhidui();

        //bool t = ting || tingChi || tingPeng;
        if (ting)
        {
            Game.SocketGame.DoTing();
            Game.Instance.Ting = true;
            detail.CtrlPanel_UIItem.gameObject.SetActive(false);
        }
    }

    void OnClickCancelBtn(){
        Game.SoundManager.PlayClick();
        //RoomMgr.actionNotify.actions = 0;
        detail.SelectPanel_UIItem.gameObject.SetActive (false);
        Game.Instance.Ting = false;
        ClearSelectPanel();

		OnClickPassBtn ();
	}

    void OnClickTingChiBtn()
    {
        Game.SoundManager.PlayClick();

        Game.Instance.Ting = true;
        if (MJUtils.TingChi())
        {
            OnClickChiBtn();
        }
    }
    void OnClickTingPengBtn()
    {
        Game.SoundManager.PlayClick();
        Game.Instance.Ting = true;
        if (MJUtils.TingPeng())
        {
            OnClickPengBtn();
        }
    }

    void OnClickZhiduiBtn()
    {
        Game.SoundManager.PlayClick();
        if (MJUtils.TingZhidui())
        {
            ClearSelectPanel();

            List<int> list = RoomMgr.actionNotify.tingDzs;
            if (null != list && list.Count > 0)
            {
                detail.CtrlPanel_UIItem.gameObject.SetActive(false);
                detail.SelectPanel_UIItem.gameObject.SetActive(true);

                for (int i = 0; i < list.Count; i++)
                {
                    int card = list[i];

                    GameObject selectGroup = PrefabUtils.AddChild(detail.SelectRoot_HorizontalLayoutGroup, detail.GroupButton_Button.gameObject);

                    GameObject card0 = Game.PoolManager.MjPool.Spawn(card.ToString());
                    card0.transform.SetParent(selectGroup.transform);
                    card0.transform.localScale = Vector3.one;
                    card0.transform.localRotation = Quaternion.identity;

                    Button btn = selectGroup.GetComponent<Button>();
                    btn.onClick.AddListener(() =>
                    {
                        Game.MJMgr.MyPlayer.Zhidui(card);
                        detail.SelectPanel_UIItem.gameObject.SetActive(false);
                        EventDispatcher.DispatchEvent(MessageCommand.MJ_UpdatePlayPage);
                        //Game.SoundManager.PlayChi();
                        //detail.DropButton_Button.gameObject.SetActive(true);
                    });
                }
            }
        }
    }

    private void OnChat(object[] objs)
    {
        int position = (int)objs[0];
        int idx = Game.MJMgr.GetIndexByPosition(position);
        PlayerSub player = players[idx];

        int type = (int)objs[1];
        switch (type)
        {
            case 0:
                int index = (int)objs[2];
                player.ShowMood(index);
                break;
            case 1:
                int id = (int)objs[2];
                ConfigWord config = ConfigWord.GetByKey(id);
                if(null != config)
                {
                    player.ShowWord(config.TextContent);
                    Game.SoundManager.PlayVoice(position,config.Talk);
                }
                break;
            case 2:
                string str = (string)objs[2];
                player.ShowWord(str);
                break;
            case 3:
                byte[] data = (byte[])objs[2];
                player.ShowVoice(data,detail.AudioSource_MicrophoneInput);
                break;
        }
    }

    private Coroutine countDownCoroutine;
    private void OnVoiceButtonDown(GameObject go)
    {
        Game.SoundManager.PlayClick();

        detail.RecodState_UIItem.gameObject.SetActive(true);
        detail.AudioSource_MicrophoneInput.StartRecord((data)=> {
            Game.StopDelay(countDownCoroutine);
            detail.RecodState_UIItem.gameObject.SetActive(false);
            Game.Delay(0.1f, () => { Game.SocketGame.DoGameChatMsgRequest(data); });
        });

        detail.CountDown_Text.text = MicrophoneInput.RECORD_TIME.ToString();

        Game.StopDelay(countDownCoroutine);
        countDownCoroutine = Game.DelayLoop(1, () =>
        {
            int leftTime = MicrophoneInput.RECORD_TIME - detail.AudioSource_MicrophoneInput.curTime;
            detail.CountDown_Text.text = leftTime.ToString();
        });
    }

    private void OnVoiceButtonUp(GameObject go)
    {
        detail.RecodState_UIItem.gameObject.SetActive(false);
        Game.Delay(1,()=> {
            detail.AudioSource_MicrophoneInput.EndRecord();
        });
    }

    void Update()
    {
        if (Application.isPlaying)
        {
            detail.CardNum_Text.text = Game.MJMgr.CardLeft.ToString();
            detail.Time_Text.text = System.DateTime.Now.ToString("HH:mm");
     //       Debug.LogWarningFormat("card：{0}", Game.MJMgr.CardLeft.ToString());
        }
    }

    void OnClickHostedBtn()
    {
        Game.SoundManager.PlayClick();
        detail.PopupMenu_UIItem.gameObject.SetActive(false);
        detail.PopupButton_Button.gameObject.SetActive(true);
        if (!Game.MJMgr.HangUp)
        {
            Game.SocketGame.DoHangUpRequest(true);
        }
    }

    private void OnPlayEffect(object[] objs)
    {
        int position = (int)objs[0];
        int index = Game.MJMgr.GetIndexByPosition(position);
        string name = (string)objs[1];
        GameObject eff = Game.PoolManager.EffectPool.Spawn(name);
        eff.transform.SetParent(detail.EffectPos_EffectPos.pos[index].transform);
        eff.transform.localPosition = Vector3.zero;
        eff.transform.localScale = Vector3.one;
        Game.PoolManager.EffectPool.Despawn(eff,2);
    }

    void OnClickWXBtn()
    {
        string deskId = null;
        if (RoomMgr.playerGamingSyn != null)
        {
            deskId = RoomMgr.playerGamingSyn.deskId;
        }
        //Game.AndroidUtil.Share(deskId);
        MyShareSDK.MyshareSDK.Share(deskId);

    }
}
