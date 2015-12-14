using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyCharacter : Character
{
    [SerializeField] public float JumpPeakOffset = 2f;
    [SerializeField] private Character player;

    // Enemy gameplay state.
    private GameObject target;
}
