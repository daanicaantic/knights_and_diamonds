import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ConfirmationService, MenuItem } from 'primeng/api';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-menubar',
  templateUrl: './menubar.component.html',
  styleUrls: ['./menubar.component.css']
})
export class MenubarComponent implements OnInit {
  subscriptions: Subscription[] = [];
  items!: MenuItem[];
  admin: any = " ";
  player: any = " ";
  id!: number;
  userID=this.authService?.userValue?.id
  user: any;

  constructor(
    public authService: AuthService,
    private router: Router,
    private confirmationService: ConfirmationService,
    private userService: UserService) { }

  ngOnInit(): void {

    console.log("ovdee user value:", this.authService.userValue)
    
    if(this.userID!=undefined) {
      this.getUser();
    }

    this.authService.loginStatusChange().subscribe(userSubject => {});
  }

  getUser() {
    this.userService.getUser(this.userID).subscribe({
      next: (res: any) => {
        this.user = res;
        console.log(this.user)
      },
      error: err => {
        console.log(err.error.text);
      }
    })
  }

  onSignUp() {
    this.router.navigate(['/register']);
  }

  onLogo() {
    this.router.navigate(['/welcome']);
  }

  onHome() {
    if(this.user != undefined) {
      this.router.navigate(['/home']);
    }
    else this.router.navigate(['/welcome']);
  }

  onRules() {
    if(this.user != undefined) {
      this.router.navigate(['/home']);
    }
    else this.router.navigate(['/welcome']);
  }

  onCards() {
    if(this.user != undefined) {
      this.router.navigate(['/home']);
    }
    else this.router.navigate(['/welcome']);
  }

  onProfile() {
    this.router.navigate(['/profil', this.authService.userValue?.id]);
  }

  onLogout() {
    this.confirmationService.confirm({
      message: 'Are you sure you want to log out?',
      accept: () => {
        this.authService.logout();
        this.router.navigate(['/welcome']);
      }
    });
  }

}
