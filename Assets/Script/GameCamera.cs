using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Camera))]
public class GameCamera : MonoBehaviour
{
    Player player;
    Vector3 offset;

    public Camera MyCamera { get; private set; }

    private void Awake()
    {
        MyCamera = GetComponent<Camera>();
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
        player.playerCamera = this;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(player != null)
            transform.parent.transform.position = player.transform.position;
    }

    public IEnumerator CameraShake(bool deactivateCameraShake, float shakingDuration, float magnetude)
    {
        if (!deactivateCameraShake)
        {
            Vector3 originalPoisition = this.transform.localPosition;

            float timeElapsed = 0f;

            while (timeElapsed < shakingDuration)
            {
                float x = Random.Range(-1f, 1f) * magnetude;
                float y = Random.Range(-1f, 1f) * magnetude;

                this.transform.localPosition = new Vector3(x + originalPoisition.x,
                                                              y + originalPoisition.y,
                                                              originalPoisition.z);

                timeElapsed += Time.deltaTime;

                yield return null;
            }

            this.transform.localPosition = originalPoisition;
        }
    }
}
