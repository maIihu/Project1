using System;
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

        [Header("Highlight")]
        [SerializeField] private Color highlightColor = new Color(1f, 1f, 0f, 0.4f);
        [SerializeField] private SpriteRenderer highlightRenderer;

		private void Awake()
		{
            if (!spriteRenderer) spriteRenderer = this.GetComponent<SpriteRenderer>();
			highlightRenderer.enabled = false;
			highlightRenderer.color = highlightColor;
		}

		public void AddEffect(NodeEffect effect, int duration)
        {
            if(effect == null ) { return; }
            if(nodeEffect != null && nodeEffect.effect != null)
            {
                var cur = nodeEffect.effect;
                if (cur.priority > effect.priority)
                    return;
            }
            nodeEffect = new NodeEffectInstance
            {
                effect = effect,
                duration = duration
            };
			if (effect is CycledNodeEffect cycledNodeEffect)
                cycledNodeEffect.Initial(this, nodeEffect);
            UpdateVisualEffect();
		}
        public void SetHighlighted(bool on)
        {
			highlightRenderer.enabled = on;
		}
        private void UpdateVisualEffect()
        {
            if (nodeEffect == null) {
                SetBaseSprite(baseSprite);
                return; }
            if (nodeEffect.effect is IChangeNodeSprite changer)
            {
                //Debug.Log(changer.GetNodeEffectSprite(this, nodeEffect, baseSprite).name);
				SetSpriteNode(changer.GetNodeEffectSprite(this, nodeEffect, baseSprite));
            }
            else
                SetBaseSprite(baseSprite);

		}
        public void ClearEffect()
        {
            nodeEffect = null;
            spriteRenderer.sprite = baseSprite;
		}
        public void ReduceExistTurn()
        {
            if (nodeEffect == null) return;
            if(nodeEffect.effect is IOnCoolDown cd)
            {
                cd.ReduceCoolDown(this);
				UpdateVisualEffect();
			}
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
		}
		public void SetBaseSprite(Sprite sprite)
		{
			baseSprite = sprite;
			SetSpriteNode(sprite);
		}
	}
}