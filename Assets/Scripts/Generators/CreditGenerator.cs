using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CreditGenerator : MonoBehaviour
{
    //grab Director
    //Add credits every second
    //Credits per second = creditMultiplier * (1 + 0.4 * coeff) * (playerCount + 1) / 2
    private Stopwatch time;

    private CombatDirector combatDirector;
    // Start is called before the first frame update
    void Start()
    {
        time = Stopwatch.StartNew();
        combatDirector = gameObject.GetComponent<CombatDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (time.Elapsed.Seconds >= 1)
        {
            combatDirector.wallet += 2 * ((int)SceneDirector.Instance.coEef);
            time.Restart();
        }
    }
}
