using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float speed = 300.0f;
	public const float crazySpeed = 250.0f;
	public byte health = 3;
	public bool cursorLock = false;
	public Vector2 targetLocation = Vector2.Zero;
	public CharacterBody2D target;
	public PackedScene shoot;
	public Timer shotTimer;
	public RandomNumberGenerator rng;
	public byte animation = 2;
	public CanvasModulate screenColor;

    public override void _Ready()
    {
		target = GetNode<CharacterBody2D>("Target");
        cursorLock = true;
		Input.MouseMode = Input.MouseModeEnum.Captured;
		shoot = ResourceLoader.Load<PackedScene>("Shot.tscn");
		shotTimer = GetNode<Timer>("ShotTimer");
		rng = new RandomNumberGenerator();
		screenColor = GetNode<CanvasModulate>("ScreenColor");
		screenColor.Color = new Color(0,0,0);
		float pos1 = rng.Randfn(2000, 100);
		float pos2 = rng.Randfn(1100, 100);
		if(rng.RandiRange(1,2)%2==0){
			pos1 *= -1;
		}
		if(rng.RandiRange(1,2)%2==0){
			pos2 *= -1;
		}
		GetNode<Area2D>("../Goal").Position = new Vector2(Position.X + pos1, Position.Y + pos2);
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		Vector2 direction = Input.GetVector("left", "right", "up", "down");
		
		int time = (int) Time.GetTicksUsec();

		velocity = direction*speed;
		velocity.X = velocity.X + (float)Math.Sin(time)*crazySpeed;
		if(time%2==0){
			velocity.Y = velocity.Y + (float)Math.Sin(time)*crazySpeed;
		}
		else{
			velocity.Y = velocity.Y - (float)Math.Sin(time)*crazySpeed;
		}
		//GD.Print(velocity);


		Velocity = velocity;

		if(Input.IsActionJustPressed("escape")){
			if(cursorLock){
				cursorLock = false;
				Input.MouseMode = Input.MouseModeEnum.Visible;
			} else{
				cursorLock = true;
				Input.MouseMode = Input.MouseModeEnum.Captured;
			}
		}

		target.Velocity = target.Velocity.Lerp(targetLocation-target.Position, .9f)*2;
		target.MoveAndSlide();

		if(shotTimer.TimeLeft == 0 && Input.IsActionJustPressed("shoot")){
			CharacterBody2D shot = shoot.Instantiate<CharacterBody2D>();
			shot.GlobalPosition = GlobalPosition;
			shot.Rotation = GlobalPosition.AngleToPoint(target.GlobalPosition) + (float) Math.PI/2f + rng.Randfn(0,.1f);
			shot.Velocity = new Vector2(0,-1000).Rotated(shot.Rotation);
			GetNode("../").AddChild(shot);
			shotTimer.Start(rng.Randfn(.3f,.7f));
		}

		MoveAndSlide();

		if(animation==1){
			screenColor.Color = new Color(screenColor.Color.R-.01f, screenColor.Color.G+.01f, screenColor.Color.B+.01f); 
			if(screenColor.Color.R <=1){
				screenColor.Color = new Color(1,1,1);
				animation = 0;
			}
		} else if(animation==2){
			screenColor.Color = new Color(screenColor.Color.R+.01f, screenColor.Color.G+.01f, screenColor.Color.B+.01f); 
			if(screenColor.Color.R>=1){
				screenColor.Color = new Color(1,1,1);
				animation = 0;
			}
			//GD.Print(screenColor.Color);
		}
	}

	public override void _Input(InputEvent @event){
		if(cursorLock && @event is InputEventMouseMotion mouseMotion){
			Vector2 motion = mouseMotion.Relative;
			targetLocation += motion;
		}
		//GD.Print(targetAngle);
	}

	public void takeDmg(){
		health--;
		if(health==0){
			CallDeferred("restart");
		}
		screenColor.Color = new Color(2,0,0);
		animation = 1;
	}

	public void restart(){
		GetTree().ReloadCurrentScene();
	}

	public void goalReached(Area2D area){
		CallDeferred("newLevel");
	}
	
	public void newLevel(){
		if(GetNode("../").Name=="Level1"){
			GetTree().ChangeSceneToFile("Level2.tscn");
		}
		else if(GetNode("../").Name=="Level2"){
			GetTree().ChangeSceneToFile("Level3.tscn");
		}
		else {
			GetTree().ChangeSceneToFile("End.tscn");
		}
	}
}
