using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    [Header("refereance")]
    private Animator animator;
    [SerializeField] private Transform characterSprite;
    public float directionX;
    public float directionY;

    private void Start()
    {
        GameObject spritePlayer = GameObject.Find("SpritePlayer");
        if (spritePlayer != null)
        {
            animator = spritePlayer.GetComponent<Animator>();
        }
    }

    public void AnimationOnWalk(bool FlashLightOn)
    {
        if (Mathf.Abs(directionX) > Mathf.Abs(directionY) && !FlashLightOn) // Horizontal
        {
            animator.Play("Side");

            if (directionX > 0)
                characterSprite.localScale = new Vector3(1, 1, 1);  // Facing right
            else if (directionX < 0)
                characterSprite.localScale = new Vector3(-1, 1, 1); // Facing left
        }
        else if (directionY > 0 && !FlashLightOn)
        {
            animator.Play("Walk-Up");
            characterSprite.localScale = new Vector3(1, 1, 1); // No flip
        }
        else if (directionY < 0 && !FlashLightOn)
        {
            animator.Play("Walk-Down");
            characterSprite.localScale = new Vector3(1, 1, 1); // No flip
        }

        if (directionX == 0 && directionY == 0)
        {
            animator.Play("Idle");
        }
    }

    public void AnimationOnFlashLight(float angle)
    {
        string directionFlash = GetDirectionFromAngle(angle);
        animator.Play(directionFlash);
    }

    string GetDirectionFromAngle(float angle)
    {
        if (angle < 0) angle += 360;

        if (angle >= 45 && angle < 135)
        {
            if (directionY != 0)
                return "Walk-Up-FlashLight";
            else
                return "Idle";
        }
        else if (angle >= 135 && angle < 225)
        {
            if (directionX < 0)
                return "Side-Left-FlashLight";
            else if (directionX > 0)
                return "Side-Right-FlashLight";
            else
                return "Idle";
        }
        else if (angle >= 225 && angle < 315)
        {
            if (directionY != 0)
                return "Walk-Down-FlashLight";
            else
                return "Idle";
        }
        else
        {
            if (directionX > 0)
                return "Side-Right-FlashLight";
            if (directionX < 0)
                return "Side-Left-FlashLight";
            return "Idle";
        }
    }
}
