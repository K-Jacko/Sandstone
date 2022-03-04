using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDirector : Director
{
    public float spawnRadius = 5f;
    private MobCard[] _validMobs;
    private GameObject _player;

    public override void Init()
    {
        _validMobs = ValidateMobCards(StageDirector.Instance.monsters);
        _player = StageDirector.Instance.Player;
    }

    protected override void Tick()
    {
        base.Tick();
        foreach (var validMob in _validMobs)
        {
            if (wallet > validMob.creditCost)
            {
                wallet -= validMob.creditCost;
                var go = Instantiate(validMob.entity,
                    _player.transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius),
                        Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius)),
                    Quaternion.identity);
                var goM = go.GetComponent<Monster>();
                validMob.GemElement = new None();
                goM.GemElement = validMob.GemElement;
                goM.gameObject.GetComponent<MeshRenderer>().material = validMob.mobMaterial;
            }
        }

        wallet += coEef * creditMultiplier;
    }
    MobCard[] ValidateMobCards(MobCard[] mobCards)
    {
        foreach (var mob in mobCards)
        {
            //Validate VS Table from stage Director. Use BNF Grammar in future. GemElements get decided here too
            Debug.Log($"{mobCards[0]}" + "is Valid");
        }

        return mobCards;
    }

    void OnDrawGizmos()
    {
        if (_player == null)
        {
            Gizmos.color = new Color(1f, 1f, 1f, 0.2f);
            var position = _player.transform.position;
            Gizmos.DrawSphere(position, spawnRadius);
        }
    }
}
