using UnityEditor.Rendering;
using UnityEngine;

namespace Mobs
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Spikes",fileName = "Spikes")]
    public class Spikes : ScriptableObject
    {
        [SerializeField]
        private float m_SpikeDamage = 15f;
        [SerializeField]
        private float m_SpikeHP = 40f;
        
        public float SpikeDamage
        {
            get => m_SpikeDamage;
            set => m_SpikeDamage = value;
        }
        
        public float SpikeHP
        {
            get => m_SpikeHP;
            set => m_SpikeHP = value;
        }
    }
}