import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { OnelineusersService } from 'src/app/services/onelineusers.service';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css']
})
export class LoginFormComponent implements OnInit, OnDestroy {
    form!: FormGroup;
    subscriptions: Subscription[] = [];
    loading = false;
    submitted = false;
    
  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private messageService: MessageService,
    public onelineusers:OnelineusersService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
  });
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub=>sub.unsubscribe())
  }

  onSubmit(){
    const userData = this.form.getRawValue();
    this.subscriptions.push(
      this.authService.login(userData).subscribe({
        next: (res: any)=>{
          this.router.navigate(['/home']);
          this.messageService.add({key: 'br', severity:'success', summary: 'Uspešno', detail: 'Prijava je uspela!'});
          console.log("ovdee",this.authService?.userValue?.id);`1`
          this.onelineusers.startConnection();
        },
        error: err=>{
          this.messageService.add({key: 'br', severity:'error', summary: 'Neuspešno', detail: 'Pokušajte ponovo, došlo je do greške.'});
        }
      })
    )

  }

  onRegistration(){
    
  }
  
}
