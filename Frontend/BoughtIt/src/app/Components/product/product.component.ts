import { Component, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../../Services/ProductService/product.service';
import { CommonModule } from '@angular/common';
import { BrowserStorageService } from '../../Services/BrowserStorage/browserstorage.service';
import { select, Store } from '@ngrx/store';
import { updateCart } from '../../Services/StoreManagement/global.actions';
import { selectIsProductInCart, selectIsProductInWishlist } from '../../Services/StoreManagement/global.selectors';
import { Observable, firstValueFrom, of, forkJoin } from 'rxjs';
import { CartService } from '../../Services/CartService/cart.service';

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent {
  public searchText: string = '';
  public productData: any[] | null = [];
  public wishlist: number[] = [];
  public fileBaseUrl = 'https://localhost:5000/static';
  public cartlist: number[] = [];
  public cartCount: number = 0;

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private browserService: BrowserStorageService,
    private store: Store,
    private cartService:CartService
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.searchText = params['searchText'] || '';
      this.loadProducts();
    });
  }
  async loadProducts() {
    try {
      const products = await firstValueFrom(this.productService.getFilteredProduct(this.searchText));
      
      if (!products || products.length === 0) {
        this.productData = [];
        return;
      }
      const productPromises = products.map(async (product) => {
        const [inWishlist, inCartlist] = await Promise.all([
          firstValueFrom(this.cartService.isInWishlist(product.productId)),
          firstValueFrom(this.cartService.isInCartlist(product.productId))
        ]);

        return {
          ...product,
          inWishlist,
          inCartlist
        };
      });
      this.productData = await Promise.all(productPromises);

    } catch (error) {
      console.error('Error loading products:', error);
    }
  }

  addToWishlist(productId: number, flag: boolean) {
    if (productId == null) return;
    this.cartService.addToWishlist(productId,flag);
    this.productData = this.productData?.map(product =>
      product.productId === productId
        ? { ...product, inWishlist: flag }
        : product
    ) || null;
  }

  addToCart(productId: number, flag: boolean) {
    if (productId == null) return;
    this.cartService.addToCart(productId,1,flag);
    this.productData = this.productData?.map(product =>
      product.productId === productId
        ? { ...product, inCartlist: flag }
        : product
    ) || null;
  }
}
