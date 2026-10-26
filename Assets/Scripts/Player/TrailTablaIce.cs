using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class TrailTablaIce : MonoBehaviour
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


    public bool _isActive;
    [SerializeField] MeshFilter _meshFilters;

    void Update()
    {
        if (!_isActive)
        {
            _isActive = true;
            StartCoroutine(ActivateTrail(_activeTime));
        }
    }


    IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= _meshRefreshRate;

   

                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(_meshFilters.transform.position, _meshFilters.transform.rotation);

                gObj.transform.localScale = _meshFilters.transform.lossyScale;

                MeshRenderer renderer = gObj.AddComponent<MeshRenderer>();
                MeshFilter filter = gObj.AddComponent<MeshFilter>();


                //Mesh mesh = new Mesh();
                //_meshFilters[i].BakeMesh(mesh);

                //filter.mesh = mesh;
                //renderer.material = _mat;

                renderer.shadowCastingMode = ShadowCastingMode.Off;

                filter.mesh = _meshFilters.sharedMesh;

                renderer.material = new Material(_mat);

                Destroy(gObj, _meshDestroyDelay);

                //StartCoroutine(MatFade(renderer.material, 0, _shaderVarRate, _shaderVarRefreshRate));


            

            yield return new WaitForSeconds(_meshRefreshRate);
        }

        _isActive = false;
    }


    IEnumerator MatFade(Material mat, float goal, float rate, float refreshRate)
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
