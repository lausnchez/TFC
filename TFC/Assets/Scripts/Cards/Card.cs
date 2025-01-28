using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TFC
{
    [CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
    public class Card : ScriptableObject
    {
        // Propiedades de las cartas
        public string cardName;
        public string description;
        public List<CardType> cardType;
        public int health;
        public int damageMin;
        public int damageMax;
        public List<DamageType> damageType;
        public Sprite cardSprite;

        public enum CardType
        {
            Fire,
            Earth,
            Water,
            Air
        }

        public enum DamageType
        {
            Fire,
            Earth,
            Water,
            Air
        }
    }
}