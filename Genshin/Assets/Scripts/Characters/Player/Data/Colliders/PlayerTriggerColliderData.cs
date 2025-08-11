using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [Serializable]
    public class PlayerTriggerColliderData
    {
        [field : SerializeField] public BoxCollider GroundCheckCollider { get; private set; }
    }
}

