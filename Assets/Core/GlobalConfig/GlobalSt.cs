using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 
 *   游戏中的结构体集合
 *   
 * 
 * 
 * */

namespace LollipopUnity.GST
{
    public class GlobalSt
    {
        public void Test()
        {
            Debug.Log("啊啊啊啊啊啊啊啊啊啊啊啊啊");
        }
    }
        // 玩家结构
      public  struct UserInfo
        {
            int RoleUid;
            string RoleName;
            int RoleAvater;
            int RoleLev;
            int RoleSex;
            float Coin;
            float Diamond;
            int ChannelId;
            ItemSt[] itemst;
            CardInfo[] cardinfo;
        };

    // 道具结构
    public struct ItemSt
        {
            int FunctionId;
            ItemData[] itemdata;
        };

    // 道具基础结构
    public struct ItemData
        {
            int ItemId;
            int ItemNum;
        }

    // 卡牌结构
    public struct CardInfo
        {
            int CardId;
            int Level;
            string RoleUid;
            SkillInfo[] skillinfo;
            EquipSt[] equipst;
            AttributeInfo[] attributeInfo;
            int Stars;
            bool IsShow;
        }

    // 技能结构
    public struct SkillInfo
        {
            int SkillId;
            int SkillLev;
            int Position;  // 上阵位置 0 1 2 ； -1 表示没有使用
        }

    // 装备结构
    public struct EquipSt
        {
            int EquipId;
            int Quality;
            int Star;
        }

    // 基础属性
    public struct AttributeInfo
        {
            int BattlePower;
            int HPPower;
            int AttackPower;
            int DefensePower;
        }

    // 邮件结构
    public struct MailSt
    {
        int MailId;
        string MailTitle;
        string Sender;
        string Content;
    }
    
}
