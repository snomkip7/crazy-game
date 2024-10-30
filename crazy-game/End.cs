using Godot;
using System;

public partial class End : Node2D
{
	public Timer timer;
	public CanvasModulate screenColor;
	public byte sectionpart = 0;
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
		timer = GetNode<Timer>("Timer");
		screenColor = GetNode<CanvasModulate>("ScreenColor");
	}

    public override void _Process(double delta)
    {
        if(sectionpart==2){
			screenColor.Color = new Color(screenColor.Color.R-.005f, screenColor.Color.G-.005f, screenColor.Color.B-.005f); 
			if(screenColor.Color.R<=0.005f){
				screenColor.Color = new Color(0,0,0);
				sectionpart = 3;
				GetNode<Node2D>("Setting1").Visible = false;
				GetNode<Node2D>("Setting2").Visible = true;
			}
		} else if(sectionpart==3){
			screenColor.Color = new Color(screenColor.Color.R+.003f, screenColor.Color.G+.003f, screenColor.Color.B+.003f); 
			if(screenColor.Color.R>=1){
				screenColor.Color = new Color(1,1,1);
				sectionpart = 4;
			}
		}
    }

    public void nextPart(){
		if(sectionpart==0){
			sectionpart++;
			GetNode<Label>("Setting1/WaitLabel").Visible = true;
			timer.Start(5);
		} else if(sectionpart==1){
			sectionpart++;
			GetNode<Label>("Setting1/FinalLabel").Visible = true;
			screenColor.Color = new Color(1,1,1);
			timer.Start(5);
		} else if(sectionpart==2){
			// do nothing lol
		} else if(sectionpart==3){
			timer.Start(30);
		} else{
			GetTree().ChangeSceneToFile("mainMenu.tscn");
		}
	}
}
