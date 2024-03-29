import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { AuthGuard } from './services/auth.guard';
import { CardCreateComponent } from './components/card-create/card-create.component';
import { HomeComponent } from './components/home/home.component';
import { CardComponent } from './components/card/card.component';
import { RegistrationFormComponent } from './components/registration-form/registration-form.component';
import { GameComponent } from './components/game/game.component';
import { RpsGameComponent } from './components/rps-game/rps-game.component';
import { UserProfileComponent } from './components/user-profile/user-profile.component';
import { Error404Component } from './components/error404/error404.component';
import { WelcomePageComponent } from './components/welcome-page/welcome-page.component';
import { LoadinscreenComponent } from './components/loadinscreen/loadinscreen.component';
import { DeckCreateComponent } from './components/deck-create/deck-create.component';
import { CardsComponent } from './components/cards/cards.component';
import { RulesComponent } from './components/rules/rules.component';

const routes: Routes = [
  {
    path: 'login',
    component: LoginFormComponent,
  },
  {
    path: 'register',
    component: RegistrationFormComponent,
  },
  {
    path: 'welcome',
    component: WelcomePageComponent,
  },
  {
    path: 'rules',
    component: RulesComponent,
  },
  {
    path: 'card-create',
    component: CardCreateComponent,
    canActivate: [AuthGuard],
    data: { roles: ['Admin'] },
  },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [AuthGuard],
    data: { roles: ['Player', 'Admin'] },
  },
  {
    path: 'profil/:id',
    component: UserProfileComponent,
    canActivate: [AuthGuard],
    data: { roles: ['Player', 'Admin'] },
  },
  {
    path: 'cards',
    component: CardsComponent,
  },
  {
    path: 'deck-create/:deckID',
    component: CardsComponent,
    
    
  },
  {
    path: 'rpsGame/:rpsGameID',
    component: RpsGameComponent,
  },
  {
    path: 'game/:gameID',
    component: GameComponent,
  },
  {
    path: 'loading',
    component: LoadinscreenComponent,
  },
  {
    path: '',
    redirectTo: 'welcome',
    pathMatch: 'full',
  },
  {
    path: '**',
    redirectTo: 'error404',
  },
  {
    path: 'error404',
    component: Error404Component,
  },
];

@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
