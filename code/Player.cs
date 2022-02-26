using Sandbox;

namespace ToiletRoyale
{
	public partial class ToiletRoyalePlayer : Player
	{
		[Net, Predicted] TimeSince TimeSinceFart { get; set; }

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			Animator = new StandardPlayerAnimator();
			CameraMode = new ThirdPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			DressPlayer();

			Host.AssertServer();

			LifeState = LifeState.Alive;
			Health = 100;
			Velocity = Vector3.Zero;

			CreateHull();

			var Game = ToiletRoyaleGame.Current;

			if ( Game != null )
			{
				if ( Game.Toilets.Find( toilet => toilet.Claimer == this ) == null ) Game.MoveToEmptyToilet( this );
			}

			ResetInterpolation();
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			SimulateActiveChild( cl, ActiveChild );

			if ( Input.Pressed( InputButton.Attack1 ) && TimeSinceFart > 2.25f )
			{
				Rand.SetSeed( Time.Tick );

				Sound Sound = PlaySound( "fart" + Rand.Int( 1, 28 ) );

				Sound.SetVolume( Rand.Float( 0.5f, 0.8f ) );
				Sound.SetPitch( Rand.Float( 0.5f, 1.5f ) );

				TimeSinceFart = Rand.Float( 0.0f, 1.0f );
			}

			SetAnimParameter( "b_sit", true );
		}

		public override void TakeDamage( DamageInfo info ) { }
		public override void OnKilled() { }
	}
}
