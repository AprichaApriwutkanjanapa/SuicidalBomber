using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Student
{

    public class OOPExit : Identity
    {
        public GameObject YouWin;

        public override void Hit()
        {
            Debug.Log("Exit unlocked");
            mapGenerator.player.enabled = false;
            YouWin.SetActive(true);
            Debug.Log("You win");
        }
    }
}