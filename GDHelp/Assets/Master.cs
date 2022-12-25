using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SearchService;

public class Master : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    
    [SerializeField] private KeyCode pickUp;
    [SerializeField] private KeyCode drop;
    [SerializeField] private Transform dropPos;

    [SerializeField] private CanvasMaster canvasMaster;

    [SerializeField] private Light light;
    [SerializeField] private float dayDuration;

    private int _health; 
    private Camera _cam;
    private Item _carriedObject;

    private DateTime _startDT;
    private float _finalTime;

    private int _mistakesCount;
    
    private bool _canSleep;

    private void Start()
    {
        _cam = Camera.main;
        _health = maxHealth;
        _startDT = DateTime.Now;

        Item.OnWrongBox += () => _mistakesCount++;
    }

    private void Update()
    {
        if (_carriedObject == null && Input.GetKeyDown(pickUp))
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit) &&  hit.collider.gameObject.TryGetComponent(out _carriedObject))
            {
                UpdateState(false);
                Damage(_carriedObject.pickUpDamage);
            }

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.TryGetComponent(out Food food))
            {
                Damage(-food.heal);
            }
        }
        
        if (_carriedObject != null && Input.GetKeyDown(drop))
        {
            _carriedObject.transform.position = dropPos.position;
            UpdateState(true);
            Damage(_carriedObject.dropDamage);
        }

        if (_canSleep && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit) && hit.collider.CompareTag("Bed"))
            {
                End(true);
            }
        }
    }

    private void UpdateState(bool pickUpDrop)
    {
        _carriedObject.gameObject.SetActive(pickUpDrop);
        canvasMaster.ShowObjectInfo(!pickUpDrop, !pickUpDrop == true ? _carriedObject.info : string.Empty);
    }

    private void Damage(int damage)
    {
        _health -= damage;
        canvasMaster.HealthText(_health);
        if (_health <= 0)
        {
            End(false);
        }
    }

    private void End(bool winLose)
    {
        _finalTime = (DateTime.Now - _startDT).Minutes;
        canvasMaster.Die(winLose, _mistakesCount, _finalTime);
    }

    private IEnumerator MoveToNight()
    {
        while (light.intensity > 0.1f)
        {
            light.intensity -= 0.9f / dayDuration;
            if (light.intensity < 0.1f)
            {
                light.intensity = 0.09f;
            }
            yield return new WaitForSeconds(1f);
        }

        _canSleep = true;
    }
}
