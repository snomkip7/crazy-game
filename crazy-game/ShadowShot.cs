using Godot;
using System;

public partial class ShadowShot : CharacterBody2D
{
	public Player player;

	public override void _Ready()
	{
		player = GetNode<Player>("../Player");
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}

	public void timeout(){
		QueueFree();
	}

	public void HitPlayer(Node2D body){
		player.takeDmg();
		CallDeferred("timeout");
	}
}
