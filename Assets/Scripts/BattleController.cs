using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{

    List<UnitAlly> allyUnits = new();
    List<UnitEnemy> enemyUnits = new();

    private void Start() {
        foreach ( UnitAlly unit in FindObjectsOfType<UnitAlly>()) {
            allyUnits.Add( unit );
        }
        foreach ( UnitEnemy enemy in FindObjectsOfType<UnitEnemy>() ) {
            enemyUnits.Add( enemy );
        }


        Debug.Log( $"{allyUnits.Count} ALLIES \n" +
            $"{enemyUnits.Count} ENEMIES" );
    }


}
