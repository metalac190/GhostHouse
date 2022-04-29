using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundSmokeAnim : MonoBehaviour
{
    [SerializeField] private float _time = 0;

    private void Start()
    {
        StartCoroutine(MoveImages(_time));
    }

    private IEnumerator MoveImages(float time)
    {
        Debug.Log("Started: " + transform.position);
        float t = 0;
        Vector3 startPos = transform.position;
        Vector3 end = new Vector3(transform.position.x-150f, transform.position.y, transform.position.z);

        while (t < 1)
        {
            yield return null;
            t += Time.deltaTime / time;
            transform.position = Vector3.Lerp(startPos, end, t);
        }

        transform.position = end;

        t = 0;
        startPos = end;
        end = new Vector3(transform.position.x+150f, transform.position.y, transform.position.z);


        while (t < 1)
        {
            yield return null;
            t += Time.deltaTime / time;
            transform.position = Vector3.Lerp(startPos, end, t);
        }
        transform.position = end;

        StartCoroutine(MoveImages(_time));
    }
}
