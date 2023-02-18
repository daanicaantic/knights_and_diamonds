import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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

  constructor(private route: ActivatedRoute,
    private authService: AuthService) { }

  ngOnInit(): void {
    this.gameID = this.route.snapshot.params['GameID']
    console.log('ID GAME',this.gameID)
  }

}
