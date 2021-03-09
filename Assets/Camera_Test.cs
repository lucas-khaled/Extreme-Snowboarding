using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Test : MonoBehaviour
{

    Player player;
    Vector3 offset;

    public void SetPlayer(Player player)
    {
        this.player = player;
        Debug.Log(this.gameObject.name);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null)
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
