using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Meal_",menuName ="Bies/Meal")]
public class Meal : ScriptableObject
{
    public string MealName;
    public int Servings;
    public float PrepTime;
    public int PrepPrice;
    public int PortionPrice;
    public int requiredLevel;
    public Transform prefab;
    public Sprite Icon;
    public string PrepTimeFormatted => System.TimeSpan.FromSeconds(PrepTime).ToString("mm':'ss");
}
