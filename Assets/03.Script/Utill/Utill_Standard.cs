using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class Utill_Standard 
{

    //���� ����� ���� ��� �������� 
   public static Utill_Enum.Unit_Grade UnitNextGrade(Utill_Enum.Unit_Grade currentGarde)
    {
        // Enum�� �迭�� ��ȯ�Ͽ� ���� ����� �ε��� ã��
        Utill_Enum.Unit_Grade[] grades = (Utill_Enum.Unit_Grade[])Enum.GetValues(typeof(Utill_Enum.Unit_Grade));
        int currentIndex = Array.IndexOf(grades, currentGarde);

        // ������ ����̸� �״�� ��ȯ
        if (currentIndex < 0 || currentIndex >= grades.Length - 1)
            return currentGarde;

        // ���� ��� ��ȯ
        return grades[currentIndex + 1];
    }

 
}
