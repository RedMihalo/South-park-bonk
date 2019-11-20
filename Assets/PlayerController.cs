using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    public override void Start()
    {
        team = UnitTeam.Player;
        base.Start();
    }
}
