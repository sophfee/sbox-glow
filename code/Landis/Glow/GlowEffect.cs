using Sandbox;

namespace Landis
{
	/// <summary>
	/// A new and a bit better glow effect.
	/// Create after setting model on a ModelEntity.
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

					GlowModel.LocalPosition = Vector3.Zero;
					GlowModel.LocalRotation = Rotation.FromPitch( 0.0f );
				}
			}

			GlowModel.SetMaterialOverride( "materials/glow.vmat" );
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
	}
}
