using UnityEngine;


namespace Game.Unit
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/Units/UnitData")]
    public class UnitData : ScriptableObject
    {
        [Header("Config")]
        public string unitName;
        public int health;
        public float speed;
        public float damage;
        public float attackSpeed;
   

        [Header("Visuals")]
        public GameObject prefab;
        public Sprite viusual;
    }
}