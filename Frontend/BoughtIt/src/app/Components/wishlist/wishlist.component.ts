import { Component, OnInit } from '@angular/core';
import { LoadingSpinnerComponent } from "../Common/LoadingSpinner/loading-spinner.component";
import { Product } from '../../Models/Product';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BrowserStorageService } from '../../Services/BrowserStorage/browserstorage.service';
import { CartService } from '../../Services/CartService/cart.service';
import { ProductService } from '../../Services/ProductService/product.service';
import { BehaviorSubject, firstValueFrom, forkJoin } from 'rxjs';
import { ProductCardComponent } from "../product-card/product-card.component";

@Component({
  selector: 'app-wishlist',
  standalone: true,
  imports: [LoadingSpinnerComponent, FormsModule, CommonModule, ProductCardComponent],
  templateUrl: './wishlist.component.html',
  styleUrl: './wishlist.component.css'
})
export class WishlistComponent implements OnInit {
  public isLoading:boolean =false;
  public wishlistItems$ =new BehaviorSubject<(Product & { inWishlist: boolean; inCartlist: boolean })[] | null>(null);
  public fileBaseUrl = 'https://localhost:5000/static/';
  public errorMessage='';
  constructor(private browserStorage:BrowserStorageService,private wishlistService:CartService,
    private productService:ProductService
  ){}
  ngOnInit(): void {
    this.loadWishlist();
  }
  loadWishlist(){
    const userData = this.browserStorage.get('currentUser');
    if (!userData) {
      this.errorMessage = 'User data not found. Please log in.';
      this.isLoading = false;
      return;
    }
    const user = JSON.parse(userData);
    this.isLoading=true;
    this.wishlistService.getUserWishlist(user.id).subscribe({
      next: (wishlistItems: number[]) => {
        if (wishlistItems.length === 0) {
          this.isLoading = false;
          this.wishlistItems$.next([]);
          return;
        }
    
        const productRequests = wishlistItems.map(item => 
          this.productService.getProductById(item)
        );
    
        forkJoin(productRequests).subscribe({
          next: async (products: (Product | null)[]) => {
            
            const productPromises = products.map(async (product) => {
              if (product == null) return null;
    
              const [inWishlist, inCartlist] = await Promise.all([
                firstValueFrom(this.wishlistService.isInWishlist(product.productId)),
                firstValueFrom(this.wishlistService.isInCartlist(product.productId))
              ]);
    
              return {
                ...product,
                inWishlist,
                inCartlist
              };
            });
    
            const resolvedProducts = await Promise.all(productPromises);
            this.wishlistItems$.next(resolvedProducts.filter(p => p !== null)); 
            this.isLoading = false;
          }
        });
      },
      error: (error) => {
        console.log(error);
        this.isLoading = false;
        this.wishlistItems$.next(null);
      }
    });
  }
  addToWishlist(productId: number, flag: boolean) {
    if (productId == null) return;
    this.wishlistService.addToWishlist(productId, flag);
    const currentItems = this.wishlistItems$.getValue();
    const updatedItems = currentItems?.map(product =>
      product.productId === productId
        ? { ...product, inWishlist: flag }
        : product
    ) || null;
    this.wishlistItems$.next(updatedItems);
  }
  addToCart(productId:number,flag:boolean){
    if (productId == null) return;
    this.wishlistService.addToCart(productId,1,flag);
    const currentItems = this.wishlistItems$.getValue();
    const updatedItems = currentItems?.map(product=>
      product.productId === productId?{...product,inCartlist:flag}
      :product
    )||null;
    this.wishlistItems$.next(updatedItems);
  }
}
