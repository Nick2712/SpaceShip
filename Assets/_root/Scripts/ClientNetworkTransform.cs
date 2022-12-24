using Unity.Netcode.Components;
using UnityEngine;


namespace SpaceShip
{
    [DisallowMultipleComponent]
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative() => false;
    }
}
