import { Component, Input } from '@angular/core';
import { Product } from '../../Models/Product';
import { CommonModule } from '@angular/common';
import { CartService } from '../../Services/CartService/cart.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-card.component.html',
  styleUrl: './product-card.component.css'
})
export class ProductCardComponent {
  @Input() product!:((Product & {inCartlist:boolean,inWishlist:boolean}));
  public fileBaseUrl = 'https://localhost:5000/static';

  constructor(
    private cartService:CartService,
    private router:Router
  ){}

  addToWishlist(productId: number, flag: boolean) {
    if (productId == null) return;
    this.cartService.addToWishlist(productId,flag);
    if(this.product!=null)
      this.product.inWishlist=flag;
  }
  productDetails(productId:number){
    if(productId==null)return;
    this.router.navigateByUrl('/product-info?productId='+productId);
  }
  addToCart(productId: number, flag: boolean) {
    if (productId == null) return;
    this.cartService.addToCart(productId,1,flag);
    if(this.product!=null)
      this.product.inCartlist = flag;
  }
}
