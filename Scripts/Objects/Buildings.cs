using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings : MonoBehaviour
{
    [SerializeField] private Texture blueprint_tex;
    private Texture default_tex;

    private MeshRenderer MR;

    private Coroutine Blinky_cor;

    [SerializeField]
    private int timeOfBlinking = 15;

    private int period = 50;
    // Start is called before the first frame update
    void Start()
    {
        MR = GetComponent<MeshRenderer>();
        default_tex = MR.materials[0].mainTexture;
    }

    public void Start_BuildProcess()
    {
        //Set new texture
        MR.materials[0].mainTexture = blueprint_tex;

        if (Blinky_cor != null) StopCoroutine(Blinky_cor);
        Blinky_cor = StartCoroutine(Blinky());

    }

    IEnumerator Blinky()
    {
        //during timeOfBlinking
        for (int i = 0; i < timeOfBlinking* period; i++)
        {
            yield return new WaitForSeconds( (float) 1/period );
            Color clr = MR.materials[0].color;
            clr.r += ( (float)(i % period - (period/2-1)) ) / (period*10);
            clr.g = clr.r; clr.b = clr.r;
            MR.materials[0].color = clr;
        }

        //Reset material
        MR.materials[0].mainTexture = default_tex;
    }
}

