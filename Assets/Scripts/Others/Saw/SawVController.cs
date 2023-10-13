using UnityEngine;

public class SawVController : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 2f;
    [SerializeField] float vY = 2f;
    [SerializeField] Transform maxPointTop;
    [SerializeField] Transform maxPointBottom;

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (transform.position.y >= maxPointTop.position.y || transform.position.y <= maxPointBottom.position.y)
            vY = -vY;

        this.transform.position += new Vector3(0, vY, 0) * Time.deltaTime;

        this.transform.Rotate(0f, 0f, rotateSpeed * 360 * Time.deltaTime);
    }
}
