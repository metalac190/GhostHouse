using UnityEngine;
using Utility.Audio.Helper;

namespace Mechanics.Feedback
{
    [CreateAssetMenu(menuName = "Sound System/Libraries/Sfx Material Library")]
    public class SfxMaterialLibrary : ScriptableObject
    {
        [SerializeField] private SfxReference _defaultSfx = new SfxReference();
        [SerializeField] private SfxReference _woodSfx = new SfxReference();
        [SerializeField] private SfxReference _stoneSfx = new SfxReference();
        [SerializeField] private SfxReference _leatherSfx = new SfxReference();
        [SerializeField] private SfxReference _carpetSfx = new SfxReference();
        [SerializeField] private SfxReference _wallSfx = new SfxReference();
        [SerializeField] private SfxReference _metalSfx = new SfxReference();
        [SerializeField] private SfxReference _glassSfx = new SfxReference();

        public SfxReference GetSfx(SfxType type) {
            switch (type) {
                case SfxType.Wood:
                    return _woodSfx;
                case SfxType.Stone:
                    return _stoneSfx;
                case SfxType.Leather:
                    return _leatherSfx;
                case SfxType.Carpet:
                    return _carpetSfx;
                case SfxType.Wall:
                    return _wallSfx;
                case SfxType.Metal:
                    return _metalSfx;
                case SfxType.Glass:
                    return _glassSfx;
                default:
                    return _defaultSfx;
            }
        }
    }

    public enum SfxType
    {
        Default,
        Wood,
        Stone,
        Leather,
        Carpet,
        Wall,
        Metal,
        Glass
    }
}