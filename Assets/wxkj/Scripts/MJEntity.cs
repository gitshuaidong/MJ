using UnityEngine;
using System.Collections;

public class MJEntity : MonoBehaviour {
    private static int id = 0;
    private int cardId;
    public int CardId
    {
        get
        {
            return cardId;
        }
    }

    public int Card
    {
        get
        {
            int card = cardId / 100000;
            return card;
        }

        set
        {
            id++;
            cardId = value * 100000 + id;
        }
    }


    void OnMouseDown()
    {
        if (!Game.MJMgr.HangUp)
        {
            if (MJUtils.DropCard())
            {
                if (IsMine() && IsHandCard())
                {
                    OnClickDrop();
                }
            }
        }
    }

    /// <summary>
    /// 根据本地座位index == 0 判断是否为手牌
    /// </summary>
    /// <returns></returns>
    bool IsMine()
    {
        MJPlayer player = this.GetComponentInParent<MJPlayer>();

        //lvlinsong index为本地chairid序号 0为自身的座位
        if (null != player && player.index == 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 根据父节点是否有HandCardLayout脚本判断是否是手牌，
    /// </summary>
    /// <returns></returns>
    bool IsHandCard()
    {
        HandCardLayout layout = this.GetComponentInParent<HandCardLayout>();
        if (null != layout)
        {
            return true;
        }
        return false;
    }

    void OnClickDrop()
    {
        if (RoomMgr.actionNotify.tingList.Count <= 0 || RoomMgr.IsTingDropCard(Card))
        {
            Game.MJMgr.MyDropMJEntity = this;

            //Game.MJMgr.MyPlayer.DropCard(Card);
            Game.SocketGame.DoDropCard(Card);
            Game.MaterialManager.TurnOnHandCard();

            EventDispatcher.DispatchEvent(MessageCommand.MJ_UpdatePlayPage);
        }
    }
}
