using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AnimatorNamedParameters : MonoBehaviour
{
    Animator animator;
    private void Awake() => animator = GetComponent<Animator>();

    [System.Serializable]
    private class Parameter
    {
        public int intParameter;
        public string name;
    }

    [SerializeField] List<Parameter> parameters;
    public int GetIntByName(string value) =>
        parameters.
            Where(param => param.name == value).
            Select(param => param.intParameter).
            DefaultIfEmpty().
            Single();

    public void SetString(string value) => animator.SetInteger("Animation", GetIntByName(value));
}
