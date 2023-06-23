import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { DeckService } from 'src/app/services/deck.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit, OnDestroy {

  subscriptions: Subscription[] = [];
  id!: number;
  user: any;
  playerProfile: Boolean = false;
  isLogged: Boolean = false;

  constructor(
    public authService: AuthService,
    private route: ActivatedRoute,
    private userService: UserService,
    private deckService: DeckService,
    private router: Router) { }

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id']

    // console.log("u s e r c i n a",this.user.mainDeckID);
    this.getUser(this.id)

    if (this.id == this.authService?.userValue?.id) {
      this.playerProfile = true
    }
    if (this.id == this.authService?.userValue?.id && this.authService?.userValue?.role == 'Player') {
      this.isLogged = true
    }
  }

  getUser(userID: number) {
    this.subscriptions.push(
      this.userService.getUser(userID).subscribe((user) => {
        this.user = user;
        console.log("u s e r c i n a",this.user.mainDeckID);
        
        console.log(this.user)
      })
    );
  }

  redirectToDeck(){
    this.router.navigate(['/deck-create', this.user.mainDeckID]);
  }

  createDeck(){
    this.subscriptions.push(
      this.deckService.createDeck(this.user.id).subscribe({
        next: (res: any) => {
          this.getUser(this.user.id)
          console.log(this.user)
          console.log(this.user.mainDeckID)
        },
        error: err => {
          console.log(err.error.text);
        }
      })
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

}
