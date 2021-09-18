using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float speed = 1.0f;
    
    float shakeTimer = 0.0f;
    float maxTimeShake = 0.0f;
    float magnitude = 0.5f;

    void Update()
    {
        var targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

        updateShake();
    }

    void updateShake(){
        if(shakeTimer >= maxTimeShake) return;
        transform.position += new Vector3(Random.Range(-magnitude, magnitude), Random.Range(-magnitude, magnitude), 0);
        shakeTimer += Time.deltaTime;
    }

    public void shake(float magnitude, float maxTime){
        this.magnitude = magnitude;
        this.maxTimeShake = maxTime;
        shakeTimer = 0.0f;
    }
}
