import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-registration-form',
  templateUrl: './registration-form.component.html',
  styleUrls: ['./registration-form.component.css'],
})
export class RegistrationFormComponent implements OnInit, OnDestroy {
  form!: FormGroup;
  subscriptions: Subscription[] = [];
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private authService: AuthService,
    private messageService: MessageService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      name: [],
      surname: [],
      username: [],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
      role: ['Player'],
    });
  }
  onSubmit() {
    this.addUser();
  }
  addUser() {
    console.log(this.form.getRawValue());
    this.subscriptions.push(
      this.userService.addUser(this.form.getRawValue()).subscribe({
        next: (res: any) => {
          this.messageService.add({
            key: 'br',
            severity: 'success',
            summary: 'Uspešno',
            detail: 'Registracija je uspela!',
          });
        },
        error: (err: any) => {
          console.log(err.error);
          this.messageService.add({
            key: 'br',
            severity: 'error',
            summary: 'Neuspešno',
            detail: err.error,
          });
        },
      })
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
}
