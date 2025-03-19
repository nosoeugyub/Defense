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
        Normal, //³ë¸»
        Rare, //Èñ±Í
        Hero, //¿µ¿õ
        Myth //½ÅÈ­
    }

    public enum Unit_Type
    {
        Humon, //ÀÎ°£
        Soul, //Á¤·É
        Robot, //·Îº¿
        Animal, //µ¿¹°
        Devil//¾Ç¸¶
    }

    public enum Unit_AtkType
    {
        Melee, //±ÙÁ¢
        Range // ¿ø°Å¸®
    }



}
