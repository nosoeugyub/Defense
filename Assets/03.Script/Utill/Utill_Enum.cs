using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utill_Enum
{
    public enum Game_sequence
    {
        DataLoad,
        Deley,
        Start,
        Stop,
    }

    public enum Unit_Grade
    {
        Normal, //�븻
        Rare, //���
        Hero, //����
        Myth //��ȭ
    }

    public enum Unit_Type
    {
        Humon, //�ΰ�
        Soul, //����
        Robot, //�κ�
        Animal, //����
        Devil//�Ǹ�
    }

    public enum Unit_AtkType
    {
        Melee, //����
        Range // ���Ÿ�
    }



}
