using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class HolograficTrail : MonoBehaviour
{
    [SerializeField] float _activeTime = 1;

    [Header("Mesh")]
    [SerializeField] float _meshRefreshRate;
    [SerializeField] float _meshDestroyDelay;
    [SerializeField] Transform _spawnPos;

    [Header("Shader")]
    [SerializeField] Material _mat;
    [SerializeField] string _shaderVarRef;
    [SerializeField] float _shaderVarRate = 0.1f;
    [SerializeField] float _shaderVarRefreshRate = 0.05f;

    private bool _isActive;
    private MeshFilter[] _meshFilters;

    RobotFollow robotCode;

    private void Awake()
    {
        robotCode = GetComponent<RobotFollow>();
    }

    private void Update()
    {
        if (!_isActive && !robotCode.estaQuieto)
        {
            _isActive = true;
            StartCoroutine(ActivateTrail(_activeTime));

        }

    }

    IEnumerator ActivateTrail (float timeActive)
    {
        while (timeActive > 0 && !robotCode.estaQuieto)
        {
            timeActive -= _meshRefreshRate;

            if(_meshFilters == null)
                _meshFilters = GetComponentsInChildren<MeshFilter>();

            for(int i = 0; i < _meshFilters.Length; i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(_meshFilters[i].transform.position, _meshFilters[i].transform.rotation);

                gObj.transform.localScale = _meshFilters[i].transform.lossyScale;

                MeshRenderer renderer = gObj.AddComponent<MeshRenderer>();
                MeshFilter filter = gObj.AddComponent<MeshFilter>();


                //Mesh mesh = new Mesh();
                //_meshFilters[i].BakeMesh(mesh);

                //filter.mesh = mesh;
                //renderer.material = _mat;

                renderer.shadowCastingMode = ShadowCastingMode.Off;

                filter.mesh = _meshFilters[i].sharedMesh;
                                                                
                renderer.material = new Material(_mat);


                StartCoroutine(MatFade(renderer.material, 0, _shaderVarRate, _shaderVarRefreshRate));

                Destroy(gObj, _meshDestroyDelay);

            }

            yield return new WaitForSeconds(_meshRefreshRate);
        }

        _isActive = false;
    }

    IEnumerator MatFade (Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(_shaderVarRef);

        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(_shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }
    }

}
