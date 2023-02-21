import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { timer } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent implements OnInit {
  gameID!: number;
  user1: any;
  user2: any;
  userID = this.authService?.userValue?.id;
  progressValue!:number;
  loadingBar:any;
  gameStarted:Boolean=false;
  timer:any;
  timerCountDown:any;
  timerContainerWidth:any;

  constructor(private route: ActivatedRoute,
    private authService: AuthService) { }

  ngOnInit(): void {
    this.progressValue = 0;
    let progressEndValue = 100;
    let speed = 50;
    this.timer = 30;


    let progress = setInterval(() => {

      this.progressValue++;
      this.loadingBar='conic-gradient(red '+this.progressValue*3.6+'deg,orange '+this.progressValue*3.6+ 'deg)';

      if (this.progressValue == progressEndValue) {
        clearInterval(progress);
        this.gameStarted=true;
        this.progressValue=0;
        progress=setInterval(()=>
        {
          console.log(this.timer,"caocao")
          this.timer--;
          this.timerCountDown=this.timer*3.33333333333 + "%"
          console.log("odje",this.timerCountDown);
          if(this.timer===this.progressValue)
          {
            console.log(this.timer,"caocao")
            clearInterval(progress);
          }

        },1000)
      }
      speed=50;
    }, speed);
  }
  ChooseMove(move:String)
  {
    console.log(move);
  }
}
