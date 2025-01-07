using System;
using UnityEngine;
using UnityEngine.U2D;


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
        public string spriteName;


        [Header("Visuals")]
        public GameObject prefab;
        public Sprite visual;
        public SpriteAtlas spriteAtlas;
        // public Texture icon;

        public Action<UnitData> BuildUnitGlobally;

        private void OnValidate()
        {
            if (visual != null)
            {
                spriteName = visual.name;
            }
        }

        public Sprite GetSpriteFromAtlas()
        {
            if(spriteAtlas != null && !string.IsNullOrEmpty(spriteName))
            {
                return spriteAtlas.GetSprite(spriteName);
            }
            Debug.LogError($" {name} sprite atlas or sprite name cannot found ");
            return null;

        }
    }
}