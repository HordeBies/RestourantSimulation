using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Meal_",menuName ="Bies/Meal")]
public class Meal : ScriptableObject
{
    public string MealName;
    public int Servings;
    public float PrepTime;
    public Transform prefab;
    public Sprite Icon;
}