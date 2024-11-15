import { User } from '../../Models/User';
import { FormsModule } from '@angular/forms';
import { LoadingSpinnerComponent } from "../Common/LoadingSpinner/loading-spinner.component";
import { forkJoin, Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { Product } from '../../Models/Product';
import { ProductService } from '../../Services/ProductService/product.service';
import { CartService } from '../../Services/CartService/cart.service';
import { BrowserStorageService } from '../../Services/BrowserStorage/browserstorage.service';
import { initialCartState } from '../../Services/StoreManagement/global.state';
import { selectCartItems } from '../../Services/StoreManagement/global.selectors';
import { select, Store } from '@ngrx/store';
import { CartProductDto } from '../../Models/CartProductDto';
import { Router, RouterModule } from '@angular/router';
import { updateCart } from '../../Services/StoreManagement/global.actions';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, FormsModule, LoadingSpinnerComponent,RouterModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit {
  public cartProducts: Product[] = [];
  isLoading = true;
  errorMessage = '';
  public fileBaseUrl = 'https://localhost:5000/static/';
  private userData!:User;
  constructor(
    private productService: ProductService,
    private cartService: CartService,
    private browserStorage: BrowserStorageService,
    private store:Store,
    private router:Router
  ) {
  }

  ngOnInit() {
    this.userData = JSON.parse(this.browserStorage.get('currentUser') || '') as User;
    this.loadCartProducts();
  }

  loadCartProducts() {
    if (!this.userData) {
      this.errorMessage = 'User data not found. Please log in.';
      this.isLoading = false;
      return;
    }
    this.cartService.getCartProducts(this.userData.id).subscribe({
      next: (cartItems) => {
        if (cartItems.length === 0) {
          this.isLoading = false;
          return;
        }

        const productRequests = cartItems.map(item => 
          this.productService.getProductById(item.productId)
        );

        forkJoin(productRequests).subscribe({
          next: (products: (Product | null)[]) => {
            
            this.cartProducts = products
              .filter((product): product is Product => product !== null) 
              .map((product, index) => ({
                ...product, 
                quantity: cartItems[index].quantity
              }));
            this.isLoading = false;
          },
          error: (err) => {
            console.error('Error fetching product details:', err);
            this.errorMessage = 'Failed to load product details. Please try again.';
            this.isLoading = false;
          }
        });
      },
      error: (err) => {
        console.error('Error fetching cart items:', err);
        this.errorMessage = 'Failed to load cart items. Please try again.';
        this.isLoading = false;
      }
    });
  }

  getTotal(): number {
    return this.cartProducts.reduce((sum, product) => sum + (product.price * product.quantity), 0);
  }

  removeProduct(productId: number) {
    var product = new CartProductDto();
    product.productId=productId;
    this.cartProducts = this.cartProducts.filter(p=>p.productId!=productId);
    this.store.dispatch(updateCart({userId:this.userData.id,item:product,isInCart:false}));
  }

  updateQuantity(product: Product) {
    var productDto = new CartProductDto();
    productDto.productId=product.productId;
    productDto.quantity = product.quantity;
    this.store.dispatch(updateCart({userId:this.userData.id,item:product,isInCart:true}));
  }
  onPlaceOrderClick(){
    this.cartService.setProductsToCheckout(this.cartProducts);
    this.router.navigateByUrl('/order');
  }
}