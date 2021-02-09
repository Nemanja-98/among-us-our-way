import { UserProfileComponent } from './components/users/user-profile/user-profile.component';
import { HomeComponent } from './components/home/home.component';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './helpers/auth.guard';
import { NgModule } from '@angular/core';
import { Role } from './models/role';
import { ChatComponent } from './components/chat/chat.component';

const accountModule = () =>
  import('./components/account/account/account.module').then(
    (x) => x.AccountModule
  );
const usersModule = () =>
  import('./components/users/users/users.module').then((x) => x.UsersModule);


const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'chat',
    component: ChatComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'profile',
    component: UserProfileComponent,
    canActivate: [AuthGuard],
  },
  { path: 'users', loadChildren: usersModule, canActivate: [AuthGuard] },
  { path: 'account', loadChildren: accountModule },
 
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
