using UnityEngine;
using System.Collections;

public class CreateRoomDialog : CreateRoomDialogBase
{
    public override void InitializeScene()
    {
        base.InitializeScene();
        detail.CloseButton_Button.onClick.AddListener(OnBackPressed);
        detail.CreateButton_Button.onClick.AddListener(OnClickCreate);
    }

    private void OnClickCreate()
    {
        Game.SoundManager.PlayClick();
        bool is2Player = detail.PlayerNum2_CheckBoxSub.IsSelected;
        int vipRoomType = is2Player ? 2 : 4;
        int quanNum = detail.Round4_CheckBoxSub.IsSelected ? 4 : 8;
        int wanfa = 0;
        if (detail.Mode0_CheckBoxSub.IsSelected)
        {
            wanfa = wanfa | MJUtils.MODE_ZHA;
        }
        if (detail.Mode1_CheckBoxSub.IsSelected)
        {
            wanfa = wanfa | MJUtils.MODE_37JIA;
        }
        if (detail.Mode2_CheckBoxSub.IsSelected)
        {
            wanfa = wanfa | MJUtils.MODE_ZHIDUI;
        }
        if (detail.Mode3_CheckBoxSub.IsSelected)
        {
            wanfa = wanfa | MJUtils.MODE_DANDIAOJIA;
        }
        if (detail.Mode4_CheckBoxSub.IsSelected)
        {
            wanfa = wanfa | MJUtils.MODE_DAFENG;
        }
        if (detail.Mode5_CheckBoxSub.IsSelected)
        {
            wanfa = wanfa | MJUtils.MODE_HONGZHONG;
        }
        if (detail.Mode6_CheckBoxSub.IsSelected)
        {
            wanfa = wanfa | MJUtils.MODE_DAILOU;
        }
        if (detail.Mode7_CheckBoxSub.IsSelected)
        {
            wanfa = wanfa | MJUtils.MODE_JIAHU;
        }

        Game.SocketGame.DoCreateVipRoom(vipRoomType, quanNum, wanfa, (result) =>
        {
            OnBackPressed();
            // 根据用户权限来决定不同逻辑
            if (!Game.Instance.createMultiRoom)
            {
                if (result.roomList.Count > 0)
                {
                    Game.SocketGame.DoEnterVipRoom(result.roomList[0].code);
                }
            }
        });
    }
}
