using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostingEffect : MonoBehaviour
{
    [SerializeField] float ghostDelay;
    [SerializeField] float ghostLifeSpan = 0.5f;
    private float ghostDelaySeconds;

    public GameObject target;
    public bool enableGhost;

    void Start()
    {
        ghostDelaySeconds = ghostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (enableGhost)
        {
            if (ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                GameObject currentGhost = Instantiate(target, transform.position, transform.rotation);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentGhost.transform.localScale = this.transform.localScale;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                ghostDelaySeconds = ghostDelay;
                Destroy(currentGhost, ghostLifeSpan);
            }
        }
    }
}
