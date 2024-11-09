import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { CartService } from './Services/CartService/cart.service';
import { filter } from 'rxjs';
import { Store } from '@ngrx/store';
import { fetchCart, fetchWishlist } from './Services/StoreManagement/global.actions';
import { BrowserStorageService } from './Services/BrowserStorage/browserstorage.service';
import { User } from './Models/User';
import { NavbarComponent } from "./Components/Common/navbar/navbar.component";
import { FooterComponent } from "./Components/Common/footer/footer.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent, FooterComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'BoughtIt';
  showNavbar:boolean = false;
  constructor(private router:Router,private store:Store,private browserStorge:BrowserStorageService){}
  ngOnInit(): void {
    this.router.events.pipe(
      filter(event=>event instanceof NavigationEnd)
    ).subscribe((event:NavigationEnd)=>{
      if(!event.url.includes('/login') && !event.url.includes('/register') && !event.url.includes('/auth-complete')){
        this.showNavbar=true;
        this.fetchUserCart();
        this.fetchUserWishlist();
      }
    })
  }

  fetchUserCart(){
    var user = JSON.parse(this.browserStorge.get('currentUser')||'') as User;
    this.store.dispatch(fetchCart({userId:user.id}));
  }
  fetchUserWishlist(){
    var user = JSON.parse(this.browserStorge.get('currentUser')||'') as User;
    this.store.dispatch(fetchWishlist({userId:user.id}));
  }
}
