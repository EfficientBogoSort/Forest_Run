using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class FogEffect : MonoBehaviour
{
    public Material _mat;
    public Color _initialFogColor;
    public Color _finalFogColor;
    public float _mixingFactor;
    public float _startDistance;
    public float _end;
    public bool _needForward;
 
    [Range(0.0f, 1.0f)]public float _density;
    [Range(0.0f, 1.0f)]public float _fogDensityWithSkyUp;
    [Range(0.0f, 1.0f)]public float _fogDensityWithSkyDown;

    // Use radial distance
    [SerializeField]
    bool _useRadialDistance;

    public bool useRadialDistance {
        get { return _useRadialDistance; }
        set { _useRadialDistance = value; }
    }

    public enum functionType {
        LINEAR,
        EXP,
        EXP2
    }
    public functionType _functionType;

    [Header("Noise")]
    public bool _implementNoise;
    [Range(-1, 1)]public float _noiseMovingSpeed;
    [Range(0, 1)]public float _noiseForwardSpeed;
    public float _currentTime;

    // Start is called before the first frame update
    void Start()
    {
        // enable depth texture
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
        _currentTime = 0;
    }

    void Update()
    {

        _mat.SetFloat("_Start", _startDistance);
        _mat.SetFloat("_End", _end);
        _mat.SetFloat("_Density", _density);
        _mat.SetFloat("_FogDensityWithSkyUp", _fogDensityWithSkyUp);
        _mat.SetFloat("_FogDensityWithSkyDown", _fogDensityWithSkyDown);
        _mat.SetColor("_InitialFogColor", _initialFogColor);
        _mat.SetColor("_FinalFogColor", _finalFogColor);
        _mat.SetFloat("_MixingFactor", _mixingFactor);

        var skybox = RenderSettings.skybox;
        _mat.SetTexture("_SkyCubemap", skybox.GetTexture("_Tex"));
        _mat.SetColor("_SkyTint", skybox.GetColor("_Tint"));
        _mat.SetFloat("_SkyExposure", skybox.GetFloat("_Exposure"));
        _mat.SetFloat("_SkyRotation", skybox.GetFloat("_Rotation"));

        if (_useRadialDistance)
            _mat.EnableKeyword("RADIAL_DIST");
        else
            _mat.DisableKeyword("RADIAL_DIST");

        if (_functionType == functionType.LINEAR) {
            _mat.EnableKeyword("LINEAR");
            _mat.DisableKeyword("EXP");
        } else if (_functionType == functionType.EXP) {
            _mat.EnableKeyword("EXP");
            _mat.DisableKeyword("LINEAR");
        } else if (_functionType == functionType.EXP2) {
            _mat.DisableKeyword("LINEAR");
            _mat.DisableKeyword("EXP");
        }
        if (_implementNoise) {
            _mat.EnableKeyword("ImplementNoise");
        } else {
            _mat.DisableKeyword("ImplementNoise");
        }
        _mat.SetFloat("_NoiseMovingSpeed", _noiseMovingSpeed);
        _mat.SetFloat("_NoiseForwardSpeed", _noiseForwardSpeed);
        if (_needForward) {
            _mat.EnableKeyword("NeedForward");
        } else {
            _mat.DisableKeyword("NeedForward");
        }
        if (_needForward) {
            _currentTime += Time.deltaTime;
        }
        _mat.SetFloat("stopTime", _currentTime);

    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination) {

            // Calculate vectors towards frustum corners.
            var cam = GetComponent<Camera>();
            var camTransfrom = cam.transform;
            var camNear = cam.nearClipPlane;
            var camFar = cam.farClipPlane;

            var tanHalfFov = Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad / 2);
            var toRight = camTransfrom.right * camNear * tanHalfFov * cam.aspect;
            var toTop = camTransfrom.up * camNear * tanHalfFov;

            var v_tl = camTransfrom.forward * camNear - toRight + toTop;
            var v_tr = camTransfrom.forward * camNear + toRight + toTop;
            var v_br = camTransfrom.forward * camNear + toRight - toTop;
            var v_bl = camTransfrom.forward * camNear - toRight - toTop;

            var v_s = v_tl.magnitude * camFar / camNear;

            // draw screen quad.
            RenderTexture.active = destination;

            _mat.SetTexture("_MainTex", source);
            _mat.SetPass(0);

            // save the current state
            GL.PushMatrix();
            GL.LoadOrtho();
            GL.Begin(GL.QUADS);

            GL.MultiTexCoord2(0, 0, 0);
            GL.MultiTexCoord(1, v_bl.normalized * v_s);
            // BL (down left)
            GL.Vertex3(0, 0, 0.1f);

            GL.MultiTexCoord2(0, 1, 0);
            GL.MultiTexCoord(1, v_br.normalized * v_s);
            // BR (down right)
            GL.Vertex3(1, 0, 0.1f);

            GL.MultiTexCoord2(0, 1, 1);
            GL.MultiTexCoord(1, v_tr.normalized * v_s);
            // TR (top right)
            GL.Vertex3(1, 1, 0.1f);

            GL.MultiTexCoord2(0, 0, 1);
            // TL (top left)
            GL.MultiTexCoord(1, v_tl.normalized * v_s);
            GL.Vertex3(0, 1, 0.1f);

            GL.End();
            GL.PopMatrix();
    }
}
