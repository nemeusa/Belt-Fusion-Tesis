using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IceState : State
{
    FSM<TypeFSM> _fsm;
    PlayerController _player;

    float _slamWindupTimer;
    float _glideTimer;
    Vector3 _boardBaseScale;
    Coroutine _boardRoutine;


    public IceState(FSM<TypeFSM> fsm, PlayerController player)
    {
        _fsm = fsm;
        _player = player;
    }

    public void OnEnter()
    {
        _player.ChangeElement(TypeFSM.Ice);

        //_player.meshColors.color = Color.blue;
        _player.ChangeMatElement(_player.iceMat[0], _player.iceMatEye[0]);
        _player.ChangeSkinElement(_player.iceMat, _player.iceMatEye, _player.iceMeshes);

        _player.OnJumpPressed += TryStartGlide;
        _player.OnAttackPressed += TryStartSlam;

    }

    public void OnUpdate()
    {
        if (_player.isSlamming)
        {
            UpdateSlam();
            return;
        }

        if (!_player.isGliding) return;

        // Cortes del planeo: tocar piso, soltar el botón, perder control, wallrun o se acabó el tiempo
        bool shouldStop = _player._controller.isGrounded
                          || !_player.jumpHeld
                          || _player.dontMovePlayer
                          || _player.isWallRunning;

        if (!shouldStop && _player.glideMaxTime > 0f)
        {
            _glideTimer += Time.deltaTime;
            shouldStop = _glideTimer >= _player.glideMaxTime;
        }

        if (shouldStop)
        {
            StopGlide(false);
            return;
        }

        // Caída lenta: si cae más rápido que la velocidad de planeo, frena de forma suave.
        // (Si todavía está subiendo no se toca, así no se come el final del salto.)
        float targetFall = -_player.glideFallSpeed;
        if (_player._playerVelocity.y < targetFall)
        {
            _player._playerVelocity.y = Mathf.MoveTowards(
                _player._playerVelocity.y,
                targetFall,
                _player.glideBrake * Time.deltaTime);
        }
    }

    public void OnExit()
    {
        _player.tablaIceCode._isActive = false;
        _player.OnJumpPressed -= TryStartGlide;
        StopGlide(true);
        _player.OnAttackPressed -= TryStartSlam;
        EndSlam();
    }

    void TryStartGlide()
    {
        if (_player.isGliding || _player.isDeath || _player.dontMovePlayer) return;

        // En el piso (o en coyote time) el salto es el normal, el planeo solo arranca en el aire
        if (_player._controller.isGrounded || _player.coyoteCounter > 0f) return;

        StartGlide();

    }


    #region Slam (panzazo)

    void TryStartSlam()
    {
        if (_player.isSlamming || _player.isDeath || _player.dontMovePlayer) return;

        // Solo en el aire (cayendo, saltando o planeando)
        if (_player._controller.isGrounded) return;

        StopGlide(false); // si venía planeando, se deshace la tabla




        _player.isSlamming = true;
        _slamWindupTimer = _player.slamWindup;
        _player.speed = _player.slamAirSpeed;

        _player.CountMoves(1);
        if (_player.slamAudio != null) _player.audioSource.PlayOneShot(_player.slamAudio);
        _player.VibrarControl(0.2f, 0.4f, 0.2f);
        _player.StartCoroutine(_player.CamEffects());

        if (_player.animator != null) _player.animator.SetBool("Slamming", true);
    }

    void UpdateSlam()
    {
        if (_player.dontMovePlayer)
        {
            EndSlam();
            return;
        }
        Vector3 pos = _player.transform.position;
        var fx = GameObject.Instantiate(_player.slamImpactPrefab, pos, Quaternion.identity);

        // Pausa corta en el aire antes de caer, para que se sienta el golpe
        if (_slamWindupTimer > 0f)
        {
            _slamWindupTimer -= Time.deltaTime;
            _player._playerVelocity.y = 0f;
            return;
        }

        if (_player._controller.isGrounded)
        {
            SlamImpact(fx);
            EndSlam();
            return;
        }

        // Se reaplica todos los frames porque MovePlayer le suma gravedad después
        _player._playerVelocity.y = -_player.slamSpeed;
    }

    void SlamImpact(GameObject target)
    {
        GameObject.Destroy(target, 2f);

        Vector3 pos = _player.transform.position;

        if (_player.slamImpactPrefab != null)
        {
            var fx = GameObject.Instantiate(_player.slamImpactPrefab, pos, Quaternion.identity);
            GameObject.Destroy(fx, 2f);
        }

        _player.VibrarControl(0.6f, 1f, 0.35f);

        var reactives = new HashSet<ISlamReactive>();
        Collider[] hits = Physics.OverlapSphere(pos, _player.slamRadius, _player.slamMask, QueryTriggerInteraction.Collide);

        foreach (var hit in hits)
        {
            var reactive = hit.GetComponentInParent<ISlamReactive>();
            if (reactive != null && reactives.Add(reactive)) reactive.OnSlam(_player);
        }
    }

    void EndSlam()
    {
        if (!_player.isSlamming) return;

        _player.isSlamming = false;
        _slamWindupTimer = 0f;
        _player.speed = _player.initialSpeed;

        if (_player.animator != null) _player.animator.SetBool("Slamming", false);
    }

    #endregion


    void StartGlide()
    {
        _player.isGliding = true;
        _glideTimer = 0f;
        _player.speed = _player.glideSpeed;

        _player.CountMoves(1);
        if (_player.iceGlideAudio != null) _player.audioSource.PlayOneShot(_player.iceGlideAudio);
        _player.VibrarControl(0.1f, 0.3f, 0.2f);
        _player.StartCoroutine(_player.CamEffects());

        if (_player.animator != null) _player.animator.SetBool("Gliding", true);

        ShowBoard(true, false);
    }

    void StopGlide(bool instant)
    {
        if (!_player.isGliding) return;

        _player.isGliding = false;
        _player.speed = _player.initialSpeed;

        if (_player.animator != null) _player.animator.SetBool("Gliding", false);

        ShowBoard(false, instant);
    }

    void ShowBoard(bool show, bool instant)
    {
        GameObject board = _player.iceBoard;
        if (board == null) return;

        if (_boardBaseScale == Vector3.zero) _boardBaseScale = board.transform.localScale;
        if (_boardRoutine != null) _player.StopCoroutine(_boardRoutine);

        if (instant)
        {
            board.transform.localScale = _boardBaseScale;
            board.SetActive(show);
            return;
        }

        _boardRoutine = _player.StartCoroutine(BoardScaleRoutine(board, show));
    }

    // La tabla "se forma" escalando de 0 a su tamaño, y al terminar se deshace
    IEnumerator BoardScaleRoutine(GameObject board, bool show)
    {
        const float duration = 0.15f;
        Transform t = board.transform;

        if (show)
        {
            board.SetActive(true);
            t.localScale = Vector3.zero;
        }

        Vector3 from = t.localScale;
        Vector3 to = show ? _boardBaseScale : Vector3.zero;

        float time = 0f;
        while (time < duration)
        {
            t.localScale = Vector3.Lerp(from, to, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        t.localScale = to;
        if (!show) board.SetActive(false);
    }
}

public interface ISlamReactive
{
    void OnSlam(PlayerController player);
}