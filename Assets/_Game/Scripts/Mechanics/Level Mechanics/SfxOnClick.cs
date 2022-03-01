using Mechanics.Feedback;
using UnityEngine;
using Utility.Audio.Managers;

namespace Mechanics.Level_Mechanics
{
    public class SfxOnClick : InteractableBase
    {
        [SerializeField] private SfxType _sfx = SfxType.Default;

        public override void OnLeftClick(Vector3 position) {
            SoundManager.Instance.PlaySfx(_sfx, position);
        }
    }
}