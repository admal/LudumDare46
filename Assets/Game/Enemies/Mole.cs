using Game.Damage;
using Game.Plants;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mole : MonoBehaviour
{
    private GameObject _target;

    [SerializeField]
    private float _speed = 2f;
    [SerializeField]
    private float _baseDamage = 1f;

    private GameObject _targetArrow;

    private void Start()
    {
        PickClosestPlant();
    }
    private void Update()
    {
        if (_target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
            _targetArrow.transform.LookAt(this.transform, Vector3.up);

            Debug.DrawLine(transform.position, _target.transform.position, Color.red);
            if (Vector3.Distance(transform.position, _target.transform.position) < 0.1f)
            {
                var damagable = _target.gameObject.GetComponent<IDamagable>();

                if (damagable != null)
                {
                    _speed = 0;
                    damagable.ApplyDamage(new DamageInfo() { BaseDamage = _baseDamage });
                    if (_targetArrow != null)
                    {
                        _targetArrow.SetActive(false);
                    }

                    DestroyMole();
                }
            }
        }
        else
        {
            PickClosestPlant();
        }
    }

    private void PickClosestPlant()
    {
        var targets = PlantsManager.Instance.GetPlants();
        if (targets.Length == 0)
        {
            DestroyMole();
            return;
        }

        Plant closestTarget = targets[Random.Range(0, targets.Length)];
        var closestTargetValue = float.MaxValue;

        foreach (var plant in targets)
        {
            var distance = Vector3.Distance(plant.transform.position, transform.position);
            if (distance < closestTargetValue)
            {
                closestTarget = plant;
                closestTargetValue = distance;
            }
        }

        _target = closestTarget.gameObject;
        _targetArrow = closestTarget.EnemyArrow;
        _targetArrow.SetActive(true);
    }

    public void DestroyMole()
    {
        if (_targetArrow != null)
        {
            _targetArrow.SetActive(false);
        }
        Destroy(gameObject);
    }
}
