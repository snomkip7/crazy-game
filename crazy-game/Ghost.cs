using Godot;
using System;

public partial class Ghost : CharacterBody2D
{
	public const float speed = 350f;
	public Player player;
	public RandomNumberGenerator rng;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<Player>("../Player");
		rng = new RandomNumberGenerator();
		float pos1 = rng.Randfn(1600, 100);
		float pos2 = rng.Randfn(900, 100);
		if(rng.RandiRange(1,2)%2==0){
			pos1 *= -1;
		}
		if(rng.RandiRange(1,2)%2==0){
			pos2 *= -1;
		}
		Position = new Vector2(player.Position.X + pos1, player.Position.Y + pos2);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if(Math.Abs(player.Position.X-Position.X) < 1000 && Math.Abs(player.Position.Y-Position.Y) < 1000){
			Vector2 targetVelocity = (player.Position-Position).Normalized() * speed;
			Velocity = Velocity.Lerp(targetVelocity, .8f);
		} else{
			Velocity = Velocity.Lerp(Vector2.Zero, .8f);
		}

		MoveAndSlide();
	}

	public void HitPlayer(Node2D body){
		player.takeDmg();
		if(rng.RandiRange(0,1)==0){
			Position = new Vector2(Position.X + rng.RandiRange(500, 900), Position.Y);
		} else{
			Position = new Vector2(Position.X - rng.RandiRange(500, 900), Position.Y);
		}
		if(rng.RandiRange(0,1)==0){
			Position = new Vector2(Position.X, Position.Y + rng.RandiRange(500, 900));
		} else{
			Position = new Vector2(Position.X, Position.Y - rng.RandiRange(500, 900));
		}
		
	}
}
