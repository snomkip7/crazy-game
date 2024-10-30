using Godot;
using System;

public partial class Shadow : CharacterBody2D
{
	public Player player;
	public PackedScene shoot;
	public RandomNumberGenerator rng;
	public Timer shotTimer;
	public override void _Ready()
	{
		player = GetNode<Player>("../Player");
		shoot = ResourceLoader.Load<PackedScene>("ShadowShot.tscn");
		rng = new RandomNumberGenerator();
		shotTimer = GetNode<Timer>("ShotTimer");
		Position = new Vector2(rng.RandfRange(-1800, 3000), rng.RandfRange(-900, 1800));
		GD.Print(Position);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(shotTimer.TimeLeft == 0 && Math.Abs(player.Position.X - Position.X) < 2500 && Math.Abs(player.Position.X - Position.X) < 2500 ){
			float angle = Position.AngleToPoint(player.Position)/(float)(2*Math.PI) * 360;
			angle += rng.Randfn(0, 4);
			CharacterBody2D shot = shoot.Instantiate<CharacterBody2D>();
			shot.GlobalPosition = GlobalPosition;
			shot.RotationDegrees = angle;
			shot.Velocity = new Vector2(500,0).Rotated(shot.Rotation);
			GetNode("../").AddChild(shot);
			shotTimer.Start(rng.Randfn(.7f,.2f));
		}
	}
}
