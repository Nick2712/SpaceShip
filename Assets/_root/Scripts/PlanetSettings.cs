using UnityEngine;


namespace SpaceShip
{
    [CreateAssetMenu(fileName = nameof(PlanetSettings), menuName = "SpaseShip/" + nameof(PlanetSettings))]
    public class PlanetSettings : ScriptableObject
    {
        [field: SerializeField] public GameObject Sun { get; private set; }
        [field: SerializeField] public GameObject[] Planets { get; private set; }
    }
}
