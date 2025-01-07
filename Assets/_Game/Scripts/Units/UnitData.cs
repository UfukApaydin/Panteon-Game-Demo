using System;
using UnityEngine;


namespace Game.Unit
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/Units/UnitData")]
    public class UnitData : Data
    {
        [Header("Config")]
       // public string dataName;
        public int health;
        public float speed;
        public int attackDamage;
        public float attackSpeed;
        public float attackRange;
   

        [Header("Visuals")]
        public GameObject prefab;
        public Sprite rankVisual;
       // public Texture icon;

        public Action<UnitData> BuildUnitGlobally;
    }
}