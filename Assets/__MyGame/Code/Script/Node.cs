using UnityEngine;

namespace __MyGame.Code.Script
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite baseSprite;
		public TileEntity OccupiedEntity { get; set; }
		public Vector2 GridPos => transform.position;

		public NodeEffectInstance nodeEffect { get; private set; }

		private void Awake()
		{
            if (!spriteRenderer) spriteRenderer = this.GetComponent<SpriteRenderer>();
		}

		public void AddEffect(NodeEffect effect, int duration)
        {
            if(effect == null ) { ClearEffect(); return; }
            nodeEffect = new NodeEffectInstance
            {
                effect = effect,
                duration = duration
            };
            spriteRenderer.sprite = effect.effectSpriteOverlay ? effect.effectSpriteOverlay : baseSprite;
		}
        public void ClearEffect()
        {
            nodeEffect = null;
            spriteRenderer.sprite = baseSprite;
		}
        public void ReduceExistTurn()
        {
            if (nodeEffect == null) return;
            if (nodeEffect.duration < 0) return;
            nodeEffect.duration--;
            if (nodeEffect.duration <= 0)
            {
                ClearEffect();
            }
        }

		public void SetSpriteNode(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            baseSprite = sprite;
		}
    }
}