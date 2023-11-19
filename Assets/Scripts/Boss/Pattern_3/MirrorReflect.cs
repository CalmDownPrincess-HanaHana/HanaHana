using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using static Define;
using Random = UnityEngine.Random;

public class MirrorReflect : MonoBehaviour
{
    private LineRenderer predictionLine;
    private RaycastHit2D predictionHit;
    private LayerMask predictionLayerMask;
    private GameObject player;
    private Player player_script;
    private SpriteRenderer player_spriterenderer;
    private AudioSource _audioSource;

    [SerializeField] private bool isReflectable;
    [SerializeField] private bool is_first_entered;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private bool _rotating = false;
    [SerializeField] private float _rotateSpeed = 20.0f;
    [SerializeField] private List<AudioClip> _audioClips;
    private float angle = 0f;
    private bool _isActivatedBefore = false;
    private bool _isPlaying = false;

    private void Start()
    {
        predictionLine = GetComponent<LineRenderer>();
        player = GameObject.FindWithTag("Player");
        player_script = player.GetComponent<Player>();
        player_spriterenderer = player.GetComponent<SpriteRenderer>();
        predictionLayerMask = ~(1 << 8);
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// material에 따라서 해당하는 Laser를 활성화
    /// </summary>
    /// <returns></returns>
    private void LateUpdate()
    {
        switch (predictionLine.materials[0].name)
        {
            case "RedLaserMat (Instance)":
                RedLaser();
                break;
            case "VioletLaserMat (Instance)":
                VioletLaser();
                break;
            case "BlueLaserMat (Instance)":
                BlueLaser();
                break;
            case "GreenLaserMat (Instance)":
                GreenLaser();
                break;
            case "YellowLaserMat (Instance)":
                YellowLaser();
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private void DrawPredictionLine()
    {
        if (_rotating)
        {
            angle = (angle + _rotateSpeed * Time.deltaTime) % 360;
            _direction = new Vector3((float)Math.Cos(angle) * 1f, (float)Math.Sin(angle) * 1f, 0f);
        }
        predictionLine.SetPosition(0, this.transform.position);
        predictionHit = Physics2D.Raycast(transform.position, _direction, Mathf.Infinity, predictionLayerMask);
        Debug.DrawRay(transform.position, _direction * 10, Color.red);

        if(predictionHit.collider == null)
        {
            predictionLine.positionCount = 2;
            predictionLine.SetPosition(1, new Vector3(_direction.x * 100, _direction.y * 100, 0));
            return;
        }

        // 플레이어면 죽음
        if (predictionHit.collider.CompareTag("Player"))
        {
            player_script.Die(new Vector2(transform.position.x, transform.position.y));
            //PlayerDied();
        }

        // draw first collision point
        predictionLine.SetPosition(1, predictionHit.point);
        
        if (!isReflectable)
        {
            predictionLine.positionCount = 2;
            return;
        }
        
        // calculate second ray by Vector2.Reflect
        var inDirection = (predictionHit.point - (Vector2)transform.position).normalized;
        var reflectionDir = Vector2.Reflect(inDirection, predictionHit.normal);

        // By multiply 0.001, can have detail calculation
        predictionHit = Physics2D.Raycast(predictionHit.point + (reflectionDir * 0.001f), reflectionDir, Mathf.Infinity, predictionLayerMask);

        if (predictionHit.collider.IsUnityNull())
        {
            predictionLine.SetPosition(1, new Vector3(_direction.x + transform.position.x, _direction.y + transform.position.y, 0));
            return;
        }

        // 플레이어면 죽음
        if (predictionHit.collider.CompareTag("Player"))
        {
            player_script.Die(new Vector2(transform.position.x, transform.position.y));
           // PlayerDied();
        }
        predictionLine.SetPosition(2, predictionHit.point);

        // finally render linerenderer
        predictionLine.enabled = true;
    }

    void HideLaser()
    {
        predictionLine.enabled = false;
    }


    void ShowLaser()
    {
        predictionLine.enabled = true;
    }

    void PlayerDied()
    {
        Vector2 target_pos = new Vector2(0, 0);
        if (player_spriterenderer.flipX)
        {
            target_pos = new Vector2(player.transform.position.x - 1, 0);
        }
        else
        {
            target_pos = new Vector2(player.transform.position.x + 1, 0);
        }
        if (is_first_entered)
        {
            is_first_entered = false;
            player_script.onDamaged(target_pos);
            GameManager.instance.OnPlayerDead();
        }
    }

    private void RedLaser()
    {
        if (player_script.player_state == PlayerState.Jump)
        {
            ShowLaser();
            DrawPredictionLine();
            PlayLaserSound();
        }
        else
        {
            HideLaser();
            _isActivatedBefore = false;
        }
    }

    private void VioletLaser()
    {
        ShowLaser();
        DrawPredictionLine();
    }

    private void BlueLaser()
    {
        if (player_script.player_state != PlayerState.Jump)
        {
            ShowLaser();
            DrawPredictionLine();
            PlayLaserSound();
        }
        else
        {
            HideLaser();
        }
    }

    private void GreenLaser()
    {
        if (player.GetComponent<SpriteRenderer>().flipX)
        {
            ShowLaser();
            DrawPredictionLine();
            PlayLaserSound();
        }
        else
        {
            HideLaser();
        }
    }

    private void YellowLaser()
    {
        if (!player.GetComponent<SpriteRenderer>().flipX)
        {
            ShowLaser();
            DrawPredictionLine();
            PlayLaserSound();
        }
        else
        {
            HideLaser();
        }
    }

    
    private void PlayLaserSound()
    {
        if (!_isActivatedBefore)
        {
            int count = Random.Range(0, _audioClips.Count);
            _audioSource.volume = 1f;
            _audioSource.PlayOneShot(_audioClips[count]);
        }
        _isActivatedBefore = true;
    }
}
