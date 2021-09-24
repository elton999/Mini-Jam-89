using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingAnimations : MonoBehaviour
{

    // This didnt end up working
    // // full anims were playing instead of partial
    // // idle sprites didnt get triggered
    // so we'll continue using built-in sprite anims

    /*public SpriteAnimations upWalk;
    public SpriteAnimations downWalk;
    public SpriteAnimations rightWalk;
    public SpriteAnimations leftWalk;

    public Sprite upIdle;
    public Sprite downIdle;
    public Sprite rightIdle;
    public Sprite leftIdle;

    SpriteRenderer sr;
    void Start() {
        sr = GetComponent<SpriteRenderer>();
    }

    public Movement movement;
    Vector3 oldMoveDirection;
    void Update() {
        if (movement.direction != oldMoveDirection) {
            SpriteAnimations oldAnim = ChooseAnim(oldMoveDirection);
            if (oldAnim != null) oldAnim.StopAnimation();

            if (movement.direction == Vector3.zero) {
                Sprite sp = ChooseSprite(oldMoveDirection);
                if (sp != null) sr.sprite = sp;
            }
            else {
                SpriteAnimations anim = ChooseAnim(movement.direction);
                if (anim != null) anim.StartAnimation();
            }
        }
        oldMoveDirection = movement.direction;
        
    }
    SpriteAnimations ChooseAnim(Vector2 direction) {
        if (direction == Vector2Int.zero) return null;
        if (direction.y > 0)
            return upWalk;
        if (direction.y < 0)
            return downWalk;
        if (direction.x > 0)
            return rightWalk;
        if (direction.x < 0)
            return leftWalk;
        return null;
    }
    Sprite ChooseSprite(Vector2 direction) {
        if (direction == Vector2Int.zero) return null;
        if (direction.y > 0)
            return upIdle;
        if (direction.y < 0)
            return downIdle;
        if (direction.x > 0)
            return rightIdle;
        if (direction.x < 0)
            return leftIdle;
        return null;
    }*/


    void Start() {
        currentWalksound = grass;
    }

    //public Movement movement;
    //Vector3 oldMoveDirection;
    void Update() {
        //if (currentWalkSound == null) return;
        ////Debug.Log(currentWalkSound + " " + currentWalkSound.volumeMultiplier);
        //if (movement.direction != oldMoveDirection)
        //{
        //    if (movement.direction == Vector3.zero)
        //        currentWalkSound.DoFadeOut();
        //    else
        //        currentWalkSound.DoFadeIn();
        //}
        //oldMoveDirection = movement.direction;

       

    }


    //public AudioAnimations grassWalk; // default
    //public AudioAnimations stoneWalk;
    //public AudioAnimations wetGrassWalk;
    //AudioAnimations currentWalkSound;


    public AudioClip[] currentWalksound;
    public AudioClip[] stone;
    public AudioClip[] grass;
    public AudioClip[] wetgrass;
    public AudioSource Pas;

    //void ChangeWalkSound(AudioAnimations newWalkSound) {
    //    if (currentWalkSound != null) currentWalkSound.StopAudio();
    //    currentWalkSound = newWalkSound;
    //    if (currentWalkSound != null) currentWalkSound.StartAudio();
    //}

    void PlayWalkSound()
    {
        Pas.PlayOneShot(currentWalksound[Random.Range(0, currentWalksound.Length)]);
    }
    //void OnTriggerStay2D(Collider2D other) {
    //    AudioAnimations newWalkSound = null;
    //    if (other.gameObject.tag == "Wet Grass Floor")
    //        newWalkSound = wetGrassWalk;
    //    if (other.gameObject.tag == "Stone Floor")
    //        newWalkSound = stoneWalk;

    //    if (newWalkSound == null) return;
    //    if (newWalkSound != currentWalkSound)
    //        ChangeWalkSound(newWalkSound);
    //}
    //void OnTriggerExit2D(Collider2D other) {
    //    AudioAnimations newWalkSound = null;
    //    if (other.gameObject.tag == "Wet Grass Floor")
    //        newWalkSound = grassWalk;
    //    if (other.gameObject.tag == "Stone Floor")
    //        newWalkSound = grassWalk;

    //    if (newWalkSound == null) return;
    //    if (newWalkSound != currentWalkSound)
    //        ChangeWalkSound(newWalkSound);
    //}

    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    currentWalksound = null;
    //    if (other.gameObject.tag == "Wet Grass Floor")
    //       currentWalksound = grass;
    //   if (other.gameObject.tag == "Stone Floor")
    //       currentWalksound = stone;
    //}
    //private void OnTriggerExit2D(Collider2D other)
    //{
        
    //    if (other.gameObject.tag == "Stone Floor")
    //    {
    //        currentWalksound = null;
    //        currentWalksound = grass;
    //    }
            

    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        currentWalksound = null;

        if (other.gameObject.tag == "Stone Floor")
        {
            currentWalksound = stone;
        }
            
        if(other.gameObject.tag == "Grass")
        {
            currentWalksound = grass;
        }
        if(other.gameObject.tag == "Wet Grass Floor")
        {
            currentWalksound = wetgrass;
        }
       

    }


}
