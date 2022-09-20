# Glow / Outline for S&box!
Since the Glow component was removed from S&box, I decided to make my own version using just C# and VFX (Valve Shader Language) It's fairly simple to use and should take much work to use.

# Install
For use within a game, place the contents of this repository into the root folder of your game.

# Example
```cs
using Sandbox;
using Landis;

namespace Example
{
	/// <summary>
	/// Just an Entity with some cool glow!
	/// </summary>
	public partial class EntityWithGlow : ModelEntity
	{
		/// <summary>
		/// The actual glow component used in the entity.
		/// </summary>
		[BindComponent] public GlowEffect Glow { get; }

		public EntityWithGlow()
		{
			SetModel( "models/elan/rooms/props/door_panel.vmdl" );
			SetupPhysicsFromModel( PhysicsMotionType.Static, false );

			// Component must be created after the model is set.
			Components.Create<GlowEffect>();

			Glow.Active = true;
			Glow.Color = Color.Green;
		}
	}
}
```

# Credit
I'd like to have a mention crediting me somewhere if you use this in your game, it doesn't have to be big text but somewhere have text that can be displayed (or a label in a mainmenu) containing something like this
`Glow System created by Landis Games`

You can tweak the wording if you want, as long as the message is conveyed.
