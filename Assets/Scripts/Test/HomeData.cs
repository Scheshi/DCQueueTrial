using UnityEngine;

namespace Assets.Scripts.Test
{
    [CreateAssetMenu(fileName = "Data/HomeData")]
    internal class HomeData : ScriptableObject
    {
        public Vector3 Position;
    }
}