import { Component, Input, OnInit } from '@angular/core';
import { User } from '../../../Models/User';
import { UserService } from '../../../Services/UserService/user.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { Store } from '@ngrx/store';
import { selectCartCount, selectWishlist, selectWishlistCount } from '../../../Services/StoreManagement/global.selectors';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule,FormsModule,RouterModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit{
  public user!:User|null;
  public searchText:string=''
  public cartCount$:Observable<number>;
  public wishlistCount$:Observable<number>;
  constructor(private userService:UserService,private router:Router,private store:Store){
    this.userService.getUserData().subscribe({
      next:(response)=>{
        this.user=response;
      }
    });
    this.cartCount$ = of(0);
    this.wishlistCount$  = of(0);
  }
  ngOnInit(): void {
    this.cartCount$ = this.store.select(selectCartCount)
    this.wishlistCount$ = this.store.select(selectWishlistCount);
  }
  searchProduct(){
    if(this.searchText=='')return;
    this.router.navigate(['/products'], { queryParams: { searchText: this.searchText } });
  }
}
