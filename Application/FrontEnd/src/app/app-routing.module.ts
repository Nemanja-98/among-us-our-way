import { UserProfileComponent } from './components/users/user-profile/user-profile.component';
import { HomeComponent } from './components/home/home.component';
import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { Role } from './models/role';
import { ChatComponent } from './components/chat/chat.component';
import { LoginComponent } from './components/account/login/login.component';

const accountModule = () =>
  import('./components/account/account/account.module').then(
    (x) => x.AccountModule
  );
const usersModule = () =>
  import('./components/users/users/users.module').then((x) => x.UsersModule);


const routes: Routes = [
  {
    path: '',
    component: LoginComponent,
  },
  {
    path: 'home',
    component: HomeComponent,
  },
  {
    path: 'chat',
    component: ChatComponent,
  },
  {
    path: 'profile',
    component: UserProfileComponent,
  },
  { path: 'users', loadChildren: usersModule },
  { path: 'account', loadChildren: accountModule },
 
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
