using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    Unit myUnit;

    private void Awake() {
        myUnit = this.GetComponent<Unit>();
        if(myUnit != null ) {

        } else {
            throw new System.Exception( $"BattleUnit \"{this.name}\" does not have a Unit component" );
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }


    private void InitBattleUnit(bool isAlly) {

    }
}
