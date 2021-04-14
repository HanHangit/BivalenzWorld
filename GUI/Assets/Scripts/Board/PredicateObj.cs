﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Validator.Game;

public class PredicateObj : MonoBehaviour
{
    public float GetDefaultSize() => _defaultSize;
    [SerializeField]
    private float _defaultSize = 4.0f;

    public GameObject GetVisual() => _visual;
    [SerializeField]
    private GameObject _visual = default;

    [SerializeField]
    private GameObject _worldCanvasPrefab = default;
    private GameObject _worldCanvasInstance = default;

    public List<string> GetConstant() => _constant;
    private List<string> _constant = new List<string>();

    public List<string> GetTemporaryConstants() => _temporaryConstants;
    private List<string> _temporaryConstants = new List<string>();

    public List<Predicate> GetPredicates() => _predicateList;
    private List<Predicate> _predicateList = new List<Predicate>();

    public Predicate GetInitialPredicate() => _initialPredicate;
    private Predicate _initialPredicate = default;
    [SerializeField]
    private Predicate _sizePredicate;
    private Field _currentField = default;
    public Field CurrentField => _currentField;

    public void Init(Predicate predicate)
    {
        _initialPredicate = predicate;
        _predicateList.Add(_sizePredicate);
    }

    public void SetField(Field field)
    {
        _currentField = field;
    }

    public Field GetField()
    {
        return _currentField;
    }

    public void AddConstant(string constant)
    {
        if (_constant.Contains(constant))
        {
            _constant.Remove(constant);
        }
        else
        {
            _constant.Add(constant);
        }
        SpawnText();
    }

    public void AddTemporaryConstant(string constant)
    {
        if (!_temporaryConstants.Contains(constant))
        {
            _temporaryConstants.Add(constant);
        }
        SpawnText();
    }

    public void RemoveTemporaryConstants()
    {
        _temporaryConstants.Clear();
        SpawnText();
    }

    public void RemoveConstant(string constant)
    {
        if (_constant.Contains(constant))
        {
            _constant.Remove(constant);
            SpawnText();
        }
        else
        {
            Debug.Log("Something is wrong. Constant has not the key but u try to delete: " + constant);
        }
    }

    public void SpawnText()
    {
        if (!GetConstant().Any() && !GetTemporaryConstants().Any() && _worldCanvasInstance != null)
        {
            Destroy(_worldCanvasInstance.gameObject);
            _worldCanvasInstance = null;
        }
        else if (_worldCanvasInstance == null && (GetConstant().Any() || GetTemporaryConstants().Any()))
        {
            _worldCanvasInstance = Instantiate(_worldCanvasPrefab, this.transform.position, Quaternion.identity, this.transform);
            if(GameManager.Instance.GetCameraManager().GetCurrentCamMode() == CameraRotation.CameraMode.Cam2D)
            {
                _worldCanvasInstance.transform.localPosition = new Vector3(-1f, 4.5f, 0f);
            }
            else
            {
                _worldCanvasInstance.transform.localPosition = new Vector3(-3f, 1.5f, 0f);
            }

            _worldCanvasPrefab.transform.localScale = Vector3.one * 1f;
            SetConstantStringToWorldCanvas();
        }
        else if (_worldCanvasInstance != null)
        {
            SetConstantStringToWorldCanvas();
        }
    }

    private void SetConstantStringToWorldCanvas()
    {
        var textElement = _worldCanvasInstance.GetComponent<GUI_ConstantDisplay>();
        if (textElement != null)
        {
            textElement.SetText(GetConstantString());
        }
    }

    private string GetConstantString()
    {
        var finalString = "";
        foreach (var item in _constant)
        {
            finalString += item + " ";

        }

        foreach (var item in _temporaryConstants)
        {
            finalString += item + " ";
        }
        return finalString;
    }
    //refactor can only have one predicate
    public void AddModifier(Predicate predicate)
    {
        Debug.Log("add modifier" + predicate.PredicateIdentifier);
        var bhvr = predicate.Prefab.GetComponent<PredicateBhvr>();
        if (bhvr != null)
        {
            _predicateList = new List<Predicate>();
            _predicateList.Add(predicate);
            bhvr.Create(this, _visual);
            //if (!_predicateList.Contains(predicate))
            //{
            //    Debug.Log("not same");
            //    bhvr.Create(this);
            //    _predicateList.Add(predicate);
            //}
            //else
            //{
            //    Debug.Log("Remnove because same");
            //    RemoveModifier(predicate);
            //}
        }
        else
        {
            Debug.LogWarning("Wrong Mapping should be PredicateBhvr");
        }
    }

    public void RemoveAllModifier()
    {
        for (int i = _predicateList.Count - 1; i >= 0; i--)
        {
            var instance = _predicateList[i];
            instance.Prefab.GetComponent<PredicateBhvr>().Undo();
            _predicateList.Remove(instance);
        }
    }

    public void RemoveModifier(Predicate predicate)
    {
        if (_predicateList.Contains(predicate))
        {
            _predicateList.Remove(predicate);
            predicate.Prefab.GetComponent<PredicateBhvr>().Undo();
        }
    }
}
