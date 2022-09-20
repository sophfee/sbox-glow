using Sandbox;

namespace Landis
{
    /// <summary>
    /// A new and a bit better glow effect.
    /// </summary>
    public partial class GlowEffect : EntityComponent<ModelEntity>
    {

        protected override void OnActivate()
        {
            if ( Host.IsServer )
            {
                if ( GlowModel is null )
                {
                    GlowModel = new ModelEntity();
                    GlowModel.CopyFrom( Entity );
                    GlowModel.Parent = Entity;
                    GlowModel.EnableDrawing = false;
                    GlowModel.LocalPosition = Vector3.Zero;
                    GlowModel.LocalRotation = Rotation.FromPitch( 0.0f );
                    GlowModel.SetMaterialOverride( "materials/glow.vmat" );
                }
            }
        }

        /// <summary>
        /// Internal glow entity.
        /// </summary>
        [Net] private ModelEntity GlowModel { get; set; }

        /// <summary>
        /// Draws the glow effect when enabled.
        /// </summary>
        public bool Active
        {
            get
            {
                if ( GlowModel is not null )
                {
                    return GlowModel.EnableDrawing;
                }

                return false;
            }

            set
            {
                if ( GlowModel is not null )
                {
                    GlowModel.EnableDrawing = value;
                }
            }
        }

        /// <summary>
        /// The color of the glow.
        /// </summary>
        public Color Color
        {
            get
            {
                if ( GlowModel is not null )
                {
                    return GlowModel.RenderColor;
                }

                return Color.White;
            }

            set
            {
                if ( GlowModel is not null )
                {
                    GlowModel.RenderColor = value;
                }
            }
        }

        /// <summary>
        /// This doesn't seem to work, let me know if it does or doesn't.
        /// </summary>
        public float Opacity
        {
            get => Color.a;
            set => Color = Color.WithAlpha( value );
        }

        private Material _glowMaterial = Material.Load( "materials/glow.vmat" );

        /// <summary>
        /// Changes the material that is used to draw the glow, don't recommend changing this.
        /// </summary>
        public Material MaterialOverride
        {
            get
            {
                return _glowMaterial;
            }
            set
            {
                _glowMaterial = value;
                GlowModel.SetMaterialOverride( _glowMaterial );
            }
        }
    }
}
