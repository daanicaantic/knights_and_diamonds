import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Route, Router } from '@angular/router';
import { IngameService } from 'src/app/services/ingame.service';
@Component({
  selector: 'app-loadinscreen',
  templateUrl: './loadinscreen.component.html',
  styleUrls: ['./loadinscreen.component.css']
})
export class LoadinscreenComponent implements OnInit {
  value=0;
  isTimerOver=false;
  @Input() loadingType:any;
  @Output() onTimerOverTask: EventEmitter<Boolean> = new EventEmitter();

  
  constructor(
    public router:Router,
    public inGameService:IngameService
  ) { }

  ngOnInit(): void {
    this.inGameService.setGameOn();
    let interval = setInterval(() => {
      this.value = this.value + Math.floor(Math.random() * 10) + 1;
      if (this.value >= 100) {
          this.value = 100;
          this.isTimerOver=true;
          this.onTimerOver();
          clearInterval(interval);
          // this.router.navigate(['/game']);
      }
  }, 700);
  }
  onTimerOver() {
    this.onTimerOverTask.emit(this.isTimerOver);
  }

}
