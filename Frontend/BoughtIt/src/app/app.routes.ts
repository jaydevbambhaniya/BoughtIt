import { Routes } from '@angular/router';
import { LoginComponent } from './Components/login/login.component';
import { RegisterComponent } from './Components/register/register.component';
import { HomeComponent } from './Components/home/home.component';
import { LoadingSpinnerComponent } from './Components/Common/LoadingSpinner/loading-spinner.component';
import { AlertComponent } from './Components/Common/Alert/alert.component';
import { ProductComponent } from './Components/product/product.component';
import { CartComponent } from './Components/cart/cart.component';
import { WishlistComponent } from './Components/wishlist/wishlist.component';
import { OrderComponent } from './Components/order/order.component';
import { OrderdetailsComponent } from './Components/orderdetails/orderdetails.component';
import { AccountComponent } from './Components/account/account.component';
import { authGuard } from './Services/AuthGuard/auth.guard';
import { AuthCompleteComponent } from './Components/auth-complete/auth-complete.component';
import { ProductInfoComponent } from './Components/product-info/product-info.component';

export const routes: Routes = [
    {path:'login',component:LoginComponent},
    {path:'register', component:RegisterComponent},
    {path:'home', component:HomeComponent, canActivate:[authGuard]},
    {path:'products',component:ProductComponent, canActivate:[authGuard]},
    {path:'cart',component:CartComponent, canActivate:[authGuard]},
    {path:'wishlist', component:WishlistComponent, canActivate:[authGuard]},
    {path:'order',component:OrderComponent, canActivate:[authGuard]},
    {path:'orderDetails',component:OrderdetailsComponent, canActivate:[authGuard]},
    {path:'user',component:AccountComponent, canActivate:[authGuard]},
    {path:'userOrders',component:OrderdetailsComponent, canActivate:[authGuard]},
    {path:'auth-complete',component:AuthCompleteComponent},
    {path:'product-info',component:ProductInfoComponent},
    {path:'**', redirectTo:'login'}
];
