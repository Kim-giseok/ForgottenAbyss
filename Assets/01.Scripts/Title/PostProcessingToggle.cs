using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PostProcessingToggle : MonoBehaviour
{
    public UnityEngine.Rendering.Volume volume;

    public Toggle bloomToggle;
    public Toggle vignetteToggle;
    public Toggle motionBlurToggle;
    public Toggle toneMappingToggle;
    public Toggle gammaGainToggle;
    public Toggle shadowsToggle;
    public Toggle filmGrainToggle;
    public Toggle chromaticAberrationToggle;

    private Bloom bloom;
    private Vignette vignette;
    private MotionBlur motionBlur;
    private Tonemapping toneMapping;
    private LiftGammaGain gammaGain;
    private ShadowsMidtonesHighlights shadows;
    private FilmGrain filmGrain;
    private ChromaticAberration chromaticAberration;

    void Start()
    {
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out motionBlur);
        volume.profile.TryGet(out toneMapping);
        volume.profile.TryGet(out gammaGain);
        volume.profile.TryGet(out shadows);
        volume.profile.TryGet(out filmGrain);
        volume.profile.TryGet(out chromaticAberration);

        bloomToggle.isOn = bloom.active;
        vignetteToggle.isOn = vignette.active;
        motionBlurToggle.isOn = motionBlur.active;
        toneMappingToggle.isOn = toneMapping.active;
        gammaGainToggle.isOn = gammaGain.active;
        shadowsToggle.isOn = shadows.active;
        filmGrainToggle.isOn = filmGrain.active;
        chromaticAberrationToggle.isOn = chromaticAberration.active;

        bloomToggle.onValueChanged.AddListener(isOn => bloom.active = isOn);
        vignetteToggle.onValueChanged.AddListener(isOn => vignette.active = isOn);
        motionBlurToggle.onValueChanged.AddListener(isOn => motionBlur.active = isOn);
        toneMappingToggle.onValueChanged.AddListener(isOn => toneMapping.active = isOn);
        gammaGainToggle.onValueChanged.AddListener(isOn => gammaGain.active = isOn);
        shadowsToggle.onValueChanged.AddListener(isOn => shadows.active = isOn);
        filmGrainToggle.onValueChanged.AddListener(isOn => filmGrain.active = isOn);
        chromaticAberrationToggle.onValueChanged.AddListener(isOn => chromaticAberration.active = isOn);
    }
}
