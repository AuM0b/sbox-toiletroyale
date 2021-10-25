﻿using Sandbox;

namespace ToiletRoyale
{
	public partial class ToiletRoyalePlayer : Player
	{
		[Net, Predicted] TimeSince TimeSinceFart { get; set; }

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			Animator = new StandardPlayerAnimator();
			Camera = new ThirdPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			Host.AssertServer();

			LifeState = LifeState.Alive;
			Health = 100;
			Velocity = Vector3.Zero;
			WaterLevel.Clear();

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

				PlaySound( "fart" );

				TimeSinceFart = Rand.Float(0.0f, 1.0f);
			}
			
			SetAnimBool( "b_sit", true );
		}

		public override void TakeDamage( DamageInfo info ) { }
		public override void OnKilled() { }
	}
}