import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ConfirmationService, MenuItem } from 'primeng/api';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';

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

  constructor(
    public authService: AuthService,
    private router: Router,
    private confirmationService: ConfirmationService)
  { }

  ngOnInit(): void {

    console.log("ovdee:",this.authService.userValue)

    this.authService.loginStatusChange().subscribe(userSubject=>{

    this.items = [
      {
          label: 'O nama', routerLink:['/home']
      },
      {
          label: 'Predavanja', routerLink:['/predavanja']
      },
      {
          label: 'Predavači', routerLink:['/predavaci']
      },
    ];
    })
  }

  onClick()
  {
    this.confirmationService.confirm({
      message: 'Da li ste sigurni da želite da se odjavite?',
      accept: () => {
        this.authService.logout();
        console.log('OVO JE IZ HOME',this.authService.userValue)
        this.router.navigate(['/home']);
      }
    });
  }

  onLogin()
  {
    
  }

}
