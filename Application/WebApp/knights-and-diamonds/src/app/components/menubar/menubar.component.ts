import { Component, OnDestroy, OnInit } from '@angular/core';
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
export class MenubarComponent implements OnInit, OnDestroy {
  subscriptions: Subscription[] = [];
  items!: MenuItem[];
  admin: any = " ";
  player: any = " ";
  id!: number;
  userID=this.authService?.userValue?.id
  user: any;
  pom:any;
  isItShowen:any=false;
  constructor(
    public authService: AuthService,
    private router: Router,
    private confirmationService: ConfirmationService,
    private userService: UserService) { }

  ngOnInit(): void {

    console.log("ovdee user value:", this.authService.userValue)
    
    // if(this.userID!=undefined) {
    //   this.getUser();
    // }
    console.log("ovde ",this.authService?.userValue.id)
    this.subscriptions.push(
      this.authService.loginStatusChange().subscribe(userSubject => {
        console.log("ovdeeeeeee",userSubject);
        if(this.authService?.userValue?.role!="Admin" && this.authService?.userValue?.role!="Player"){
          console.log(this.authService?.userValue?.role);
          this.pom=false;
        }
        else{
          this.pom=true;
          this.userID=this.authService.userValue.id;
          this.getUser();
        }
        console.log("pomara",this.pom);
        // console.log("userSubject")
        // console.log(userSubject);
        // this.user = userSubject;

      })

    );
  }

  getUser() {
    this.subscriptions.push(
      this.userService.getUser(this.userID).subscribe({
        next: (res: any) => {
          this.user = res;
          console.log("beaaaaaaaaaaaaa",this.user)
        },
        error: err => {
          console.log(err.error.text);
        }
      })
    );
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
    this.isItShowen=false;
  }

  onRules() {
    this.router.navigate(['/rules']);
    this.isItShowen=false;
  }

  onCards() {
    this.router.navigate(['/cards']);
    this.isItShowen=false;
  }

  onCreateCard() {
    this.router.navigate(['/card-create']);
    this.isItShowen=false;
  }

  onProfile() {
    this.router.navigate(['/profil', this.authService.userValue?.id]);
    this.isItShowen=false;
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
  showenChange(){
    this.isItShowen=!this.isItShowen;
  }
  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
}
