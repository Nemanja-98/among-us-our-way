//import { loadProducts } from './store/actions/product.actions';
import { AccountService } from './services/account/account.service';
import { loadUsers } from './store/actions/user.actions';
import { State } from './store/reducers/root.reducer';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Role } from './models/role';
import { User } from './models/user';
import { Store } from '@ngrx/store';
import { Title } from '@angular/platform-browser';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app',
  templateUrl: 'app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title: string = 'Among Us';
  user: User;
  online: any;
  searchText = '';
  currentUser: any;

  constructor(
    private router: Router,
    private accountService: AccountService,
    private store: Store<State>,
    private titleService: Title,
    private http : HttpClient
  ) {
    this.accountService.user.subscribe((x) => (this.user = x));
  }

  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    console.log(this.currentUser);
  //  this.store.dispatch(new loadProducts());
    this.store.dispatch(new loadUsers());
    this.titleService.setTitle("AmongUs");
  }

  get isAdmin() {
    return this.user //&& this.user.role === Role.Admin;
  }

  logout() {
    this.accountService.logout();
  }
}
