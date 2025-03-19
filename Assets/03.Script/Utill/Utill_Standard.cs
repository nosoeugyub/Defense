using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class Utill_Standard 
{

    //현재 등급의 다음 등급 가져오기 
   public static Utill_Enum.Unit_Grade UnitNextGrade(Utill_Enum.Unit_Grade currentGarde)
    {
        // Enum을 배열로 변환하여 현재 등급의 인덱스 찾기
        Utill_Enum.Unit_Grade[] grades = (Utill_Enum.Unit_Grade[])Enum.GetValues(typeof(Utill_Enum.Unit_Grade));
        int currentIndex = Array.IndexOf(grades, currentGarde);

        // 마지막 등급이면 그대로 반환
        if (currentIndex < 0 || currentIndex >= grades.Length - 1)
            return currentGarde;

        // 다음 등급 반환
        return grades[currentIndex + 1];
    }

 
}
