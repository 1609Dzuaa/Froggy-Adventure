using UnityEngine;

public class SawHController : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 2f;
    [SerializeField] float vX = 2f;
    [SerializeField] Transform maxPointLeft;
    [SerializeField] Transform maxPointRight;

    // Update is called once per frame
    void Update()
    {
        //Để ý tag của thằng Saw
    }

    private void FixedUpdate()
    {
        if (transform.position.x <= maxPointLeft.position.x || transform.position.x >= maxPointRight.position.x)
            vX = -vX;

        this.transform.position += new Vector3(vX, 0, 0) * Time.deltaTime;

        this.transform.Rotate(0f, 0f, rotateSpeed * 360 * Time.deltaTime);
    }

}
