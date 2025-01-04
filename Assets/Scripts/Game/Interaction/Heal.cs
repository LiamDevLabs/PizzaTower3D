using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Collectable
{
    public override CollectableType Collect(PlayerBaseController controller, Rigidbody playerRB)
    {
        controller.Health.Damage(-1);
        return base.Collect(controller, playerRB);
    }
}
