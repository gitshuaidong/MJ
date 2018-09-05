using UnityEngine;
using System.Collections.Generic;

public class TableCardLayout : MonoBehaviour
{
    //时间 2018年9月4日
    public int distance = 0;
    public float disWidth = 0.015f;

    //-------------->
    public float width = 7.1f;
    public float height = 5.02f;
    public int row = 2;
    public int col = 18;
    public GameObject last = null;
    public List<int> TableCards = new List<int>();
    //public List<int> GangCards = new List<int>();
    public List<GameObject> TableCardsGO = new List<GameObject>();
    public List<GameObject> GangCards = new List<GameObject>();

    private void LineUp()
    {
        for (int j = 0; j < row; j++)//列 1
        {
            for (int i = 0; i < TableCardsGO.Count; i++)//12
            {

                int index = j * TableCardsGO.Count + i;
                if (index < this.TableCardsGO.Count)
                {
                    Transform trans = TableCardsGO[i].transform;
                    trans.localPosition = Vector3.right * width * i + Vector3.forward * height * j + Vector3.right * disWidth*distance;
                    trans.localRotation = Quaternion.identity;
                    trans.localScale = Vector3.one;
                    if ((index + 1) % 3 == 0)
                    {
                        distance += 1;
                    }
                }
            }

            for (int i = 0; i < GangCards.Count; i++)
            {
                int _nm = 0;
                for (int k = 0; k < this.transform.childCount; k++)
                {
                    if (this.transform.GetChild(k).name == GangCards[i].name.ToString())
                    {
                        _nm = k + 1;
                        Transform trans = GangCards[i].transform;
                        trans.localPosition = this.transform.GetChild(_nm).transform.localPosition + Vector3.up * height * 0.5f;
                        trans.localRotation = Quaternion.identity;
                        trans.localScale = Vector3.one;
                        break;
                    }
                }
            }
        }
    }

    public void Clear()
    {
        while (this.transform.childCount > 0)
        {
            Transform child = this.transform.GetChild(0);
            Game.PoolManager.CardPool.Despawn(child.gameObject);
        }

        TableCardsGO.Clear();
        GangCards.Clear();
        TableCards.Clear();
        distance = 0;
    }

    //普通吃碰杠
    public void AddCard(int card)
    {
        GameObject child = Game.PoolManager.CardPool.Spawn(card.ToString());
        if(null == child)
        {
            Debug.LogWarningFormat("AddCard error card:{0}", card);
            return;
        }
        child.transform.SetParent(this.transform);
        child.transform.localScale = Vector3.one;
        child.transform.localRotation = Quaternion.identity;
        
        TableCardsGO.Add(child);

        LineUp();

        TableCards.Add(card);
        last = child;
    }

    //杠 - 第四张牌处理
    public void ChangeCard(int card)
    {
        GameObject child = Game.PoolManager.CardPool.Spawn(card.ToString());
        if (null == child)
        {
            Debug.LogWarningFormat("AddCard error card:{0}", card);
            return;
        }
        child.transform.SetParent(this.transform);
        child.transform.localScale = Vector3.one;
        child.transform.localRotation = Quaternion.identity;

        GangCards.Add(child);

        LineUp();

        TableCards.Add(card);
        last = child;
    }
}